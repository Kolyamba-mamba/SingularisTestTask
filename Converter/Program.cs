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
            const string outDirectory = "outcoming";
            using var messageReceiver = new MessageReceiver<BusMessage>();

            var fileManager = new FileManager();
            var directoryManager = new DirectoryManager(outDirectory);
            var settings = ParseCommandLine(args);
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
                Console.WriteLine($"Press '{exitMessage}' to quit the sample.");
                if (Console.ReadLine() == exitMessage)
                    break;
            }
        }

        private static (Image logo, WatermarkSettings settings)? ParseCommandLine(string[] args)
        {
            var parser = new CommandLineParser<CommandLineOptions>();
            var parsingResult = parser.Parse(args);
            if (parsingResult.HelpRequested)
                return null;
            var result = parsingResult.Result;
            return (Image.FromFile(result.WatermarkPath), new WatermarkSettings
            {
                Opacity = result.Opacity,
                WatermarkRelativeSize = result.WatermarkRelativeSize,
                Position = result.WatermarkPosition,
                ShouldResizeWatermark = result.ShouldResizeWatermark
            });
        }
    }
}