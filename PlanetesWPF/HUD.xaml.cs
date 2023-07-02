using GameObjects;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PlanetesWPF
{
    /// <summary>
    /// Interaction logic for HUD.xaml
    /// </summary>
    public partial class HUD : UserControl
    {      
        GameClient C;
        int playerID;

        public HUD()
        {
            InitializeComponent();
        }
        public void bind(GameClient game, Player p)
        {
            C = game;
            playerID = p.ID;
            lblName.Content = p.Name;
            pbAmmo.Maximum = p.MaxAmmo;
            pbHealth.Maximum = p.MaxHealth;
            Visibility = Visibility.Visible;
            Color c = Color.FromArgb(p.Jet.Color.A, p.Jet.Color.R, p.Jet.Color.G, p.Jet.Color.B);
            Background = new SolidColorBrush(c);
        }

        public void Draw()
        {
            try
            {
                Player playerstate = C.gameObjects.Players.SingleOrDefault(p => p.ID == playerID);
                if (playerstate != null)
                    lock (C.gameObjects)
                    {
                        lblSpeed.Content = playerstate.Jet.Speed.ToString();

                        lblAcc.Content = playerstate.Jet.Acceleration.X.ToString();

                        pbHealth.Value = playerstate.Health;
                        lblHealth.Content = "Health: " + playerstate.Health;
                        pbAmmo.Value = playerstate.Ammo;
                        lblAmmo.Content = "Ammo: " + playerstate.Ammo;
                    }
            }
            catch (Exception e)
            {
                Logger.Log(e, LogLevel.Debug);
            }
        }
    }
}
