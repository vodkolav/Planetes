using System;
using System.Windows.Media;
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
                return Center;
            }
        }

        public Vector LastOffset { get; set; }

        public Vector Acceleration { get; set; }
        
        public float Thrust { get; set; }

        public Vector Bearing { get; set; } = new Vector(1, 0);

        public Vector Aim { get; set; }

        public int Health { get; set; }
        public int MaxHealth { get; set; }
        public int Ammo { get; set; }
        public int MaxAmmo { get; set; }

        public Color Color
        {
            get => Owner.Color;
        }

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

        public float LastFired { get; set; }

        public bool KeyShoot { get; set; }

        public bool KeyBrake { get; set; }

        public Vector BounceNormal { get; set; }

        public float Cooldown { get; set; }

        public override Circle BoundingCirc { get { return new Circle(Pos, (Gun - Pos).Magnitude); } }

        [JsonIgnore]
        public override bool isAlive
        {
            get => Owner.isAlive;
        }

        public Jet()
        {
            
        }

        public Jet(Player owner, int health, int ammo)
        {
            double l = GameConfig.JetScale; 
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
            _hull.Offset(by);
            _cockpit.Offset(by);
        }

        public Vector Center
        {
            get { return (_cockpit.Center + _hull.Center) / 2; }
        }

        private void Rotate(Vector dir)
        {
            dir.Normalize();
            Bearing = dir;
            float angl = -Bearing.Angle(new Vector(1,0));

            _cockpit_cache = (Polygon)_cockpit.Clone();
            _cockpit_cache.RotateAt(angl, Center);

            _hull_cache = (Polygon)_hull.Clone();
            _hull_cache.RotateAt(angl, Center);
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

        public override void HandleCollision(Wall w, PolygonCollisionResult r)
        {
            Offset(r.MinimumTranslationVector);
            Bounce(r.translationAxis);
        }

        public override void HandleCollision(Map WorldEdge, PolygonCollisionResult r)
        {
            Offset(-Pos);
            Offset(GameConfig.TossPoint);
            //when I debug, the time continues, so when run continues, objects have 
            //flied far beyond world edge. return them to random place on map
        }

        public override void Move(GameState gO)
        {
            if (KeyBrake)
            {
                Acceleration = -Speed.GetNormalized() * 0.8;
            }

            if (Speed.X == Single.NaN)
            {
                Logger.Log("speed nan", LogLevel.Debug);
            }

            Vector deltaV = Acceleration * GameConfig.GameSpeed * Thrust * GameTime.DeltaTime; 
     
            Vector newSpeed = Speed + deltaV * 0.5;

            Vector offset = (Speed + deltaV * 0.5) * GameConfig.GameSpeed  * GameTime.DeltaTime ;
            
            //Physics Police            

            if (newSpeed.Magnitude <= GameConfig.Lightspeed)
            {
                Speed = newSpeed;
            }
            else
            {
                Speed = newSpeed.GetNormalized() * GameConfig.Lightspeed;
            }


            if (!(BounceNormal is null)) 
            {
                // TODO find out why it bounces strangely (too far)
                // Probably need to do "* GameConfig.GameSpeed * GameTime.DeltaTime"
                Vector oldspeed = Speed;

                Speed -= 2 * Speed.Dot(BounceNormal) * BounceNormal ;

                if (Speed.Magnitude != oldspeed.Magnitude)
                {
                    Logger.Log("hmmm", LogLevel.Info);
                }
                BounceNormal = null;
            }

            /*if (GameTime.DeltaTime == 0.000000f) // TODO: find WTF this happens
            {
                Logger.Log("DeltaTime zero", LogLevel.Debug);
            }*/

            Offset(offset);

            Rotate(Aim );///  GameTime.DeltaTime/ GameTime.DeltaTime 
            
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
            if (Health > hitter.Power)
                Health -= hitter.Power;
            else
                Die(hitter);
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
                    Bullet bullet = new Bullet(Owner, Gun, Bearing.GetNormalized(), GameConfig.bulletSpeed /*+ Speed*/, size: 3, color: Color);
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
