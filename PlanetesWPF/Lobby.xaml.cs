using GameObjects;
using System;
using System.Windows;

namespace PlanetesWPF
{
    /// <summary>
    /// Interaction logic for Lobby.xaml
    /// </summary>
    public partial class Lobby : Window
    {
        public Lobby()
        {
            InitializeComponent();
        }
        public bool OpenLobby_WaitForGuestsAndBegin()
        {
            var res = ShowDialog();
            if (res.HasValue && res.Value )
            {
                return true;
            }
            return false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        internal void UpdateLobby(GameState go)
        {
            Console.WriteLine("updating lobby ");
        }
    }
}
