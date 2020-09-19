using Newtonsoft.Json;
using PolygonCollision;
using System;
using System.Drawing;

namespace GameObjects
{
    public enum AstType { Rubble, Ammo, Health };

    public class Astroid
    {
        [JsonIgnore]
        public float Size { get { return Body.R * 2; } }
        public Circle Body { get; set; }
        public Vector Speed { get; set; }
        [JsonIgnore]
        public Vector Pos { get { return Body.Pos; } set { Body.Pos = value; } }
        public AstType Type { get; set; }
        public bool HasHit { get; set; }

        static public int Timeout { get { return 10; } }

        public void TossType(Random random)
        {
            switch (random.Next(10))
            {
                case (1):
                    Type = AstType.Ammo;
                    break;
                case 2:
                    Type = AstType.Health;
                    break;
                default:
                    Type = AstType.Rubble;
                    break;
            }
        }
        [JsonIgnore]
        public Brush Color
        {
            get
            {
                switch (Type)
                {
                    case AstType.Ammo:
                        return Brushes.Yellow;
                    case AstType.Health:
                        return Brushes.Blue;
                    default:
                        return Brushes.Brown;
                }
            }
        }

        public Astroid(Size winSize)
        {
            Random random = new Random();
            Body = new Circle(new Vector(random.Next(winSize.Width), random.Next(winSize.Height)), random.Next(10) + 5);
            int linearSpeed = random.Next(1, 4) + 1;
            double Angle = Math.PI / 180 * random.Next(360);
            Vector mult = new Vector((float)Math.Cos(Angle), (float)Math.Sin(Angle));
            Speed = mult * linearSpeed;
            TossType(random);
            HasHit = false;
        }

        public void Draw(Graphics g)
        {
            if (!HasHit)
            {
                Body.Draw(g, Color);
            }
        }

        public void Collides(Player p)
        {
            if (p.Jet.Collides(this))
            //Pos_x + Size > gameObjects.player1.Jet.Pos_x
            //&& Pos_x - Size < gameObjects.player1.Jet.Pos_x + gameObjects.player1.Jet.Width
            //&& Pos_y - Size < gameObjects.player1.Jet.Pos_y + gameObjects.player1.Jet.Height
            //&& Pos_y + Size > gameObjects.player1.Jet.Pos_y)
            {
                HasHit = true;
                if (Type == AstType.Ammo)
                {
                    p.Recharge((int)Size);
                }
                else if (Type == AstType.Health)
                {
                    p.Heal(1);
                }
                else
                    p.Hit((int)Size);
            }
        }

        public void Move(GameState gameObjects)
        {
            gameObjects.players.ForEach(Collides);

            //Asteroid is out of screen
            if (Pos.X + Size > gameObjects.WinSize.Width || Pos.X < 0 || Pos.Y > gameObjects.WinSize.Height || Pos.Y < 0)
            {
                HasHit = true;
            }



            foreach (Wall w in gameObjects.Walls)
            {
                if (w.region.Collides(Pos))
                {
                    HasHit = true;
                }
            }
            Offset(Speed);
            //Pos_x += (int)(Math.Cos(2 * Math.PI / 360 * Angle) * Speed);
            //Pos_y += (int)(Math.Sin(2 * Math.PI / 360 * Angle) * Speed);
        }

        private void Offset(Vector by)
        {
            Body.Offset(by);
        }
    }
}
