using System;
using System.Collections.Generic;
using System.Windows.Media;
using Newtonsoft.Json;
using PolygonCollision;

namespace GameObjects.Model
{
    [JsonObject(IsReference = true)]
    public class Jet : ICollideable
    {         

        public Vector LastOffset { get; set; }

        public Vector Acceleration { get; set; }
        
        public float Thrust { get; set; }

        public Vector Bearing { get; set; } = new Vector(1, 0);

        public Vector Orientation { get; set; }

        public override float Rot { get => -Bearing.Angle(Orientation); }

        public Vector Aim { get; set; }

        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public int Ammo { get; set; }
        public int MaxAmmo { get; set; }

        public Color Color
        {
            get => Owner.Color;
        }

        [JsonIgnore]
        public Polygon Hull
        {
            get
            {
                return (Polygon)Body[0];
            }
        }

        [JsonIgnore]
        public Polygon Cockpit 
        { 
            get 
            { 
                return (Polygon)Body[1]; 
            }  
        }

        [JsonIgnore]
        public Vector Gun { get { return Cockpit.Vertices[2]; } }

        public float LastFired { get; set; }

        public bool KeyShoot { get; set; }

        public bool KeyBrake { get; set; }

        public Vector BounceNormal { get; set; }

        public float Cooldown { get; set; }

        [JsonIgnore]
        public override Circle BoundingCirc { get { return new Circle(Pos, (Gun - Pos).Magnitude); } }

        [JsonIgnore]
        public override bool isAlive
        {
            get => Owner.isAlive;
        }

        public Jet()
        {
            
        }

        public void Construct()
        {
            double l = GameConfig.JetScale;

            Polygon hull = new Polygon(7);
            hull.AddVertex(new Vector(-10,  10) * l);
            hull.AddVertex(new Vector(-20, -30) * l);
            hull.AddVertex(new Vector(-10, -40) * l);
            hull.AddVertex(new Vector( 10, -40) * l);
            hull.AddVertex(new Vector( 20, -30) * l);
            hull.AddVertex(new Vector( 10,  10) * l);
            hull.AddVertex(new Vector(-10,  10) * l);
            _body.Add(hull);

            Polygon cockpit = new Polygon(5);
            cockpit.AddVertex(new Vector(-10, 10) * l);
            cockpit.AddVertex(new Vector( 10, 10) * l);
            cockpit.AddVertex(new Vector( 05, 25) * l);
            cockpit.AddVertex(new Vector(-05, 25) * l);
            cockpit.AddVertex(new Vector(-10, 10) * l);
            _body.Add(cockpit);

            Orientation = new Vector(0, 1);
        }

        public void ConstructOld()
        {
            double l = GameConfig.JetScale;
            Polygon hull = new Polygon(5);
            hull.AddVertex(new Vector(0, 0) * l);
            hull.AddVertex(new Vector(50, 10) * l);//
            hull.AddVertex(new Vector(50, 30) * l);//
            hull.AddVertex(new Vector(0, 40) * l);
            hull.AddVertex(new Vector(0, 0) * l);

            Polygon cockpit = new Polygon(5);
            cockpit.AddVertex(new Vector(50, 10) * l);//
            cockpit.AddVertex(new Vector(80, 20) * l);
            cockpit.AddVertex(new Vector(50, 30) * l);//
            cockpit.AddVertex(new Vector(50, 10) * l);
            Body.Add(cockpit);

            Orientation = new Vector(1,0);
            Vector centering = new Vector(-40, -20);
            hull.Offset(centering);
            cockpit.Offset(centering);
        }

        public Jet(Player owner, int health, int ammo)
        {
            _body = new List<Figure>();
            Construct();
            Pos = GameConfig.TossPoint;
            Owner = owner;
            
            Speed = new Vector(0, 0);
            Acceleration = new Vector(0, 0);
            Aim = new Vector(1, 0);
            Thrust = GameConfig.Thrust;
            Cooldown = GameConfig.Cooldown;

            Health = health;
            MaxHealth = health;
            Ammo = ammo;
            MaxAmmo = ammo;
        }

        public void Offset(Vector by)
        {
            LastOffset = by;
            Pos += by;
        }

        public Vector Center { get; } = new Vector(0, 0);
        
        private void Rotate(Vector dir)
        {
            Bearing = dir.GetNormalized();
        }
        
        public override PolygonCollisionResult Collides(Jet j)
        {
            // Jets don't collide with other Jets. they might later.
            return PolygonCollisionResult.noCollision;
        }

        public override PolygonCollisionResult Collides(Wall w)
        {
            foreach (Figure f in Body)
            {
                PolygonCollisionResult r = ((Polygon)f).Collides((Polygon)w.Body[0], Speed);
                if (r.Intersect)
                {
                    return r;
                }
            }        
            return PolygonCollisionResult.noCollision;
        }       

        public override void HandleCollision(Wall w, PolygonCollisionResult r)
        {
            Offset(r.MinimumTranslationVector);
            Bounce(r.translationAxis);
        }

        public override void HandleCollision(Map WorldEdge, PolygonCollisionResult r)
        {
            Teleport();
            //when I debug, the time continues, so when run continues, objects have 
            //flied far beyond world edge. return them to center of the world 
        }

        public void Teleport()
        {
            Pos = GameConfig.WorldSize / 2; 
            Speed.X = 0; 
            Speed.Y = 0;
        }

        public override void Move()
        {
            if (KeyBrake)
            {
                Acceleration = -Speed.GetNormalized() * 0.8;
            }

            if (Speed.X == float.NaN)
            {
                Logger.Log("speed nan", LogLevel.Debug);
            }

            Vector deltaV = Acceleration * GameConfig.GameSpeed * Thrust * GameTime.DeltaTime; 
     
            Speed += deltaV * 0.5;

            if (!(BounceNormal is null))
            {
                // TODO find out why it bounces strangely (too far)
                // Probably need to do "* GameConfig.GameSpeed * GameTime.DeltaTime"
                // Logger.Log(BounceNormal.ToString(), LogLevel.Debug);

                Vector oldspeed = (Vector)Speed.Clone();

                Speed -= 2 * Speed.Dot(BounceNormal) * BounceNormal;

                if (Speed.Magnitude != oldspeed.Magnitude && Owner.Name.ToLower().Contains("player"))
                {
                    Logger.Log($"Speed.Magnitude: {Speed.Magnitude}| oldspeed.Magnitude: {oldspeed.Magnitude}| GameConfig.Lightspeed: {GameConfig.Lightspeed}", LogLevel.Debug);
                }
                BounceNormal = null;
            }

            //Physics Police            

            if (Speed.Magnitude >= GameConfig.Lightspeed)
            {            
                Speed = Speed.GetNormalized() * GameConfig.Lightspeed;
            }

            Vector offset = Speed * GameConfig.GameSpeed * GameTime.DeltaTime;

            /*if (GameTime.DeltaTime == 0.000000f) // TODO: find WTF this happens
            {
                Logger.Log("DeltaTime zero", LogLevel.Debug);
            }*/

            Offset(offset);

            Rotate(Aim);///  GameTime.DeltaTime/ GameTime.DeltaTime 

            Owner.viewPort.Update();            
        }

        public void Bounce(Vector normal)
        {
            BounceNormal = normal;
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

        public void Recharge(int amount)
        {
            Ammo = Math.Min(Ammo + (int)amount, MaxAmmo);
        }

        public void Heal(int amount)
        {
            Health = Math.Min(Health + amount, MaxHealth);
        }

        internal void Hit(ICollideable hitter)
        {
            if (Health <= hitter.Power)
                Die(hitter);                
            Health -= hitter.Power;
        }

        private void Die(ICollideable hitter)
        {
            Owner.MatchEvent(hitter);
        }

        public virtual void Shoot(GameState gameObjects)
        {
            if (KeyShoot)
            {
                if (Ammo != 0 && GameTime.TotalElapsedSeconds > LastFired + Cooldown)
                {
                    LastFired = GameTime.TotalElapsedSeconds;
                    Bullet bullet = new Bullet(Owner, Gun, Bearing, GameConfig.bulletSpeed /*+ Speed*/, width: 3, color: Color);
                    gameObjects.Entities.Add(bullet);
                    Ammo--;
                }
            }
        }

        public virtual void Press(HOTAS argument)
        {
            switch (argument)
            {
                case (HOTAS.Up):
                {
                    Acceleration.Y = -1;
                    break;
                }
                case (HOTAS.Down):
                {
                    Acceleration.Y = 1;
                    break;
                }
                case (HOTAS.Left):
                {
                    Acceleration.X = -1;
                    break;
                }
                case (HOTAS.Right):
                {
                    Acceleration.X = 1;
                    break;
                }
                case (HOTAS.Shoot):
                {
                    KeyShoot = true;
                    break;
                }
                case HOTAS.Brake:
                {
                    KeyBrake = true;
                    break;
                }
                case HOTAS.Scuttle:
                    {
                        Bullet TheLastAmmunition = new Bullet();
                        Die(TheLastAmmunition);
                        break;
                    }
            }
        }

        public virtual void Release(HOTAS argument)
        {
            switch (argument)
            {
                case (HOTAS.Up):
                {
                    Acceleration.Y = 0;
                    break;
                }
                case (HOTAS.Down):
                {
                    Acceleration.Y = 0;
                    break;
                }
                case (HOTAS.Left):
                {
                    Acceleration.X = 0;
                    break;
                }
                case (HOTAS.Right):
                {
                    Acceleration.X = 0;
                    break;
                }
                case (HOTAS.Shoot):
                {
                    KeyShoot = false;
                    break;
                }
                case HOTAS.Brake:
                {
                    KeyBrake = false;
                    Acceleration.X = 0;
                    Acceleration.Y = 0;
                    break;
                }
            }
        }
    }
}
