using GameObjects;
using PolygonCollision;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Planetes
{
	public partial class HUD : UserControl
	{
		Game GAME;
		int playerID;

		public HUD()
		{
			InitializeComponent();
		}
		public void bind(Game game, Player p)
		{
			GAME = game;
			playerID = p.ID;
			lblName.Text = p.Name;
			pbAmmo.Maximum = p.MaxAmmo;
			pbHlth.Maximum = p.MaxHealth;
			Visible = true;
			BackColor = p.Jet.Color;
		}

		public void Draw()
		{
			try
			{
				Player playerstate = GAME.gameObjects.players.SingleOrDefault(p => p.ID == playerID);
				if (playerstate != null)
					lock (GAME.gameObjects)
					{
						lblSpeedX.Text = playerstate.Jet.Speed.X.ToString("N2");
						lblSpeedY.Text = playerstate.Jet.Speed.Y.ToString("N2");

						lblAccX.Text = playerstate.Jet.Acceleration.X.ToString("N2");
						lblAccY.Text = playerstate.Jet.Acceleration.Y.ToString("N2");

						pbHlth.Value = playerstate.Health;
						lblHealth.Text = "Health: " + playerstate.Health;
						pbAmmo.Value = playerstate.Ammo;
						lblAmmo.Text = "Ammo: " + playerstate.Ammo;
					}
			}
			catch(Exception e)
			{
                Console.WriteLine(e.Message);
			}
		}
    }
}
