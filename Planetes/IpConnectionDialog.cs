using System;
using System.Windows.Forms;

namespace Planetes
{
    public partial class IpConnectionDialog : Form
    {
        public IpConnectionDialog(string ip = "127.0.0.1", string port = "8030")
        {
            InitializeComponent();
            ipAddressControl1.Text = ip;
            tbxport.Text = port;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        public string IP
        { get { return ipAddressControl1.Text; } }

        public string URL
        {
            get
            {
                return "http://" + ipAddressControl1.Text + ":" + tbxport.Text;
            }
        }
    }
}
