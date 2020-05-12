using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameObjects
{
	public enum HOTAS {Up, Down, Left, Right, Shoot };
	public class Controls
	{
		private Dictionary<Keys, Tuple<Player, HOTAS>> bindings;

		public Controls()
		{
			bindings = new Dictionary<Keys, Tuple<Player, HOTAS>>();

		}

		public void bind(Keys key, Player player, HOTAS action)
		{
			bindings.Add(key, new Tuple<Player, HOTAS>(player, action));
		}

		public void bindWASDto(Player player)
		{
			bind(Keys.W, player, HOTAS.Up);
			bind(Keys.S, player, HOTAS.Down);
			bind(Keys.A, player, HOTAS.Left);
			bind(Keys.D, player, HOTAS.Right);
			bind(Keys.Space, player, HOTAS.Shoot);
		}

		public void bindARROWSto(Player player)
		{
			bind(Keys.Up, player, HOTAS.Up);
			bind(Keys.Down, player, HOTAS.Down);
			bind(Keys.Left, player, HOTAS.Left);
			bind(Keys.Right, player, HOTAS.Right);
			bind(Keys.Enter, player, HOTAS.Shoot);
		}

		public void Press(Keys key)
		{
			if (bindings.Keys.Contains(key))
			{
				Tuple<Player, HOTAS> t = bindings[key];
				t.Item1.Steer(t.Item2);
			}
		}

		public void Release(Keys key)
		{
			if (bindings.Keys.Contains(key))
			{
				Tuple<Player, HOTAS> t = bindings[key];
				t.Item1.Release(t.Item2);
			}
		}
	}
}
