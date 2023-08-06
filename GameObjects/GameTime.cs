namespace GameObjects
{
    public static class GameTime
    {
        private static float deltasum ;
        private static int frames ;

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

        public static float DeltaTimeAvg {
            get { return deltasum / frames; }
        } 
        public static float TotalElapsedSeconds { get; set; }
    }
}
