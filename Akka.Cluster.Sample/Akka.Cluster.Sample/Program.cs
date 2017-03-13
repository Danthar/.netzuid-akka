using System;
using Akka.Actor;
using Serilog;

namespace Akka.Cluster.Sample.Frontend
{
    class Program
    {
        static void Main(string[] args)
        {
            Serilog.Log.Logger = new LoggerConfiguration()
                    .WriteTo.ColoredConsole()
                    .MinimumLevel.Information()
                    .CreateLogger();

            Console.Title = $"Frontend system";

            using (var system = ActorSystem.Create("ClusterSystem"))
            {

                system.ActorOf(Props.Create<FrontendActor>(), "frontend");

                Console.WriteLine("Started. Press enter to stop");
                Console.ReadLine();
            }
        }
    }
}
