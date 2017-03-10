using Akka.Routing;

namespace Akka.Cluster.Sample.IO
{
    public class FrontendCommand : IConsistentHashable
    {
        public string Message { get; private set; }

        public string JobId { get; private set; }

        public object ConsistentHashKey => JobId;

        public FrontendCommand(string message, string jobId)
        {
            Message = message;
            JobId = jobId;
        }
    }
}