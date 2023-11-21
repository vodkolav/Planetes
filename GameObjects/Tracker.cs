using GameObjects.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameObjects
{

    public class IntVal : ITrackable
    {
        public int Value { get; set; }
    }

    public class AtomicVal : ITrackable
    {
        public float Value { get; set; }
    }
    public class StrVal : ITrackable
    {
        public string Value { get; set; }
    }

    public class ColSpec //where T : ITrackable
    {
        public virtual string header { get; set; }
        public  Func<object,string> value { get; set; }
    }

    public class ColSpec<T> :ColSpec where T : ITrackable
    {
        public ColSpec(string header, Func<T, string> val ) 
        {
            this.header = header;
            value = val;
        }
        public override string header { get; set; }
        public new Func<T, string> value { get; set; }
    }

    internal class Tracker
    {
        List<ColSpec> colSpecs = new List<ColSpec>();

        internal void Configure()
        {

/*            string CSVheader = "frame, UtcNow, DeltaTime, Source, JetSpeedMag, JetSpeedX, JetSpeedY, JetPosMag, JetPosX, JetPosY";

            string CSVline = $"{frameNum}, {tes:F4}, {dt:F4}, {source}, " +
            $"{debugged.Speed.Magnitude:F4}, {debugged.Speed.X:F4},{debugged.Speed.Y:F4}, " +
            $"{debugged.Pos.Magnitude}, {debugged.Pos.X}, {debugged.Pos.Y}";
*/
            

            colSpecs.Add(new ColSpec<GameState>("frameNum", (GameState gs) => $"{gs.frameNum}:F4"));

            colSpecs.Add(new ColSpec<AtomicVal>("UtcNow", (AtomicVal fv) => $"{GameTime.TotalElapsedSeconds}:F4"));

            colSpecs.Add(new ColSpec<AtomicVal>("DeltaTime", (AtomicVal gt) => $"{GameTime.DeltaTime}:F4"));
            
            colSpecs.Add(new ColSpec<AtomicVal>("source", (AtomicVal fv) => $"{fv}:F4"));

            
            colSpecs.Add(new ColSpec<ICollideable>("JetSpeedMag", (ICollideable j) => $"{j.Speed.Magnitude}:F4"));

            colSpecs.Add(new ColSpec<ICollideable>("JetSpeedX", (ICollideable j) => $"{j.Speed.X}:F4"));

            colSpecs.Add(new ColSpec<ICollideable>("JetSpeedY", (ICollideable j) => $"{j.Speed.Y}:F4"));

            colSpecs.Add(new ColSpec<ICollideable>("JetPosMag", (ICollideable j) => $"{j.Pos.Magnitude}:F4"));

            colSpecs.Add(new ColSpec<ICollideable>("JetPosX", (ICollideable j) => $"{j.Pos.X}:F4"));

            colSpecs.Add(new ColSpec<ICollideable>("JetPosY", (ICollideable j) => $"{j.Pos.Y}:F4"));
        }

        public string GetLine(ITrackable obj)
        {
            var x =  colSpecs.Select(cs => cs.value(obj));

            return string.Join(",", x );
        }

        public string GetHeader()
        {
            var x = colSpecs.Select(cs => cs.header);
            return string.Join(",", x);
        }

        internal void Track(GameState gs, string playerName, string source)
        {
#if DEBUG
            // This function may be useful to track a specific game object (usually Jet)
            // over time and then plot the data. useful when diagnosing fps issues
            try
            {
                if (GameConfig.loglevels.Contains(LogLevel.CSV))
                {
                    if (source == "header")
                    {
                        string CSVheader = "frame, UtcNow, DeltaTime, Source, JetSpeedMag, JetSpeedX, JetSpeedY, JetPosMag, JetPosX, JetPosY";
                        //string CSVheader = "frame, UtcNow, DeltaTime, Source, JetSpeed, JetPosMag, JetBearingX, JetBearingY";
                        Logger.Log(CSVheader, LogLevel.CSV);
                    }

                    Jet debugged = gs.Players.Single(p => p.Name.ToLower().Contains(playerName)).Jet; // WPFplayer

                    float tes;
                    float dt;
                    if (source == "Draw")
                    {
                        tes = ClientTime.TotalElapsedSeconds;
                        dt = ClientTime.DeltaTime;
                    }
                    else
                    {
                        tes = GameTime.TotalElapsedSeconds;
                        dt = GameTime.DeltaTime;
                    }


                    string CSVline = $"{frameNum}, {tes:F4}, {dt:F4}, {source}, " +
                                     $"{debugged.Speed.Magnitude:F4}, {debugged.Speed.X:F4},{debugged.Speed.Y:F4}, " +
                                     $"{debugged.Pos.Magnitude}, {debugged.Pos.X}, {debugged.Pos.Y}";
                    // $"{debugged.Bearing.X}, {debugged.Bearing.Y}";

                    Logger.Log(CSVline, LogLevel.CSV);
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
