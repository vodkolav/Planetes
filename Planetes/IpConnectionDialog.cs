﻿using System;
using System.Windows.Forms;

namespace Planetes
{
    public partial class IpConnectionDialog : Form
    {
        public IpConnectionDialog(string ip = "127.0.0.1", string port = "2861")
        {
            InitializeComponent();
            ipAddressControl1.Text = ip; //TODO: replace this with https://github.com/m66n/ipaddresscontrollib
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
                if (ipAddressControl1.Text == "...")
                {
                    return $"http://127.0.0.1:2861/";
                }
                else 
                {
                    return "http://" + ipAddressControl1.Text + ":" + tbxport.Text;
                }
            }
        }
    }
}
