﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Planetes;

namespace TestClient
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Thread.Sleep(1000);
#if DEBUG
            Application.Run(new Game("joinNetworkGame"));
#else
            Application.Run(new Game());
#endif
        }
    }
}
