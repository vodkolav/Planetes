using Newtonsoft.Json;
using PolygonCollision;
using System.Drawing;

namespace GameObjects
{
    [JsonObject(IsReference = true)]
    public class Jet : ICollideable
    {
        public Vector Pos { get => Hull.Center; }

        public Vector Speed { get; set; }

        public Vector Acceleration { get; set; }

        public float Thrust { get; set; }

        public Vector Bearing { get; set; } = new Vector(1, 0);

        public Vector Aim { get; set; }

        public Color Color { get; set; } 

        public Polygon Hull { get; set; }

        public Polygon Cockpit { get; set; }

        public Player Owner { get; set; }

        public Vector Gun { get { return Cockpit.Vertices[1]; } }

        public int LastFired { get; set; }

        public int Cooldown { get; set; }

        public Jet(Player owner, Color color)
        {
            Hull = new Polygon();
            Hull.AddVertex(new Vector(50, 40));
            Hull.AddVertex(new Vector(100, 50));
            Hull.AddVertex(new Vector(100, 70));
            Hull.AddVertex(new Vector(50, 80));
            Hull.AddVertex(new Vector(50, 40));

            Cockpit = new Polygon();
            Cockpit.AddVertex(new Vector(100, 50));
            Cockpit.AddVertex(new Vector(130, 60));
            Cockpit.AddVertex(new Vector(100, 70));
            Cockpit.AddVertex(new Vector(100, 50));

            Owner = owner;
            Offset(new Vector(GameConfig.TossPoint));
            Speed = new Vector(0, 0);
            Acceleration = new Vector(0, 0);
            Aim = new Vector(1, 0);
            Color = color;
            Thrust = GameConfig.Thrust;
            Cooldown = 3;
        }

        public void Offset(Vector by)
        {
            Hull.Offset(by);
            Cockpit.Offset(by);
        }

        private void Rotate(Vector dir)
        {
            dir.Normalize();
            float diff = Bearing.Angle(dir);
            Bearing = dir;
            Hull.RotateAt(diff, Hull.Center);
            Cockpit.RotateAt(diff, Hull.Center);
        }

        public float Dist(Astroid a)
        {
            return Pos.Dist(a.Pos);
        }

        public float Dist(Bullet b)
        {
            return Pos.Dist(b.Pos);
        }

        public float Dist(Jet j)
        {
            return Pos.Dist(j.Pos);
        }

        public bool Collides(Astroid a)
        {
            return false;
        }

        public bool Collides(Jet j)
        {
            return false ;
        }

        public PolygonCollisionResult Collides(Wall w)
        {
            PolygonCollisionResult r = Hull.Collides(w.Body, Speed);
            if (r.WillIntersect)
            {
                return r;
            }
            else
            {
                return Cockpit.Collides(w.Body, Speed);
            }
        }

        public void Move(GameState gO)
        {
            Vector newSpeed = Speed + Acceleration * Thrust ;
            //Physics Police            

            if (newSpeed.Magnitude <= GameConfig.Lightspeed)
            {
                Speed = newSpeed;
            }
            else
            {
                Speed = newSpeed.GetNormalized() * GameConfig.Lightspeed;
            }

            PolygonCollisionResult r;
            foreach (Wall w in gO.World.Walls)
            {
                r = Collides(w);
                if (r.WillIntersect)
                {
                    Offset(r.MinimumTranslationVector);
                    Bounce(r.translationAxis);
                    break;
                }
            }
            Offset(Speed);

            //Rotate
           
            Rotate(Aim);

        }

        public void Bounce(Vector normal)
        {
            //TODO: rename speed to velocity
            Speed -= 2 * Speed.Dot(normal) * normal;
        }

        public void Shoot(int timeElapsed)
        {
            if (Owner.Ammo != 0 && timeElapsed > LastFired + Cooldown)
            {
                LastFired = timeElapsed;
                Bullet bullet = new Bullet(Owner, Owner.Jet.Gun, speed: Bearing.GetNormalized() * Bullet.linearSpeed /*+ Speed*/, size: 3, color: Color);
                lock (Owner.gameState)
                {
                    Owner.Bullets.Add(bullet);
                }
                Owner.Ammo--;
            }
        }

        public void Draw()
        {
            Hull.Draw(Color);
            Cockpit.Draw(Color.Gray);
        }

        public override string ToString()
        {
            return "Speed: " + Speed.ToString() + " |Acc:" + Acceleration.ToString();
        }
    }
}
