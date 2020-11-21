using System;
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
            if (InvokeRequired)
            {
                Invoke(new Action<Form,string>(Show), new object[] { message });
            }
            else
            {
                Show(game);
                Text = message;
            }
        }
    }
}
