using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;
using Akka.Event;
using Serilog;

namespace Akka.Cluster.Sample.Backend
{
    class Program
    {
        static void Main(string[] args)
        {
            Serilog.Log.Logger = new LoggerConfiguration()
                    .WriteTo.ColoredConsole()
                    .MinimumLevel.Information()
                    .CreateLogger();
            
            using (var system = ActorSystem.Create("ClusterSystem"))
            {
                var cluster = Cluster.Get(system);
                Console.Title = $"Backend system {cluster.SelfAddress}";

                Console.WriteLine("Started. Press enter to stop");
                Console.ReadLine();
                
                cluster.LeaveAsync().Wait();

                Console.WriteLine("Node exited cluster. Hit enter to stop");
            }
        }
    }
}
