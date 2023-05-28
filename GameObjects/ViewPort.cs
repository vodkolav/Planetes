using Newtonsoft.Json;
using PolygonCollision;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace GameObjects
{
    [JsonObject(IsReference = true)]
    public class ViewPort
    {
        // GameState gameState;
        public Polygon Body { get; set; }
        //Vector Position { get { return Body.Center; } }
        [JsonIgnore]
        public Vector Origin { get { return Body.Vertices.Min(); } }
        //Size size;
        public Vector velocity { get; set; }
        public Polygon Space { get; set; }
        public List<Wall> Walls { get; set; }
        public List<Player> Players { get; set; }
        public List<Astroid> Astroids { get; set; }
        [JsonIgnore]
        public Player P { get; set; }

        private Vector size { get; set; }
      
        public Vector Size
        {
            get { return size; }

            internal set
            {
                size = value;
                if (Body != null) {
                    Body  = new Polygon().FromRectangle(Body.Vertices[0].X, Body.Vertices[0].Y, value.X, value.Y);
                    Space = new Polygon().FromRectangle(Body.Vertices[0].X, Body.Vertices[0].Y, value.X, value.Y);
                }
                else
                {
                    Body = new Polygon().FromRectangle(0, 0, value.X, value.Y);
                    Space = new Polygon().FromRectangle(0, 0, value.X, value.Y);
                }
            }
        }

        public ViewPort() //, GameState gameState)
        {

        }

        public ViewPort(Player player) //, GameState gameState)
        {

            //this.gameState = gameState;
            velocity = new Vector(0,0);
            //this.size = size;
            Walls = new List<Wall>();
            Players = new List<Player>();
            Astroids = new List<Astroid>();

            P = player;
            Size = new Vector(800, 600);

        }


        internal void Update(GameState gameState)
        {


            //this.gameState = gameState;
            lock (gameState)
            {
                Vector target = P.Jet.Hull.Center;
                Vector source = Body.Center;
                Vector ofst = target - source;
                Body.Offset(ofst);

                Walls = gameState.World.Walls.Where(w => w.Body.Collides(Body, velocity).Intersect).ToList();
                Players = gameState.players.Where(p => p.Jet.Collides(Body)).ToList();
                // what's with all the bullets? apparently, when a jet is outside viewport, it's bullets wont be visible to enemies 
                Astroids = gameState.Astroids.Where(a => Body.Collides(a.Body)).ToList();
            }
        }


        public void Draw()
        {

            // WriteableBitmap.Crop() approach ? no 

            lock (this)
            {
                Space.Draw(Color.Black);
            }

            lock (this)
            {
                Walls.ForEach(w => w.Draw(-Origin)); 
            }

            lock (this)
            {
                Players.ForEach(p => p.Draw(-Origin));
            }

            lock (this)
            {
                Astroids.ForEach(a => a.Draw(-Origin));
            }
        }
    }
}
