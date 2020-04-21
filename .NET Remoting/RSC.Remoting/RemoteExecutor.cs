using System;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Lifetime;
namespace RSC.Remoting
{
    public abstract class RemoteExecutor : MarshalByRefObject
    {
        private const int INITIAL_LEASE_TIME = 21; // days
        private ObjRef registeredReference;
        public abstract AbstractRemoteResponse Execute(AbstractRemoteCommand command);

        /// <summary>
        /// The ObjRef registered by the RemoteExecutor. When the ObjRef is disposed,
        /// the service becomes unavailable.
        /// </summary>
        public ObjRef RegisteredReference
        {
            get
            {
                return registeredReference;
            }
        }

        public void RegisterService(string publicName, int port)
        {
            IChannel channel = new TcpServerChannel(publicName + " server channel", port);
            ChannelServices.RegisterChannel(channel, true);
            RemotingServices.SetObjectUriForMarshal(this, publicName);
            registeredReference = RemotingServices.Marshal(this, publicName, GetType());
        }

        public void UnregisterService()
        {
          RemotingServices.Disconnect(this);
        }

        public override object InitializeLifetimeService()
        {
            ILease lease = (ILease)base.InitializeLifetimeService();
            if (lease.CurrentState == LeaseState.Initial)
            {
                lease.InitialLeaseTime = TimeSpan.FromDays(INITIAL_LEASE_TIME);
                lease.RenewOnCallTime = TimeSpan.FromDays(1);
            }
            return lease;
        }
    }
}