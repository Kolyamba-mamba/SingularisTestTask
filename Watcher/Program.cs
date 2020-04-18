using System;
using Common;

namespace Watcher
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            const string incomingString = "incoming";
            using var directoryWatcher = new DirectoryWatcher(incomingString); 
            using var messageSender = new MessageSender<BusMessage>();
            var watcher = new Watcher(messageSender, directoryWatcher, new FileManager());
            Run();
        }

        private static void Run()
        {
            const string exitMessage = "quit";
            while (true)
            {
                Console.WriteLine($"Press '{exitMessage}' to quit the app.");
                if (Console.ReadLine() == exitMessage)
                    break;
            }
        }
    }
}