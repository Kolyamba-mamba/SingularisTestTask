using System;
using System.Drawing;
using Common;
using MatthiWare.CommandLine;

namespace Converter
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var settings = ParseCommandLine(args);
            using var messageReceiver = new MessageReceiver<BusMessage>(settings?.hostName);
            var fileManager = new FileManager();
            var directoryManager = new DirectoryManager(settings?.folderPath);
            if (settings is null)
                return;
            var imageConverter = new WatermarkOverlayer(settings?.logo,
                new WatermarkSettings
                {
                    Opacity = (int) settings?.settings.Opacity, 
                    Position = (WatermarkPosition) settings?.settings.Position, 
                    WatermarkRelativeSize = (int) settings?.settings.WatermarkRelativeSize,
                    ShouldResizeWatermark = (bool) settings?.settings.ShouldResizeWatermark
                });
            var converter = new Converter(messageReceiver, imageConverter, fileManager, directoryManager);
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

        private static (string hostName, string folderPath, Image logo, WatermarkSettings settings)? ParseCommandLine(string[] args)
        {
            var parser = new CommandLineParser<CommandLineOptions>();
            var parsingResult = parser.Parse(args);
            if (parsingResult.HelpRequested)
                return null;
            var result = parsingResult.Result;
            return (result.HostName,
                result.FolderPath,
                Image.FromFile(result.WatermarkPath), new WatermarkSettings
            {
                Opacity = result.Opacity,
                WatermarkRelativeSize = result.WatermarkRelativeSize,
                Position = result.WatermarkPosition,
                ShouldResizeWatermark = result.ShouldResizeWatermark
            });
        }
    }
}