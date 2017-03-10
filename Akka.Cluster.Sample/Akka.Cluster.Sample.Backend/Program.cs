using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Actor;

namespace Akka.Cluster.Sample.Backend
{
    class Program
    {
        static void Main(string[] args)
        {
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
