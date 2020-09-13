
using System;
using System.Threading;
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
            bool autoconnect = false;
#if DEBUG
            Application.Run( autoconnect ? new Game("joinNetworkGame") : new Game());
#else
            Application.Run(new Game());
#endif
        }
    }
}
