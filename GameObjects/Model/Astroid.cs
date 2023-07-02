﻿using Newtonsoft.Json;
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
        public override Vector Pos { get { return Body.Pos; } set { Body.Pos = value; } }
        public AstType Type { get; set; }
        public override Player Owner { get; set; }

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

        public override void Draw()
        {
            if (!HasHit)
            {
                Body.Draw(Color);
            }
        } 

        public override PolygonCollisionResult Collides(Jet j)
        {
            PolygonCollisionResult r = j.Hull.Collides(Body);
            if (r.WillIntersect)
            {
                return r;
            }
            else
            {
                return j.Cockpit.Collides(Body);
            }
        }



        public override void Move(GameState gameObjects)
        {
            //Asteroid is out of world bounds
            if (Pos.X + Size > gameObjects.World.size.Width || Pos.X < 0 || Pos.Y > gameObjects.World.size.Height || Pos.Y < 0)
            {
                HasHit = true;
                return;
            }
            Offset(Speed);
        }

        private void Offset(Vector by)
        {
            Body.Offset(by);
        }

        public override void HandleCollision(Jet j, PolygonCollisionResult r)
        {
            HasHit = true;
            if (Type == AstType.Ammo)
            {
                j.Owner.Recharge((int)Size);
            }
            else if (Type == AstType.Health)
            {
                j.Owner.Heal(1);
            }
            else
                j.Owner.Hit((int)Size);
        }
    }
}
