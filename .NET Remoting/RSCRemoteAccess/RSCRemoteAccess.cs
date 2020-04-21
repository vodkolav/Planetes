using System;
using System.Collections.Generic;
using System.Text;

namespace RSCRemoteAccess
{
    public class RSCRemoteAccess : MarshalByRefObject
    {
        public RSCRemoteAccess()
        {
            //System.Runtime.Remoting.RemotingConfiguration.CustomErrorsMode = System.Runtime.Remoting.CustomErrorsModes.Off;
        }

        //delegieren des Typs ReceiveMessage
        public delegate object ReceiveMessage(object sender, EventArgs ev, string message, string[] parameter);
        //Event vom Type des Delegaten erstellen
        public event ReceiveMessage recMessage;

        public object DoMethod(string methodName,string[] parameter)
        {
            return recMessage(this, new EventArgs(), methodName,parameter);
        }
    }
}
