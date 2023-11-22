using GameObjects.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GameObjects
{

    public class ColSpec 
    {
        public ColSpec(string header, Func<string> val ) 
        {
            Header = header;
            Value = val;
        }
        public  string Header { get; set; }
        public Func<string> Value { get; set; }
    }

    public class Tracker
    {

        public Tracker() { }

        List<ColSpec> colSpecs ;

        public GameState GS { get; set; }

        public string PlayerName { get; set; }

        public string Tag { get; set; }

        public float TotalElapsedSeconds
        {
            get
            {
                if (Tag == "Draw")
                {
                    return ClientTime.TotalElapsedSeconds;                  
                }
                else
                {
                    return GameTime.TotalElapsedSeconds;                    
                }
            }
        }
        
        public float DeltaTime
        {
            get
            {
                if (Tag == "Draw")
                {
                    return ClientTime.DeltaTime;
                }
                else
                {
                    return GameTime.DeltaTime;
                }
            }
        }

        public ICollideable J
        {
            get { return P.Jet; }
        }

        public Player P
        {
            get { return GS.Players.Single(p => p.Name.ToLower().Contains(PlayerName.ToLower())); }
        }

        internal void Configure()
        {
            /*
            string CSVheader = "frame, UtcNow, DeltaTime, Source, JetSpeedMag, JetSpeedX, JetSpeedY, JetPosMag, JetPosX, JetPosY";
            JetBearingX, JetBearingY
            string CSVline = $"{frameNum}, {tes:F4}, {dt:F4}, {source}, " +
            $"{debugged.Speed.Magnitude:F4}, {debugged.Speed.X:F4},{debugged.Speed.Y:F4}, " +
            $"{debugged.Pos.Magnitude}, {debugged.Pos.X}, {debugged.Pos.Y}";
            */
            colSpecs = new List<ColSpec>
            {
                new ColSpec("frameNum", () => $"{GS.frameNum}"),

                new ColSpec("GameSec", () => $"{TotalElapsedSeconds:F4}"),

                new ColSpec("DeltaTime", () => $"{DeltaTime:F4}"),

                new ColSpec("source", () => $"{Tag}"),

                new ColSpec("player", () => $"{P.Name}"),


                new ColSpec("JetSpeedMag", () => $"{J.Speed.Magnitude:F4}"),

                new ColSpec("JetSpeedX", () => $"{J.Speed.X:F4}"),

                new ColSpec("JetSpeedY", () => $"{J.Speed.Y:F4}"),

                new ColSpec("JetPosMag", () => $"{J.Pos.Magnitude}"),

                new ColSpec("JetPosX", () => $"{J.Pos.X}"),

                new ColSpec("JetPosY", () => $"{J.Pos.Y}")
            };
        }

        public string GetLine()
        {
            var x = colSpecs.Select(cs => cs.Value());
            return string.Join(",", x);
        }

        public string GetHeader()
        {
            var x = colSpecs.Select(cs => cs.Header);
            return string.Join(",", x);
        }

        internal void Track(GameState gs, string playerName, string tag)
        {
#if DEBUG

            GS = gs;
            PlayerName = playerName;
            Tag = tag;

            // This function may be useful to track a specific game object (usually Jet)
            // over time and then plot the data. useful when diagnosing fps issues
            try
            {
                if (GameConfig.loglevels.Contains(LogLevel.CSV))
                {
                    if (tag == "header")
                    {
                        string CSVheader = GetHeader(); 
                            //"frame, UtcNow, DeltaTime, Source, JetSpeedMag, JetSpeedX, JetSpeedY, JetPosMag, JetPosX, JetPosY";
                        //string CSVheader = "frame, UtcNow, DeltaTime, Source, JetSpeed, JetPosMag, JetBearingX, JetBearingY";
                        Logger.Log(CSVheader, LogLevel.CSV);
                    }

                    Logger.Log(GetLine(), LogLevel.CSV);
                }
            }
            catch (Exception e)
            {
                Logger.Log(e, LogLevel.Warning);
            }
#endif
        }
    }
}
