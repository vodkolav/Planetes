using GameObjects;
using System;
using System.Linq;
using System.Windows.Forms;
using GameObjects.Model;

namespace Planetes
{
    public partial class HUD : UserControl
    {
        GameClient C;
        
        public HUD()
        {
            InitializeComponent();
        }
        public void bind(GameClient game, Player p)
        {
            C = game;
            lblName.Text = p.Name;
            pbAmmo.Maximum = p.Jet.MaxAmmo;
            pbHlth.Maximum = p.Jet.MaxHealth;
            Visible = true;
            BackColor = WFGraphicsContainer.ConvertColor(p.Jet.Color);
        }

        public void Draw()
        {
            try
            {
                Player playerstate = C.Me;
                if (playerstate != null)
                    lock (C.gameObjects)
                    {
                        lblSpeedX.Text = playerstate.Jet.Speed.X.ToString("N2");
                        lblSpeedY.Text = playerstate.Jet.Speed.Y.ToString("N2");

                        lblAccX.Text = playerstate.Jet.Acceleration.X.ToString("N2");
                        lblAccY.Text = playerstate.Jet.Acceleration.Y.ToString("N2");

                        pbHlth.Value = playerstate.Jet.Health;
                        lblHealth.Text = "Health: " + playerstate.Jet.Health;
                        pbAmmo.Value = playerstate.Jet.Ammo;
                        lblAmmo.Text = "Ammo: " + playerstate.Jet.Ammo;
                    }
            }
            catch (Exception e)
            {
                Logger.Log(e, LogLevel.Debug);
            }
        }
    }
}
