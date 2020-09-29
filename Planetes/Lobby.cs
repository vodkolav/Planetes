using GameObjects;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Planetes
{
    public partial class Lobby : Form, Ilobby
    {
        BindingList<Player> players;

        public Lobby()
        {
            InitializeComponent();
            DialogResult = DialogResult.Ignore;
        }

        public Lobby(Form owner) : this()
        {
            Owner = owner;
            Text = owner.Text;
        }

        public void UpdateLobby(GameState state)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<GameState>(UpdateLobby), new object[] { state });
            }
            else
            {
                state.players.ForEach(p => Console.WriteLine(p.Name));
                players = new BindingList<Player>(state.players);
                playerBindingSource = new BindingSource(players, null);
                dgvwPlayers.DataSource = playerBindingSource;
                playerBindingSource.ResetBindings(false);
            }
        }



        private void gameServerBindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (((Game)Owner).isServer)
            {
                if (players.Count < 2)
                {
                    MessageBox.Show("It takes two to tango", "Wait a second!");
                }
                else
                {
                    DialogResult = DialogResult.OK;
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

        private async void btnCancel_Click(object sender, EventArgs e)
        {
            await ((Game)Owner).LeaveLobby();
            Console.WriteLine("oops");
            DialogResult = DialogResult.Ignore;
            Close();
        }

        //purge these later
        private void playerBindingSource_ListChanged(object sender, ListChangedEventArgs e)
        {
            Console.WriteLine("ListChanged");
        }

        private void playerBindingSource_DataSourceChanged(object sender, EventArgs e)
        {
            Console.WriteLine("DataSourceChanged");
        }

        private void dgvwPlayers_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //will be needed further so that players will be able to update their name in lobby
        }

        bool Ilobby.OpenLobby_WaitForGuestsAndBegin()
        {
            if (ShowDialog(Owner) == DialogResult.OK)
            {
                return true;
            }
            return false;
        }

        private void btnAddBot_Click(object sender, EventArgs e)
        {
            ((Game)Owner).AddBot();
        }

        private void btnKickPlayer_Click(object sender, EventArgs e)
        {
            Player kickedone = (Player)dgvwPlayers.SelectedRows[0].DataBoundItem;
            ((Game)Owner).KickPlayer(kickedone);
        }
    }
}
