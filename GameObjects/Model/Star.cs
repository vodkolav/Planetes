﻿using PolygonCollision;
using System.Windows.Media;
using Newtonsoft.Json;

namespace GameObjects
{
    [JsonObject(IsReference = true)]
    public class Star 
    {
        public Circle Body { get; set; }    
        public Color Color { get; set; }

        [JsonIgnore]
        public Circle BoundingCirc { get { return Body; } }

        public Star() { }

        public Star(Vector pos, int size, Color col) 
        {
            Body = new Circle(pos,size);
            Color = col;
        }   

        public void Draw()
        {
            Body.Draw(Color);
        }
    }
}