using GameObjects;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace PlanetesWPF
{
    /// <summary>
    /// Interaction logic for Lobby.xaml
    /// </summary>
    public partial class Lobby : Window
    {
        ObservableCollection<Player> players = new ObservableCollection<Player>();
        
        public Lobby(Window owner) : this()
        {
            Title = owner.Title;            
        }       

        public Lobby()
        {
            InitializeComponent();
            dgPlayers.AutoGenerateColumns = false;
            dgPlayers.ItemsSource = players;
        }

        public bool OpenLobby_WaitForGuestsAndBegin(Window uI)
        {
            Owner = uI;
            var res = ShowDialog();
            Owner = uI;
            return res.HasValue && res.Value;
        }

        internal void UpdateLobby(GameState go)
        {
            Dispatcher.BeginInvoke(new System.Action(() =>
            {
                players.Clear();
                go.Players.ForEach(p => players.Add(p));
            }
            ));
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (((UI)Owner).isServer)
            {
                if (players.Count < 2)
                {
                    MessageBox.Show("It takes two to tango", "Wait a second!");
                }
                else
                {
                    DialogResult = true;
                    Close();
                }
            }
            else
            {
                string mess = "Only the host can start the game";
                MessageBox.Show(mess);
                Console.WriteLine(mess);
            }

        }

        private void btnAddBot_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                ((UI)Owner).AddBot();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Wait a second!");
                Console.WriteLine(ex.Message);
            }          
        }

        private void btnKickPlayer_Click(object sender, RoutedEventArgs e)
        {            
            Player kickedone = (Player)dgPlayers.SelectedItem;
            ((UI)Owner).KickPlayer(kickedone);
        }

        private async void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            await((UI)Owner).LeaveLobby();
            Console.WriteLine("Ну нахер");
            DialogResult = false;
            Close();
        }
    }
}
