using System;

namespace GameObjects
{
    public static class GameTime
    {
        private static float deltasum;
        private static int frames;
        public static DateTime StartTime { get; set; }
        private static float _deltatime;

        public static float DeltaTime
        {
            get { return _deltatime; }
            set
            {
                _deltatime = value;
                if (value < 2.0f)
                {
                    deltasum += value;
                    frames++;
                }
            }
        }

        public static void Tick()
        {
            float dt = (float)(DateTime.UtcNow - StartTime).TotalSeconds;
            DeltaTime = dt - TotalElapsedSeconds;
            TotalElapsedSeconds = dt;
        }

        public static float DeltaTimeAvg
        {
            get { return deltasum / frames; }
        }
        public static float TotalElapsedSeconds { get; set; }
    }

    public static class ClientTime
    {
        private static float deltasum;
        private static int frames;
        public static DateTime StartTime { get; set; }
        private static float _deltatime;

        public static float DeltaTime
        {
            get { return _deltatime; }
            set
            {
                _deltatime = value;
                if (value < 2.0f)
                {
                    deltasum += value;
                    frames++;
                }
            }
        }

        public static void Tick()
        {
            float dt = (float)(DateTime.UtcNow - StartTime).TotalSeconds;
            DeltaTime = dt - TotalElapsedSeconds;
            TotalElapsedSeconds = dt;
        }

        public static float DeltaTimeAvg
        {
            get { return deltasum / frames; }
        }

        public static float TotalElapsedSeconds { get; set; }
    }
}
