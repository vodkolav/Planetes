using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
