using System;

namespace Watcher
{
    public class ConsoleMessageSender : IMessageSender
    {
        public void Send(string filename)
        {
            Console.WriteLine(filename);
        }
    }
}