using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Event;

namespace Akka.Remoting.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var system = ActorSystem.Create("ZuidServer"))
            {
                Console.Title = ".Net Zuid Server";
                var server = system.ActorOf<ServerActor>("server");
                Console.ReadLine();
                Console.WriteLine("Shutting down...");
                server.GracefulStop(TimeSpan.FromSeconds(2));
                Console.WriteLine("Terminated");
            }
        }
    }

    public class ServerActor : ReceiveActor
    {
        private readonly ILoggingAdapter _log = Context.GetLogger();

        public ServerActor()
        {
            Receive<string>(m =>
            {
                Console.WriteLine("Received {0}", m);
                Sender.Tell("ack");
            });

        }
    }
}
