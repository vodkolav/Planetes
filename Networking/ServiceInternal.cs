using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Channels.Tcp;
using Services;
using GameObjects;
using System.Windows.Forms;

namespace Service.Internal
{
    public class SimpleService : MarshalByRefObject, IService
    {
        static SimpleService()
        {
            MessageBox.Show("Server created");
        }

        public void Operation(string arg)
        {
            MessageBox.Show("Connection established");
        }

    }
}