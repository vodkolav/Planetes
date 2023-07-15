﻿using System.Windows.Media;
using Newtonsoft.Json;
using PolygonCollision;

namespace GameObjects.Model
{
    [JsonObject(IsReference = true)]
    public class Bullet : ICollideable
    {
        public override Vector Pos { get => Body.Pos; set => Body.Pos = value; }     
        
        public static int linearSpeed = GameConfig.bulletSpeed;

        public Vector Speed { get => Body.Tail; }
        
        public int Size { get => Body.Size; }       
        
        Ray Body { get; set; }

        public Color Color { get; set; }

        public override int Power { get; internal set; } = 1;

        public Bullet(Player owner, Vector pos, Vector speed, int size, Color color)
        {
            Owner = owner;            
            Body = new Ray(pos, speed*0.5, size);
            Color = color;
            isAlive = true;
        }   

        public override PolygonCollisionResult Collides(Astroid a)
        {
            return a.Body.Collides(Body);
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

        public override void HandleCollision(Jet j, PolygonCollisionResult r)
        {
            isAlive = false;
            j.Hit(this);
        }

        public override void Draw()
        {
            if (isAlive)
            {
                Body.Draw(Color);
            }
        }

        public override void Move(GameState gameObjects)
        { 
            Offset(Speed);
        }

        public void Offset(Vector by)
        {
           Body.Offset(by);
        }

        public override void HandleCollision(Map WorldEdge, PolygonCollisionResult r)
        {
            isAlive = false;
        }
    }
}
