using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using GameObjects;

namespace PlanetesWPF
{
    public enum RecordingState {Ready, Recording, Saving, Complete}

    public class GameRecorder : IDisposable
    {

        public event  Action<GameRecorder> OnSaveComplete;

        public RecordingState State { get; set; } =  RecordingState.Ready;
        
        BlockingCollection<byte[]> frames;
        GifBitmapEncoder encoder;

        byte[] aPixels;
        int PixelWidth;
        int PixelHeight;
        double DpiX;
        double DpiY;
        PixelFormat Format;
        BitmapPalette Palette;       
        int Stride;
        private int _framerate;

        public double ScaleFactor { get; set; } = 0.666;

        public int LastDrawnFrame { get; internal set; }

        public GameRecorder(WriteableBitmap imageSample)
        {
            frames = new BlockingCollection<byte[]>();
            aPixels = new byte[imageSample.PixelHeight * imageSample.BackBufferStride];
            PixelWidth = imageSample.PixelWidth;
            PixelHeight = imageSample.PixelHeight;
            DpiX = imageSample.DpiX;
            DpiY = imageSample.DpiY;
            Format = imageSample.Format;
            Palette = imageSample.Palette;
            Stride = imageSample.BackBufferStride;
        }

        public void AddFrame(WriteableBitmap imageSource)
        {
            if (State == RecordingState.Recording)
            {
                imageSource.CopyPixels(aPixels, imageSource.BackBufferStride, 0);
                frames.Add(aPixels);
            }
        }

        public void Feed(object Image)
        {
            encoder = new GifBitmapEncoder();
            foreach (var pixels in frames.GetConsumingEnumerable())
            {
                BitmapSource BF = BitmapSource.Create(PixelWidth, PixelHeight,
                DpiX, DpiY, Format, Palette, pixels, Stride);
                if (ScaleFactor != 1.0f)
                {
                    BF = new TransformedBitmap(BF, new ScaleTransform(ScaleFactor, ScaleFactor));
 }
                encoder.Frames.Add(BitmapFrame.Create(BF));
               
            }
            Save();
        }       

        public void Start()
        {            
            if (State is RecordingState.Ready )
            {
                Logger.Log($"Recording game to gif ({PixelWidth}x{PixelHeight} )...", LogLevel.Status);
                State = RecordingState.Recording;
                ThreadPool.QueueUserWorkItem(new WaitCallback(Feed));
            }
        }

        public void End()
        {
            State = RecordingState.Saving;
            frames.CompleteAdding();
        }

        private void Save()
        {
            //TODO: use this library to encode gifs instead:  https://github.com/mrousavy/AnimatedGif
            string filename = string.Format("game {0}_{1}.gif", DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"),GameConfig.TossInt(999));
            using (FileStream stream = new FileStream(filename, FileMode.Create))
            {
                try
                {
                    Logger.Log("Saving Gif...", LogLevel.Status);
                    Save(stream);
                }
                catch (Exception e)
                {
                    Logger.Log(e, LogLevel.Debug);
                }
            }

            State = RecordingState.Complete; //  IsComplete = true;
            OnSaveComplete(this);
            Logger.Log("done saving", LogLevel.Status);
            Dispose();
        }

        internal bool Fits(WriteableBitmap imageSample)
        {
            return aPixels.Length == imageSample.PixelHeight * imageSample.BackBufferStride;
        }
        
        public void Dispose()
        {
            OnSaveComplete = null;
            frames.Dispose();
            encoder = null;
            aPixels = null;
        }

        /// <summary>
        ///     ''' Writes the animated GIF binary to a specified IO.Stream
        ///     ''' </summary>
        ///     ''' <param name="Stream">The stream where the binary is to be output. Can be any object type that derives from IO.Stream</param>
        public void Save(Stream Stream)
        {
            byte[] Data;


            if (encoder.Frames.Count != 0)
            {
                // Get the raw binary
                using (MemoryStream MStream = new MemoryStream())
                {
                    encoder.Save(MStream);
                    Data = MStream.ToArray();
                }
            }
            else
                throw new Exception("Cannot encode the Gif. The frame collection is empty.");
         
            // Locate the right location where to insert the metadata in the binary
            // This will be just before the first label &H0021F9 (Graphic Control Extension)
            int MetadataPTR = -1;
            int flag = 0;
            do
            {
                MetadataPTR += 1;
                if (Data[MetadataPTR] == 0)
                {
                    if (Data[MetadataPTR + 1] == 0x21)
                    {
                        if (Data[MetadataPTR + 2] == 0xF9)
                            flag = 1;
                    }
                }
            }
            while (flag == 0);

            // SET METADATA Repeat
            // This add an Application Extension Netscape2.0
            if (Repeat)
            {
                byte[] Temp = new byte[System.Convert.ToInt32(Data.Length) - 1 + 19 + 1];
                // label: &H21, &HFF + one byte: length(&HB) + NETSCAPE2.0 + one byte: Datalength(&H3) + {1, 0, 0} + Block terminator, 1 byte, &H00
                byte[] ApplicationExtension = new byte[] { 0x21, 0xFF, 0xB, 0x4E, 0x45, 0x54, 0x53, 0x43, 0x41, 0x50, 0x45, 0x32, 0x2E, 0x30, 0x3, 0x1, 0x0, 0x0, 0x0 };
                Array.Copy(Data, Temp, MetadataPTR);
                Array.Copy(ApplicationExtension, 0, Temp, MetadataPTR + 1, 19);
                Array.Copy(Data, MetadataPTR + 1, Temp, MetadataPTR + 20, Data.Length - MetadataPTR - 1);
                Data = Temp;
            }

            // SET METADATA Comments
            // This add a Comment Extension for each string
            if (MetadataString.Count > 0)
            {
                foreach (string Comment in MetadataString)
                {
                    if (!string.IsNullOrEmpty(Comment))
                    {
                        string TheComment;
                        if (Comment.Length > 254)
                            TheComment = Comment.Substring(0, 254);
                        else
                            TheComment = Comment;
                        byte[] CommentStringBytes = System.Text.UTF7Encoding.UTF7.GetBytes(TheComment);
                        byte[] DataString = new byte[] { 0x21, 0xFE, System.Convert.ToByte(CommentStringBytes.Length) };
                        DataString = DataString.Concat(CommentStringBytes).Concat(new byte[] { 0x0 }).ToArray();
                        byte[] Temp = new byte[Data.Length - 1 + DataString.Length + 1];
                        Array.Copy(Data, Temp, MetadataPTR);
                        Array.Copy(DataString, 0, Temp, MetadataPTR + 1, DataString.Length);
                        Array.Copy(Data, MetadataPTR + 1, Temp, MetadataPTR + DataString.Length + 1, Data.Length - MetadataPTR - 1);
                        Data = Temp;
                    }
                }
            }

            // SET METADATA frameRate
            // Sets the third and fourth byte of each Graphic Control Extension (5 bytes from each label 0x0021F9)
            for (int x = 0; x <= Data.Count() - 1; x++)
            {
                if (Data[x] == 0)
                {
                    if (Data[x + 1] == 0x21)
                    {
                        if (Data[x + 2] == 0xF9)
                        {
                            if (Data[x + 3] == 4)
                            {
                                // word, little endian, the hundredths of second to show this frame
                                byte[] Bte = BitConverter.GetBytes(FrameRate);
                                Data[x + 5] = Bte[0];
                                Data[x + 6] = Bte[1];
                            }
                        }
                    }
                }
            }
            Stream.Write(Data, 0, Data.Length);
        }

        /// <summary>
        ///''' Return the GIF specification version. This always returns "GIF89a"
        ///''' </summary>
        public string EncoderVersion
        {
            get
            {
                return "GIF89a";
            }
        }

        /// <summary>
        ///   ''' Get or set a value that indicate if the GIF will repeat the animation after the last frame is shown. The default value is True
        ///   ''' </summary>
        public bool Repeat { get; set; } = true;

        /// <summary>
        ///     ''' Get or set a collection of metadata string to be embedded in the GIF file. Each string has a max length of 254 
        ///     ''' characters (Any character above this limit will be truncated). The string will be encoded UTF-7. 
        ///     ''' </summary>
        public List<string> MetadataString { get; set; } = new List<string>();
        
        /// <summary>
        ///     '''  Get or set the amount of time each frame will be shown (in tens of milliseconds). The default value is 200ms
        ///     ''' </summary>
        public int FrameRate { get => _framerate; set => _framerate = Math.Max(2,value); }  //(int)(GameTime.DeltaTime);//  (int)GameConfig.FrameInterval.TotalMilliseconds/10;
    }
}

