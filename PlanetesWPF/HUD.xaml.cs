using GameObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
                Player playerstate = C.gameObjects.players.SingleOrDefault(p => p.ID == playerID);
                if (playerstate != null)
                    lock (C.gameObjects)
                    {
                        //lblSpeedX.Text = playerstate.Jet.Speed.X.ToString("N2");
                        //lblSpeedY.Text = playerstate.Jet.Speed.Y.ToString("N2");

                        //lblAccX.Text = playerstate.Jet.Acceleration.X.ToString("N2");
                        //lblAccY.Text = playerstate.Jet.Acceleration.Y.ToString("N2");

                        pbHealth.Value = playerstate.Health;
                        lblHealth.Content = "Health: " + playerstate.Health;
                        pbAmmo.Value = playerstate.Ammo;
                        lblAmmo.Content = "Ammo: " + playerstate.Ammo;
                    }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
