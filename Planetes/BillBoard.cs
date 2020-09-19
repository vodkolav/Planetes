using System.Windows.Forms;

namespace Planetes
{
    public partial class BillBoard : Form
    {
        public BillBoard()
        {
            InitializeComponent();
        }

        internal void Show(Form game, string message)
        {
            Show(game);
            Text = message;
        }
    }
}
