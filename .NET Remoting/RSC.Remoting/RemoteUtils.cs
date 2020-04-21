namespace RSC.Remoting
{
  using System.Runtime.Remoting.Channels;
  using System.Runtime.Remoting.Channels.Tcp;
  using System.Text;
  using System.Runtime.Remoting;
  using System.Net;
  using System.Net.Sockets;

  public sealed class RemoteUtils
  {
    private const int SOCKET_ALREADY_IN_USE = 10048;

    private RemoteUtils() { }

    public static RemoteExecutor GetRemoteExecutor(string hostname, string name, int port)
    {
      // test if there is already a channel registered for communicating with servers
      if (ChannelServices.GetChannel("tcp") == null)
        ChannelServices.RegisterChannel(new TcpClientChannel(), true);
      StringBuilder url = new StringBuilder("tcp://").Append(hostname).Append(":")
        .Append(port).Append("/").Append(name);

      RemoteExecutor executor = (RemoteExecutor)RemotingServices.Connect(typeof(RemoteExecutor),
        url.ToString());
      return executor;
    }

    public static bool IsPortAvailable(int portNumber)
    {
      if (portNumber < IPEndPoint.MinPort || portNumber > IPEndPoint.MaxPort)
      {
        return false;
      }
      try
      {
        Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.Bind(new IPEndPoint(IPAddress.Any, portNumber));
        socket.Close();
      }
      catch (SocketException ex)
      {
        if (ex.ErrorCode == SOCKET_ALREADY_IN_USE)
        {
          return false;
        }
      }
      return true;
    }
  }
}
