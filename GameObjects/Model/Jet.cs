using Newtonsoft.Json;
using PolygonCollision;
using System.Drawing;

namespace GameObjects
{
    [JsonObject(IsReference = true)]
    public class Jet : ICollideable
    {
        public override Vector Pos { get => Hull.Center; }

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

        public bool KeyBrake { get; set; }
        public int Cooldown { get; set; }

        public override Circle BoundingCirc { get { return new Circle(Pos, (Gun - Pos).Magnitude); } }

        [JsonIgnore]
        public override bool HasHit { get => false; set { } }

        public Jet(Player owner, Color color)
        {
            double l = 0.8; 
            Hull = new Polygon();
            Hull.AddVertex(new Vector(0, 0)*l);
            Hull.AddVertex(new Vector(50, 10) * l);//
            Hull.AddVertex(new Vector(50, 30) * l);//
            Hull.AddVertex(new Vector(0, 40) * l);
            Hull.AddVertex(new Vector(0, 0) * l);

            Cockpit = new Polygon();
            Cockpit.AddVertex(new Vector(50, 10) * l);//
            Cockpit.AddVertex(new Vector(80, 20) * l);
            Cockpit.AddVertex(new Vector(50, 30) * l);//
            Cockpit.AddVertex(new Vector(50, 10) * l);

            Owner = owner;
            Offset(new Vector(GameConfig.TossPoint));
            Speed = new Vector(0, 0);
            Acceleration = new Vector(0, 0);
            Aim = new Vector(1, 0);
            Color = color;
            Thrust = GameConfig.Thrust;
            Cooldown = 2;
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
                Acceleration = -Speed * 0.5;
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
            Cockpit.Draw(Color.Gray);
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
