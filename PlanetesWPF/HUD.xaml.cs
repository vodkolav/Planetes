using GameObjects;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using GameObjects.Model;

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
            pbAmmo.Maximum = p.Jet.MaxAmmo;
            pbHealth.Maximum = p.Jet.MaxHealth;
            Visibility = Visibility.Visible;
            Color c = Color.FromArgb(p.Jet.Color.A, p.Jet.Color.R, p.Jet.Color.G, p.Jet.Color.B);
            Background = new SolidColorBrush(c);
        }
        internal void unbind()
        {
            lblName.Content = "";
            Visibility = Visibility.Hidden;
        }

        //TODO: remake this with data binding
        public void Draw()
        {
            try
            {
                Player playerstate = C.gameObjects.Players.SingleOrDefault(p => p.ID == playerID);
                if (playerstate != null)
                    lock (C.gameObjects)
                    {
                        lblSpeed.Content = "Speed: " + playerstate.Jet.Speed.ToString();

                        lblAcc.Content = "Acc:" + playerstate.Jet.Acceleration.X.ToString();

                        pbHealth.Value = playerstate.Jet.Health;
                        lblHealth.Content = "Health: " + playerstate.Jet.Health;
                        pbAmmo.Value = playerstate.Jet.Ammo;
                        lblAmmo.Content = "Ammo: " + playerstate.Jet.Ammo;
                    }
            }
            catch (Exception e)
            {
                Logger.Log(e, LogLevel.Debug);
            }
        }
    }
}
