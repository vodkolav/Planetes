using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PolygonCollision;
using GameObjects;

namespace Planetes
{
	public partial class HUD : UserControl
	{
		ClsGameObjects GO;
		Player player;

		public HUD()
		{			
			InitializeComponent();
		}
		public void bind(ClsGameObjects gO, Player p)
		{
			GO = gO;
			player = p;
			pbAmmo.Maximum = player.MaxAmmo;
			pbHlth.Maximum = player.MaxHealth;
			Visible = true;
			BackColor = player.Jet.Color;
		}

		public void Draw()
		{
			lock (GO)
			{
				lblSpeedX.Text = ((int)player.Jet.Speed.X).ToString();
				lblSpeedY.Text = ((int)player.Jet.Speed.Y).ToString();

				lblAccX.Text = player.Jet.Acceleration.X.ToString();
				lblAccY.Text = player.Jet.Acceleration.Y.ToString();

				pbHlth.Value = player.Health;
				lblHealth.Text = "Health: " + player.Health;
				pbAmmo.Value = player.Ammo;
				lblAmmo.Text = "Ammo: " + player.Ammo;
			}
		}
	}
}
