﻿using System.Windows.Media;
using Newtonsoft.Json;
using PolygonCollision;

namespace GameObjects.Model
{
    [JsonObject(IsReference = true)]
    public class Jet : ICollideable
    {
        [JsonIgnore]
        public override Vector Pos
        {
            get
            {
                return Hull.Center;
            }
        }

        public Vector Speed { get; set; }

        public Vector Acceleration { get; set; }

        public float Thrust { get; set; }

        public Vector Bearing { get; set; } = new Vector(1, 0);

        public Vector Aim { get; set; }

        public Color Color { get; set; }

        private Polygon _hull;
        private Polygon _hull_cache;


        public Polygon Hull 
        {
            get
            {
                return _hull_cache;
            }
            set { _hull = value;
                  _hull_cache = value;
            }
        }

        private Polygon _cockpit;

        private Polygon _cockpit_cache;

        public Polygon Cockpit
        {
            get
            {
                return _cockpit_cache;
            }
            set { 
                _cockpit = value;
                _cockpit_cache = value;
            }
        }

        public Vector Gun { get { return Cockpit.Vertices[1]; } }

        public int LastFired { get; set; }

        public bool KeyBrake { get; set; }
        public int Cooldown { get; set; }

        public override Circle BoundingCirc { get { return new Circle(Pos, (Gun - Pos).Magnitude); } }

        [JsonIgnore]
        public override bool HasHit { get => false; set { } }

        public Jet()
        {
            
        }

        public Jet(Player owner, Color color)
        {
            double l = 0.8; 
            Hull = new Polygon(5);
            Hull.AddVertex(new Vector(0, 0)*l);
            Hull.AddVertex(new Vector(50, 10) * l);//
            Hull.AddVertex(new Vector(50, 30) * l);//
            Hull.AddVertex(new Vector(0, 40) * l);
            Hull.AddVertex(new Vector(0, 0) * l);

            Cockpit = new Polygon(5);
            Cockpit.AddVertex(new Vector(50, 10) * l);//
            Cockpit.AddVertex(new Vector(80, 20) * l);
            Cockpit.AddVertex(new Vector(50, 30) * l);//
            Cockpit.AddVertex(new Vector(50, 10) * l);

            Owner = owner;
            Offset(GameConfig.TossPoint);
            Speed = new Vector(0, 0);
            Acceleration = new Vector(0, 0);
            Aim = new Vector(1, 0);
            Color = color;
            Thrust = GameConfig.Thrust;
            Cooldown = 2;
        }

        public void Offset(Vector by)
        {
            _hull.Offset(by);
            _cockpit.Offset(by);
        }

        private void Rotate(Vector dir)
        {
            dir.Normalize();
            Bearing = dir;
            float angl = -Bearing.Angle(new Vector(1,0));

            _cockpit_cache = (Polygon)_cockpit.Clone();
            _cockpit_cache.RotateAt(angl, _hull.Center);

            _hull_cache = (Polygon)_hull.Clone();
            _hull_cache.RotateAt(angl, _hull.Center);          

        }

        public override PolygonCollisionResult Collides(Jet j)
        {
            // Jets don't collide with other Jets. they might later.
            return PolygonCollisionResult.noCollision;
        }

        public override PolygonCollisionResult Collides(Wall w)
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

        public override  void Move(GameState gO)
        {
            if (KeyBrake)
            {
                Acceleration = -Speed * 0.8;
            }

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

            Offset(Speed);

            Rotate(Aim);

            Owner.viewPort.Update();
        }

        public void Bounce(Vector normal)
        {
            //TODO: rename speed to velocity
            Speed -= 2 * Speed.Dot(normal) * normal;
        }

        public override void Draw()
        {
            Hull.Draw(Color);
            Cockpit.Draw(Colors.Gray);
        }

        public override string ToString()
        {
            return "Speed: " + Speed.ToString() + " |Acc:" + Acceleration.ToString();
        }

        public override void HandleCollision(Wall w , PolygonCollisionResult r)
        {
            Offset(r.MinimumTranslationVector);
            Bounce(r.translationAxis);
        }

        public override void HandleCollision(Map WorldEdge, PolygonCollisionResult r)
        {
            Bounce(-Speed);
        }
    }
}
