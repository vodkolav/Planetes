using GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;

namespace PlanetesWPF
{
    public class RecorderController
    {
        byte[] aPixels;
        List<GameRecorder> cassetes = new List<GameRecorder>(10);
        GameRecorder current;
        WriteableBitmap imageSample;
        WPFGraphicsContainer graphicsContainer;
        public RecorderController(WPFGraphicsContainer gc )
        {
            graphicsContainer = gc;
            WriteableBitmap imageSample = gc.CurrentView;
            aPixels = new byte[imageSample.PixelHeight * imageSample.BackBufferStride];
            this.imageSample = imageSample;
            current = new GameRecorder(imageSample);
            cassetes.Add(current);
        }

        internal void Start()
        {
            Logger.Log("Cassettes: "+ cassetes.Count(),LogLevel.Info);
            current.Start();
        }

        public void AddFrame( int frameNum)
        {
            WriteableBitmap Source = graphicsContainer.CurrentView;
            if (current.IsRecording)
                if (Source != null)
                {
                    //Im gonna need this here : 
                    //stackoverflow.com/questions/8763761/threading-communication-between-two-threads-c-sharp

                    // This is also a nice thread about doing the whole rendering writeblebitmap on another thread: 
                    //https://stackoverflow.com/questions/9868929/how-to-edit-a-writablebitmap-backbuffer-in-non-ui-thread
                    if (!(Source.Width == 0) && !(Source.Height == 0))
                    {
                        Source.CopyPixels(aPixels, Source.BackBufferStride, 0);
                        current.AddFrame(aPixels);
                        //encoder.Frames.Add(BitmapFrame.Create(Source.CloneCurrentValue()));                       
                    }
                    else
                        throw new ArgumentException("Argument Frame, The bitmap size cannot be zero");
                }
                else
                    throw new ArgumentException("Argument Frame cannot be nothing");
        }


        internal void End()
        {
            current.End();

            try
            {
                current = cassetes.First(c => !c.IsRecording);
            }
            catch
            {
                current = new GameRecorder(imageSample);
                cassetes.Add(current);
            }
        }
    }
}
