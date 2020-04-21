/*using System;
using System.Collections;
using Services;

namespace Server
{
    public class ServerLauncher
    {
        public static void Main(string[] args)
        {
            
            TcpChannel channel = new TcpChannel(9898);
            ChannelServices.RegisterChannel(channel,false);
            RemotingConfiguration.CustomErrorsMode = CustomErrorsModes.Off;
            RemotingConfiguration.ApplicationName = "Server";
            RemotingConfiguration.RegisterWellKnownServiceType(typeof(SimpleService), "service", WellKnownObjectMode.SingleCall);

            Console.WriteLine("Server Ready!");
            Console.WriteLine("Press enter for shutdown.");
            Console.ReadLine();

            ChannelServices.UnregisterChannel(channel);
        }
    }

   
}*/