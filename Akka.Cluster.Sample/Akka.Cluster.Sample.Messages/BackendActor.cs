using System;
using Akka.Actor;
using Akka.Event;
using Akka.Logger.Serilog;

namespace Akka.Cluster.Sample.IO
{
    public class BackendActor : ReceiveActor
    {
        private Akka.Cluster.Cluster Cluster = Akka.Cluster.Cluster.Get(Context.System);
        private ILoggingAdapter logger = Context.GetLogger(new SerilogLogMessageFormatter());

        public BackendActor()
        {
            Receive<FrontendCommand>(cmd =>
            {
                logger.Info("Received command {message} for job {JobId} from {Sender}", cmd.Message, cmd.JobId, Sender);
                
                Sender.Tell(new CommandComplete());
            });
        }
    }
}