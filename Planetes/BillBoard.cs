using System;
using System.Windows.Forms;

namespace Planetes
{
    public partial class BillBoard : Form
    {  
        public BillBoard(Form owner): this()
        {
            Owner = owner;            
        }
        public BillBoard()
        {
            InitializeComponent();
        }

        internal void Show(IWin32Window game, string message)
        {
            Show(game);
            Text = message;
        }
    }
}
