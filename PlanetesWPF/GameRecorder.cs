using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace PlanetesWPF
{
    public class GameRecorder
    {
        bool IsRecording { get; set; } = false;
        BlockingCollection<byte[]> frames;
        Thread feeder;
        GifBitmapEncoder encoder;

        int PixelWidth;
        int PixelHeight;
        double DpiX;
        double DpiY;
        PixelFormat Format;
        BitmapPalette Palette;       
        int Stride;

        public void AddFrame(WriteableBitmap Source)
        {
            lock (this)
            {
                if (IsRecording)
                    if (Source != null)
                    {
                        //Im gonna need this here : 
                        //stackoverflow.com/questions/8763761/threading-communication-between-two-threads-c-sharp

                        if (!(Source.Width == 0) && !(Source.Height == 0))
                        {

                            int fSB = Source.PixelHeight * Source.BackBufferStride;

                            byte[] aPixels = new byte[fSB];
                            Source.CopyPixels(aPixels, Source.BackBufferStride, 0);
                            frames.Add(aPixels);
                            //encoder.Frames.Add(BitmapFrame.Create(Source.CloneCurrentValue()));                       
                        }
                        else
                            throw new ArgumentException("Argument Frame, The bitmap size cannot be zero");
                    }
                    else
                        throw new ArgumentException("Argument Frame cannot be nothing");
            }
        }


        public void Feed(object Image)
        {
            encoder = new GifBitmapEncoder();
            foreach (var aPixels in frames.GetConsumingEnumerable())
            {
                if (IsRecording)
                {
                    BitmapSource BF = BitmapSource.Create(PixelWidth, PixelHeight,
                    DpiX, DpiY, Format, Palette, aPixels, Stride);
                    encoder.Frames.Add(BitmapFrame.Create(BF));
                }
                else
                {
                    break;
                }
            }
            Save();
        }

        public GameRecorder(WriteableBitmap imageSample)
        {
            frames = new BlockingCollection<byte[]>();

            PixelWidth = imageSample.PixelWidth;
            PixelHeight = imageSample.PixelHeight;
            DpiX = imageSample.DpiX;
            DpiY = imageSample.DpiY;
            Format = imageSample.Format;
            Palette = imageSample.Palette;
            Stride = imageSample.BackBufferStride;
            feeder = new Thread(Feed)
            {
                Name = "Recorder Thread"
            };
        }

        public void Start()
        {
            if (!IsRecording)
            {
                IsRecording = true;
                feeder.Start(this);
            }
        }

        public void End()
        {
            IsRecording = false;
            frames.CompleteAdding();
        }

        private void Save()
        {
            string filename = string.Format("game {0}.gif", DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss"));
            using (FileStream stream = new FileStream(filename, FileMode.Create))
            {
                try
                {
                    Save(stream);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            Console.WriteLine("done saving");
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
                                byte[] Bte = BitConverter.GetBytes(FrameRate / 10);
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
        ///     '''  Get or set the amount of time each frame will be shown (in milliseconds). The default value is 200ms
        ///     ''' </summary>
        public int FrameRate { get; set; } = 50;
    }
}

