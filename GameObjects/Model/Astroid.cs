using Newtonsoft.Json;
using PolygonCollision;
using System;
using System.Drawing;

namespace GameObjects
{
    public enum AstType { Rubble, Ammo, Health };

    public class Astroid : ICollideable
    {
        [JsonIgnore]
        public float Size { get { return Body.R * 2; } }
        public Circle Body { get; set; }
        public Vector Speed { get; set; }
        [JsonIgnore]
        public Vector Pos { get { return Body.Pos; } set { Body.Pos = value; } }
        public AstType Type { get; set; }
        public bool HasHit { get; set; }

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
        public Color Color
        {
            get
            {
                switch (Type)
                {
                    case AstType.Ammo:
                        return Color.Yellow;
                    case AstType.Health:
                        return Color.Blue;
                    default:
                        return Color.SaddleBrown;
                }
            }
        }

        public Astroid(Size worldSize)
        {
            Random random = new Random();
            Body = new Circle(new Vector(random.Next(worldSize.Width), random.Next(worldSize.Height)), random.Next(20) + 5);
            int linearSpeed = random.Next(1, (int)GameConfig.Lightspeed);
            double Angle = Math.PI / 180 * random.Next(360);
            Vector mult = new Vector((float)Math.Cos(Angle), (float)Math.Sin(Angle));
            Speed = mult * linearSpeed;
            TossType(random);
            HasHit = false;
        }

        public void Draw()
        {
            if (!HasHit)
            {
                Body.Draw(Color);
            }
        } 

        public bool Collides(Astroid a)
        {
            return false;
        }

        public bool Collides(Jet j)
        {
            return j.Hull.Collides(Body).Intersect || j.Cockpit.Collides(Body).Intersect;
        }

        public PolygonCollisionResult Collides(Wall w)
        {
            return w.Body.Collides(Pos);
        }

        public void Move(GameState gameObjects)
        {
            //Asteroid is out of world bounds
            if (Pos.X + Size > gameObjects.World.size.Width || Pos.X < 0 || Pos.Y > gameObjects.World.size.Height || Pos.Y < 0)
            {
                HasHit = true;
                return;
            }

            foreach (Player p in gameObjects.Players)
            {
                if (Collides(p.Jet))
                {
                    HandleCollision(p);
                    return;
                }
            }

            foreach (Wall w in gameObjects.World.Walls)
            {
                if (Collides(w).Intersect)
                {
                    HasHit = true;
                    return;
                }
            }
            Offset(Speed);
        }

        private void Offset(Vector by)
        {
            Body.Offset(by);
        }

        public void HandleCollision(Player p)
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
}
