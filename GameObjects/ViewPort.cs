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
        public Vector Origin { get { return Body.Vertices[0]; } }
        //Size size;
        public Vector velocity { get; set; }
        public Map World { get; set; }
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
                    Body  = new Polygon().FromRectangle(Body.Vertices.Min(v => v.X), Body.Vertices.Min(v => v.Y), value.X, value.Y);
                }
                else
                {
                    Body = new Polygon().FromRectangle(0, 0, value.X, value.Y);
                }
            }
        }

        public ViewPort() 
        {

        }

        public ViewPort(Player player) 
        {
            velocity = new Vector(0,0);
            World = new Map(new Size(500,500));
            Walls = new List<Wall>();
            Players = new List<Player>();
            Astroids = new List<Astroid>();
            P = player;
            Size = new Vector(800, 600);
        }


        internal void Update(GameState gameState)
        {
            lock (gameState)
            {
                Vector target = P.Jet.Pos;
                Vector source = Body.Center;
                Vector ofst = target - source;
                Body.Offset(ofst);

                World = gameState.World;
                Walls = gameState.World.Walls.Where(w => w.Body.Collides(Body, velocity).Intersect).ToList();
                Players = gameState.players.Where(p => p.Jet.Collides(Body) || p.Bullets.Any(b => Body.Collides(b.Pos))).ToList();
                Astroids = gameState.Astroids.Where(a => !Body.Collides(a.Body)).ToList(); // TODO: understand why Collides here is supposed to be negated?
            }
        }


        public void Draw()
        {
            DrawingContext.GraphicsContainer.ViewPortOffset = -Origin;

            lock (this)// TODO: do these checks in map class 
            {
                World.Space.Draw(Color.Black);
            }

            lock (this) // TODO: do these checks in map class 
            {
                Walls.ForEach(w => w.Draw()); 
            }

            lock (this)
            {
                Players.ForEach(p => p.Draw());
            }

            lock (this)
            {
                Astroids.ForEach(a => a.Draw());
            }
        }
    }
}
