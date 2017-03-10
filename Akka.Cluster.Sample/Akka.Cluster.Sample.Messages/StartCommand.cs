namespace Akka.Cluster.Sample.IO
{
    public class StartCommand
    {
        public string CommandText { get; private set; }

        public StartCommand(string commandText)
        {
            CommandText = commandText;
        }

        public override string ToString()
        {
            return CommandText;
        }
    }
}