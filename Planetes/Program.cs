using System;
using System.Windows.Forms;

namespace Planetes
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
            string gametype ="SinglePlayer" ;//"hostNetworkGame";//  "joinNetworkGame", 
#if DEBUG
            Application.Run(new Game(gametype));
#else
            Application.Run(new Game());
#endif
        }
    }
}
