using System;
using Akka.Actor;
using Akka.Cluster.Sample.IO;
using Akka.Event;
using Akka.Logger.Serilog;
using Akka.Routing;

namespace Akka.Cluster.Sample.Frontend
{
    public class FrontendActor : ReceiveActor, IWithUnboundedStash
    {
        private Akka.Cluster.Cluster Cluster = Akka.Cluster.Cluster.Get(Context.System);
        private ILoggingAdapter logger = Context.GetLogger(new SerilogLogMessageFormatter());

        private IActorRef backendRouter;

        private int jobCount = 0;

        private ICancelable timer;

        private int counter = 0;

        public IStash Stash { get; set; }

        protected override void PreStart()
        {
            Cluster.Subscribe(Self, new[] {typeof(ClusterEvent.MemberUp)});

            if (Context.Child("backend").Equals(ActorRefs.Nobody))
            {
                backendRouter = Context.ActorOf(
                    Props.Create(() => new BackendActor())
                        .WithRouter(FromConfig.Instance),
                    "backend");
            }
            else //this would only enter when the frontend actor restarts
            {
                backendRouter = Context.Child("backend");
            }

            base.PreStart();
        }

        protected override void PostStop()
        {
            timer.Cancel();
            Cluster.Unsubscribe(Self);
        }

        public FrontendActor()
        {
            Receive<ClusterEvent.MemberUp>(up =>
            {
                if (up.Member.Address == Cluster.SelfAddress)
                {
                    Become(ReadyToProcess);
                }
            });

            ReceiveAny(_ =>
            {
                Stash.Stash();
            });
        }

        public void ReadyToProcess()
        {
            Receive<StartCommand>(start =>
            {
                backendRouter.Tell(new FrontendCommand($"message {jobCount++}", start.CommandText));
                backendRouter.Tell(new FrontendCommand($"message {jobCount++}", start.CommandText));
            });

            Receive<CommandComplete>(complete =>
            {
                logger.Info("Received CommandComplete from {Sender}", Sender);
            });

            var interval = TimeSpan.FromSeconds(5);

            var me = Self;
            timer = Context.System.Scheduler.Advanced.ScheduleRepeatedlyCancelable(interval, interval, () => me.Tell(new StartCommand($"Hello {counter++}")));
        }
    }
}