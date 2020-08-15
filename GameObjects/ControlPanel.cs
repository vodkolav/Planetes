using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.AspNet.SignalR.Client;
using PolygonCollision;

namespace GameObjects
{
	public enum HOTAS {Up, Down, Left, Right, Shoot, Aim };
	public class ControlPanel
	{
		private Dictionary<Keys, HOTAS> KeyBindings;

		private Dictionary<MouseButtons, HOTAS> MouseBindings;

		private Player _player { get; set; }

		IHubProxy _Proxy;

		public ControlPanel(IHubProxy Proxy, Player player)
		{
			_player = player;
			_Proxy = Proxy;
			KeyBindings = new Dictionary<Keys, HOTAS>();
			MouseBindings = new Dictionary<MouseButtons, HOTAS>();
		}

		public void bindKey(Keys key, HOTAS action)
		{
			KeyBindings.Add(key, action);
		}

		public void bindMouse(MouseButtons btn, HOTAS action)
		{
			MouseBindings.Add(btn, action);
			MouseBindings.Add(MouseButtons.None, HOTAS.Aim);
		}

		public void bindWASD()
		{
			bindKey(Keys.W, HOTAS.Up);
			bindKey(Keys.S, HOTAS.Down);
			bindKey(Keys.A, HOTAS.Left);
			bindKey(Keys.D, HOTAS.Right);
			bindKey(Keys.Space, HOTAS.Shoot);
		}

		public void bindARROWSto()
		{
			bindKey(Keys.Up, HOTAS.Up);
			bindKey(Keys.Down, HOTAS.Down);
			bindKey(Keys.Left, HOTAS.Left);
			bindKey(Keys.Right, HOTAS.Right);
			bindKey(Keys.Enter, HOTAS.Shoot);
		}




		public void Press(Keys key)
		{
			if (KeyBindings.Keys.Contains(key))
			{
				HOTAS instruction = KeyBindings[key];				
				_Proxy.Invoke("Command", new object[] { _player.Name, new Tuple<Action, object>(Action.Press, instruction) });
			}
		}
		public void Release(Keys key)
		{
			if (KeyBindings.Keys.Contains(key))
			{
				HOTAS instruction = KeyBindings[key];
				_Proxy.Invoke("Command", new object[] { _player.Name, new Tuple<Action, object>(Action.Release, instruction) });
			}
		}

		public void Press(MouseButtons button)
		{
			if (MouseBindings.Keys.Contains(button))
			{
				HOTAS instruction = MouseBindings[button];
				_Proxy.Invoke("Command", new object[] { _player.Name, new Tuple<Action, object>(Action.Press, instruction) });
			}
		}

		public void Release(MouseButtons button)
		{
			if (MouseBindings.Keys.Contains(button))
			{
				HOTAS instruction = MouseBindings[button];
				_Proxy.Invoke("Command", new object[] { _player.Name, new Tuple<Action, object>(Action.Release, instruction) });
			}
		}

		public void Aim(Point at)
		{			
			if (MouseBindings.Keys.Contains(MouseButtons.None))
			{
				_Proxy.Invoke("Aim", new object[] { _player.Name, new Tuple<Action, Vector>(Action.Aim, Vector.FromPoint(at))});

			}
		}
	}
}
