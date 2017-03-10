using System;
using Akka.Actor;

namespace Akka.Cluster.Sample.IO
{
    public class BackendActor : ReceiveActor
    {
        public Akka.Cluster.Cluster Cluster = Akka.Cluster.Cluster.Get(Context.System);

        public BackendActor()
        {
            Receive<FrontendCommand>(cmd =>
            {
                Console.WriteLine($"Received command {cmd.Message} for job {cmd.JobId} from {Sender}");

                Sender.Tell(new CommandComplete());
            });
        }
    }
}