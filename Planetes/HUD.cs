using GameObjects;
using PolygonCollision;
using System.Linq;
using System.Windows.Forms;

namespace Planetes
{
    public partial class HUD : UserControl
	{
		Game GAME;
		string player;

		public HUD()
		{			
			InitializeComponent();
		}
		public void bind(Game game, Player p)
		{
			GAME = game;
			player = p.Name;
			pbAmmo.Maximum = p.MaxAmmo;
			pbHlth.Maximum = p.MaxHealth;
			Visible = true;
			BackColor = p.Jet.Color;
		}

		public void Draw()
		{
			Player playerstate = GAME.gameObjects.players.Single(p => p.Name == player);
			lock (GAME.gameObjects)
			{
				lblSpeedX.Text = ((int)playerstate.Jet.Speed.X).ToString();
				lblSpeedY.Text = ((int)playerstate.Jet.Speed.Y).ToString();

				Vector acc = (Vector)playerstate.Jet.Acceleration.Clone();
				lblAccX.Text = acc.X.ToString();
				lblAccY.Text = acc.Y.ToString();

				pbHlth.Value = playerstate.Health;
				lblHealth.Text = "Health: " + playerstate.Health;
				pbAmmo.Value = playerstate.Ammo;
				lblAmmo.Text = "Ammo: " + playerstate.Ammo;
			}
		}
	}
}
