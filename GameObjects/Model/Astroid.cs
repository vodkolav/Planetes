using System;
using System.Collections.Generic;
using System.Windows.Media;
using Newtonsoft.Json;
using PolygonCollision;

namespace GameObjects.Model
{
    public enum AstType { Rubble, Ammo, Health };

    public class Astroid : ICollideable
    {
        public Vector Size
        {
            get; set;
        }

        [JsonIgnore]
        public override Circle BoundingCirc
        {
            get { return (Circle)Body[0]; }
        }    

        public AstType Type { get; set; }

        public override Player Owner { get; set; }

        public override int Power {  get {return (int)Size.X; } }
        
        [JsonIgnore]
        public Color Color
        {
            get
            {
                switch (Type)
                {
                    case AstType.Ammo:
                        return Colors.Yellow;
                    case AstType.Health:
                        return Colors.Blue;
                    default:
                        return Colors.SaddleBrown;
                }
            }
        }
               
        public override float Rot => 0f;

        public Astroid(AstType type)
        {
            Pos = GameConfig.TossPoint;
            Size = new Size(GameConfig.TossInt(10, 30), 0);
            _body = new Corpus();
            _body.Add(new Circle(new Vector(0,0),Size.X));
            double linearSpeed = GameConfig.TossInt(1,(int)(GameConfig.Lightspeed * 0.5));
            double Angle = Math.PI / 180 * GameConfig.TossInt(360);
            Vector mult = new Vector((float)Math.Cos(Angle), (float)Math.Sin(Angle));
            Speed = mult * linearSpeed;
            Type = type;
            isAlive = true;
        }

        public Astroid()
        {
            
        }

        internal override bool OwnedBy(Player pl)
        {
            //Asteroids aren't owned by any player
            return false;
        }

        public override void Draw()
        {
            if (isAlive)
            {
                Body[0].Draw(Color);
            }
        }

        public override PolygonCollisionResult Collides(Wall w)
        {
            //can also be with foreach loop 
           return Body[0].Collides((Polygon)w.Body[0],Speed);
        }

        public override PolygonCollisionResult Collides(Jet j)
        {
            foreach (Polygon o in j.Body.Parts)
            {
                PolygonCollisionResult r = Body[0].Collides(o, Speed);
                if (r.Intersect)
                {
                    return r;
                }
            }            
            return PolygonCollisionResult.noCollision;
        }

        public override void HandleCollision(Jet j, PolygonCollisionResult r)
        {
            isAlive = false;
            if (Type == AstType.Ammo)
            {
                j.Recharge(Power);
            }
            else if (Type == AstType.Health)
            {
                j.Heal(Power);
            }
            else
                j.Hit(this);
        }

        public override void HandleCollision(Map WorldEdge, PolygonCollisionResult r)
        {
                isAlive = false;
        }

        public override void Move(float DeltaTime)
        {
            Offset(Speed * GameConfig.GameSpeed * DeltaTime);//*GameTime.DeltaTime);
        }

        private void Offset(Vector by)
        {
            Pos += by;
        }
    }
}
