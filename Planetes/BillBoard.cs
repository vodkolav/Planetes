using System.Windows.Forms;
using System.Drawing;

namespace Planetes
{
    public partial class BillBoard : Form
    {  
        public BillBoard(Form owner)
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterParent;
            Owner = owner;            
        }

        internal void Show(IWin32Window game, string message)
        {
            Text = message;
            Location = Owner.Location + new Size(50, 50);
            Show(game);
        }
    }
}
