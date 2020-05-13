using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PolygonCollision;

namespace GameObjects
{
	public enum HOTAS {Up, Down, Left, Right, Shoot, Aim };
	public class ControlPanel
	{
		private Dictionary<Keys, Tuple<Player, HOTAS>> KeyBindings;

		private Dictionary<MouseButtons, Tuple<Player, HOTAS>> MouseBindings;

		public ControlPanel()
		{
			KeyBindings = new Dictionary<Keys, Tuple<Player, HOTAS>>();
			MouseBindings = new Dictionary<MouseButtons, Tuple<Player, HOTAS>>();
		}

		public void bindKey(Keys key, Player player, HOTAS action)
		{
			KeyBindings.Add(key, new Tuple<Player, HOTAS>(player, action));
		}

		public void bindMouse(MouseButtons btn, Player player, HOTAS action)
		{
			MouseBindings.Add(btn, new Tuple<Player, HOTAS>(player, action));
			MouseBindings.Add(MouseButtons.None, new Tuple<Player, HOTAS>(player, HOTAS.Aim));
		}

		public void bindWASDto(Player player)
		{
			bindKey(Keys.W, player, HOTAS.Up);
			bindKey(Keys.S, player, HOTAS.Down);
			bindKey(Keys.A, player, HOTAS.Left);
			bindKey(Keys.D, player, HOTAS.Right);
			bindKey(Keys.Space, player, HOTAS.Shoot);
		}

		public void bindARROWSto(Player player)
		{
			bindKey(Keys.Up, player, HOTAS.Up);
			bindKey(Keys.Down, player, HOTAS.Down);
			bindKey(Keys.Left, player, HOTAS.Left);
			bindKey(Keys.Right, player, HOTAS.Right);
			bindKey(Keys.Enter, player, HOTAS.Shoot);
		}




		public void Press(Keys key)
		{
			if (KeyBindings.Keys.Contains(key))
			{
				Tuple<Player, HOTAS> t = KeyBindings[key];
				t.Item1.Steer(t.Item2);
			}
		}

		public void Press(MouseButtons button)
		{
			if (MouseBindings.Keys.Contains(button))
			{
				Tuple<Player, HOTAS> t = MouseBindings[button];
				t.Item1.Steer(t.Item2);
			}
		}

		public void Release(Keys key)
		{
			if (KeyBindings.Keys.Contains(key))
			{
				Tuple<Player, HOTAS> t = KeyBindings[key];
				t.Item1.Release(t.Item2);
			}
		}

		public void Release(MouseButtons button)
		{
			if (MouseBindings.Keys.Contains(button))
			{
				Tuple<Player, HOTAS> t = MouseBindings[button];
				t.Item1.Release(t.Item2);
			}
		}

		public void Aim(Vector vector)
		{			
			if (MouseBindings.Keys.Contains(MouseButtons.None))
			{
				Tuple<Player, HOTAS> t = MouseBindings[MouseButtons.None];
				t.Item1.Aim(vector);
			}
		}
	}
}
