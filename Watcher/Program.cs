using System;
using System.Net.Mime;
using Common;
using MatthiWare.CommandLine;

namespace Watcher
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var settings = ParseCommandLine(args);
            using var directoryWatcher = new DirectoryWatcher(settings?.folderPath);
            using var messageSender = new MessageSender<BusMessage>(settings?.hostName);
            var shouldDelete = settings?.isDelete;
            var watcher = new Watcher(messageSender, directoryWatcher, new FileManager(), shouldDelete);
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

        private static (string hostName, string folderPath, bool isDelete)? ParseCommandLine(string[] args)
        {
            var parser = new CommandLineParser<CommandLineOptions>();
            var parsingResult = parser.Parse(args);
            if (parsingResult.HelpRequested)
                return null;
            var result = parsingResult.Result;
            return (
                result.HostName, 
                result.FolderPath,
                result.ShouldDelete
            );
        }
    }
}