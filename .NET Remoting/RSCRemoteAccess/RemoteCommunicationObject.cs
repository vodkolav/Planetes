using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;

namespace RSCRemoteAccess
{
    public class RemoteCommunicationObject
    {
        private RemoteType m_type;
        private int m_port;
        private string m_name;
        private string m_target;

        private bool tcpRegistered = false;

        public RemoteCommunicationObject(RemoteType type,string target,int port, string name)
        {
            m_type = type;
            m_name = name;
            m_port = port;
            m_target = target;
        }

        public RSCRemoteAccess GetRemoteObject()
        {
            if (m_type == RemoteType.Client)
            {
                return GetClientObject();
            }
            else
            {
                return GetServerObject();
            }
        }

        private RSCRemoteAccess GetClientObject()
        {
            if (tcpRegistered)
            {
                TcpChannel myClientChannel = new TcpChannel();
                ChannelServices.RegisterChannel(myClientChannel, false);
                tcpRegistered = true;
            }

            return (RSCRemoteAccess)Activator.GetObject(typeof(RSCRemoteAccess), "tcp://" + m_target + ":" + m_port + "/" + m_name);
        }

        private RSCRemoteAccess GetServerObject()
        {
            //Ablaufzeit des Remoteobjekts
            LifetimeServices.LeaseTime = TimeSpan.FromDays(5);
            //Zeitspanne in der der LeaseTimeManager den Pool 'reinigt'
            LifetimeServices.LeaseManagerPollTime = TimeSpan.FromDays(1);
            //Zeit um die das Remoteobjekt verlängert bei jedem Call
            LifetimeServices.RenewOnCallTime = TimeSpan.FromDays(1);

            BinaryClientFormatterSinkProvider clientProvider = new BinaryClientFormatterSinkProvider();
            BinaryServerFormatterSinkProvider serverProvider = new BinaryServerFormatterSinkProvider();
            serverProvider.TypeFilterLevel = TypeFilterLevel.Full;

            IDictionary p = new Hashtable();
            p["port"] = m_port;
            p["name"] = m_name;

            TcpChannel myChannel = new TcpChannel(p, clientProvider, serverProvider);
            ChannelServices.RegisterChannel(myChannel, false);

			RemotingConfiguration.RegisterWellKnownServiceType(typeof(RSCRemoteAccess),m_name, WellKnownObjectMode.Singleton);

            string uri = "tcp://" + m_target + ":" + m_port + "/" + m_name;
            return (RSCRemoteAccess)Activator.GetObject(typeof(RSCRemoteAccess), "tcp://" + m_target + ":" + m_port + "/" + m_name);
        }
    }
}
