using GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Imaging;

namespace PlanetesWPF
{
    public class RecorderController
    {
        List<GameRecorder> cassetes = new List<GameRecorder>(10);
        GameRecorder current;
        WPFGraphicsContainer graphicsContainer;
        private WriteableBitmap Source => graphicsContainer.CurrentView;

        internal bool isRecording { get {  return current.State == RecordingState.Recording; } }

        public RecorderController(WPFGraphicsContainer gc )
        {
            graphicsContainer = gc;
            graphicsContainer.PropertyChanged += GraphicsContainer_PropertyChanged;
            ManageCassette();
        }

        private void GraphicsContainer_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "CurrentView":
                    ManageCassette();
                    break;
            }
        }

        public void ManageCassette()
        {
            if (current != null && current.State == RecordingState.Recording)
                return;
            try
            {
                current = cassetes.First(c => c.State == RecordingState.Ready && c.Fits(Source));
            }
            catch
            {
                current = new GameRecorder(graphicsContainer.CurrentView);
                current.OnSaveComplete +=  ClearCassetes;
                current.FrameRate = 2;
                cassetes.Add(current);
            }
            string tmp = graphicsContainer.CurrentView.Width.ToString() + "x" + graphicsContainer.CurrentView.Height.ToString();
        }

        public void ClearCassetes(GameRecorder cur)
        {
            cur.OnSaveComplete -= ClearCassetes;
            cassetes.RemoveAll(c => c.State == RecordingState.Complete );        
        }

        internal void Start()
        {
            Logger.Log("Cassettes: "+ cassetes.Count,LogLevel.Status);
            ManageCassette();
            current.Start();
        }

        public void AddFrame(int frameNum)
        {
            //TODO: add frame delay, so that gif is smoother
            if (current.State == RecordingState.Recording)
            {
                if (current.LastDrawnFrame >= frameNum) // && frameNum % 4 != 0)
                {
                    return;
                }

                if (Source != null)
                {
                    //Im gonna need this here : 
                    //stackoverflow.com/questions/8763761/threading-communication-between-two-threads-c-sharp

                    // This is also a nice thread about doing the whole rendering writeblebitmap on another thread: 
                    //https://stackoverflow.com/questions/9868929/how-to-edit-a-writablebitmap-backbuffer-in-non-ui-thread
                    if (!(Source.Width == 0) && !(Source.Height == 0))
                    {
                        current.AddFrame(Source);
                        current.LastDrawnFrame = frameNum;
                        //encoder.Frames.Add(BitmapFrame.Create(Source.CloneCurrentValue()));                       
                    }
                    else
                        throw new ArgumentException("Argument Frame, The bitmap size cannot be zero");
                }
                else
                    throw new ArgumentException("Argument Frame cannot be nothing");
            }
        }

        internal void End()
        {
            current.End();
        }
    }
}
