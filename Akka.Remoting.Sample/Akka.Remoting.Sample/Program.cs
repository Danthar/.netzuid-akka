using System;
using System.Collections.Generic;
using System.Diagnostics;
using Akka.Actor;

namespace Akka.Remoting.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var system = ActorSystem.Create("ZuidClient"))
            {
                var tmp = system.ActorSelection("akka.tcp://ZuidServer@localhost:9391/user/server")
                    .ResolveOne(TimeSpan.FromSeconds(5)).Result;

                Console.Title = $"TimeClient {Process.GetCurrentProcess().Id}";

                var client = system.ActorOf(Props.Create(() => new ClientActor(tmp)));

                Console.ReadLine();
                Console.WriteLine("Shutting down...");
                Console.WriteLine("Terminated");
            }
        }
        
    }

    public class ClientActor : ReceiveActor
    {
        private class Tick
        {
            public static readonly Tick Instance = new Tick();
        }

        private readonly IActorRef _server;
        private readonly ICancelable _timer;

        public ClientActor(IActorRef server)
        {
            _server = server;

            Receive<string>(m =>
            {
                Console.WriteLine(m);
            });

            Receive<Tick>(_ =>
            {
                
                _server.Tell($"Hello from: {Process.GetCurrentProcess().Id}");

            });

            _timer = Context.System.Scheduler.ScheduleTellRepeatedlyCancelable(TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(1), Self, Tick.Instance, Self);
        }

        protected override void PostStop()
        {
            _timer.Cancel();
            base.PostStop();
        }
    }
}
