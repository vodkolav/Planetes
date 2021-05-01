using PolygonCollision;
using System.Drawing;

namespace GameObjects
{
    public class Jet
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

        public Vector Gun { get { return Cockpit.Vertices[1]; } }

        public int LastFired { get; set; }

        public int Cooldown { get; set; }

        public Jet(Point start, Color color)
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

            Offset(new Vector(start));
            Speed = new Vector(0, 0);
            Acceleration = new Vector(0, 0);
            Aim = new Vector(0, 0);
            //Pos_x = start.X;
            //Pos_y = start.Y;
            Color = color;
            Thrust = GameConfig.Thrust;
            Cooldown = 3;
        }

        private void Offset(Vector by)
        {
            Hull.Offset(by);
            Cockpit.Offset(by);
        }

        private void Rotate(Vector dir)
        {
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
            return Hull.Collides(a.Body) || Cockpit.Collides(a.Body);
        }

        public bool Collides(Bullet b)
        {
            return Hull.Collides(b.Pos) || Cockpit.Collides(b.Pos);
        }

        public void Move(GameState gO)
        {
            //Physics Police
            Vector newSpeed = Speed + Acceleration * Thrust ;
            if (newSpeed.Magnitude_X <= GameConfig.Lightspeed && newSpeed.Magnitude_Y <= GameConfig.Lightspeed)
            {
                Speed = newSpeed;
            }

            PolygonCollisionResult r;
            foreach (Wall w in gO.World.Walls)
            {
                r = Hull.Collides(w.Body, Speed);
                if (r.WillIntersect)
                {
                    Offset(Speed + r.MinimumTranslationVector);
                    Bounce(r.translationAxis);
                    break;
                }
            }
            Offset(Speed);

            //Rotate
            Vector dir = Aim - Hull.Center;

            Rotate(dir);

        }

        public void Bounce(Vector normal)
        {
            Speed -= 2 * Speed.Dot(normal) * normal;
        }

        public void Shoot(Player player, int timeElapsed)
        {
            if (player.Ammo != 0 && timeElapsed > LastFired + Cooldown)
            {
                LastFired = timeElapsed;
                Bullet bullet = new Bullet(pos: player.Jet.Gun, speed: Bearing * (Bullet.linearSpeed / Bearing.Magnitude), size: 5, color: Color);
                lock (player.gameState)
                {
                    player.Bullets.Add(bullet);
                }
                player.Ammo--;
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
