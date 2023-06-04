using Microsoft.AspNet.SignalR.Client;
using PolygonCollision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GameObjects
{
    public enum HOTAS { Up, Down, Left, Right, Shoot, Aim, Brake };
    public class ControlPanel
    {
        private Dictionary<Keys, HOTAS> KeyBindings;

        private Dictionary<MouseButtons, HOTAS> MouseBindings;

        private int PlayerID { get; set; }

        private IHubProxy Proxy { get; set; }

        private bool isWorking { get; set; }

        public ControlPanel(IHubProxy Proxy, int playerid)
        {
            PlayerID = playerid;
            this.Proxy = Proxy;
            KeyBindings = new Dictionary<Keys, HOTAS>();
            MouseBindings = new Dictionary<MouseButtons, HOTAS>();
            isWorking = true;
        }

        public void bindKey(Keys key, HOTAS action)
        {
            KeyBindings.Add(key, action);
        }

        public void bindMouse()
        {            
            MouseBindings.Add(MouseButtons.Left, HOTAS.Shoot);
            MouseBindings.Add(MouseButtons.None, HOTAS.Aim);
        }

        public void bindWASD()
        {
            bindKey(Keys.W, HOTAS.Up);
            bindKey(Keys.S, HOTAS.Down);
            bindKey(Keys.A, HOTAS.Left);
            bindKey(Keys.D, HOTAS.Right);
            bindKey(Keys.Space, HOTAS.Brake);
        }

        public void bindARROWSto()
        {
            bindKey(Keys.Up, HOTAS.Up);
            bindKey(Keys.Down, HOTAS.Down);
            bindKey(Keys.Left, HOTAS.Left);
            bindKey(Keys.Right, HOTAS.Right);
            bindKey(Keys.Enter, HOTAS.Shoot);
        }

        public void unbind()
        {
            KeyBindings.Clear();
            MouseBindings.Clear();
            isWorking = false;
        }
        public void Press(HOTAS argument)
        {
            if (isWorking)
                Proxy.Invoke("Command", new object[] { PlayerID, new Tuple<Action, HOTAS>(Action.Press, argument) });
        }

        public void Release(HOTAS argument)
        {
            if (isWorking)
                Proxy.Invoke("Command", new object[] { PlayerID, new Tuple<Action, HOTAS>(Action.Release, argument) });
        }
            
        public void Do(Action instruction, object argument)
        {
            //instructions will probably be required later, for example to apply abilities at something/someone
            if (isWorking)
                Proxy.Invoke("Command", new object[] { PlayerID, new Tuple<Action, object>(instruction, argument) });
        }

        public void Aim(Vector argument)
        {
            if (isWorking)
                Proxy.Invoke("Aim", new object[] { PlayerID, new Tuple<Action, Vector>(Action.Aim, argument) });
            // these exceptions appear after waiting too long on a breakpoint. how to fix : https://stackoverflow.com/a/38161578

        }

        public void Press(int key)
        {
            Press((Keys)key);
        }

        public void Press(Keys key)
        {
            if (KeyBindings.Keys.Contains(key))
            {
                HOTAS instruction = KeyBindings[key];
                Press(instruction);
            }
        }

        public void Release(int key)
        {
            Release((Keys)key);
        }
        
        public void Release(Keys key)
        {
            if (KeyBindings.Keys.Contains(key))
            {
                HOTAS instruction = KeyBindings[key];
                Release(instruction);
            }
        }  

        public void Press(MouseButtons button)
        {
            if (MouseBindings.Keys.Contains(button))
            {
                HOTAS instruction = MouseBindings[button];
                Press(instruction);
            }
        }
          
        public void Release(MouseButtons button)
        {
            if (MouseBindings.Keys.Contains(button))
            {
                HOTAS instruction = MouseBindings[button];
                Release(instruction);
            }
        }

        public void StopGame()
        {
            Proxy.Invoke("Over");
        }
    }
}
