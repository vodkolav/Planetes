﻿using Microsoft.AspNet.SignalR.Client;
using PolygonCollision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Action = GameObjects.Model.Action;

namespace GameObjects
{
    public enum HOTAS { Up, Down, Left, Right, Shoot, Brake, Nothing, Scuttle, GameSpeedUp, GameSpeedDown }
    public class ControlPanel
    {
        private Dictionary<Key, HOTAS> KeyBindings;

        private Dictionary<MouseButton, HOTAS> MouseBindings;

        private HOTAS LastPress { get; set; } = HOTAS.Nothing;

        private float LastAimed { get; set; } = 0f;

        private int PlayerID { get; set; }

        private IHubProxy Proxy { get; set; }

        private bool isWorking { get; set; }

        public ControlPanel(IHubProxy Proxy, int playerid)
        {
            PlayerID = playerid;
            this.Proxy = Proxy;
            KeyBindings = new Dictionary<Key, HOTAS>();
            MouseBindings = new Dictionary<MouseButton, HOTAS>();
            isWorking = true;
        }

        public void bindKey(Key key, HOTAS action)
        {
            KeyBindings.Add(key, action);
        }

        public void bindMouse()
        {            
            MouseBindings.Add(MouseButton.Left, HOTAS.Shoot);
            //MouseBindings.Add(MouseButton.None, HOTAS.Aim);
        }

        public void bindWASD()
        {
            bindKey(Key.W, HOTAS.Up);
            bindKey(Key.S, HOTAS.Down);
            bindKey(Key.A, HOTAS.Left);
            bindKey(Key.D, HOTAS.Right);
            bindKey(Key.Space, HOTAS.Brake);
            bindKey(Key.L, HOTAS.Scuttle);
            bindKey(Key.Add, HOTAS.GameSpeedUp);
            bindKey(Key.Subtract, HOTAS.GameSpeedDown);
        }

        public void bindARROWSto()
        {
            bindKey(Key.Up, HOTAS.Up);
            bindKey(Key.Down, HOTAS.Down);
            bindKey(Key.Left, HOTAS.Left);
            bindKey(Key.Right, HOTAS.Right);
            bindKey(Key.Enter, HOTAS.Shoot);
        }

        public void unbind()
        {
            KeyBindings.Clear();
            MouseBindings.Clear();
            isWorking = false;
        }
        public void Press(HOTAS argument)
        {
            if (isWorking && argument != LastPress)
            {
                LastPress = argument;
                Proxy.Invoke("Command", new object[] { PlayerID, new Tuple<Action, HOTAS>(Action.Press, argument) });
            }
        }

        public void Release(HOTAS argument)
        {
            if (isWorking)
            {
                LastPress = HOTAS.Nothing;
                Proxy.Invoke("Command", new object[] { PlayerID, new Tuple<Action, HOTAS>(Action.Release, argument) });
            }
        }
            
        public void Do(Action instruction, Vector argument)
        {
            //instructions will probably be required later, for example to apply abilities at something/someone

            if (isWorking)
                Proxy.Invoke("Do", new object[] { PlayerID, new Tuple<Action, Vector>(instruction, argument) });
        }

        public void Aim(Vector ScreenCoordinate)
        {
            if (LastAimed + 0.05 < GameTime.TotalElapsedSeconds)
            {
                Do(Action.Aim, ScreenCoordinate);               
                LastAimed = GameTime.TotalElapsedSeconds;
            }
        }

        public void Press(int key)
        {
            Press((Key)key);
        }

        public void Press(Key key)
        {
            if (KeyBindings.Keys.Contains(key))
            {
                HOTAS instruction = KeyBindings[key];
                Press(instruction);
            }
        }

        public void Release(int key)
        {
            Release((Key)key);
        }
        
        public void Release(Key key)
        {
            if (KeyBindings.Keys.Contains(key))
            {
                HOTAS instruction = KeyBindings[key];
                Release(instruction);
            }
        }  

        public void Press(MouseButton button)
        {
            if (MouseBindings.Keys.Contains(button))
            {
                HOTAS instruction = MouseBindings[button];
                Press(instruction);
            }
        }
          
        public void Release(MouseButton button)
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
