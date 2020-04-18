using System;
using System.Linq;
using Common;

namespace Watcher
{
    public class Watcher
    {
        private readonly IMessageSender<BusMessage> _sender;
        private readonly IDirectoryWatcher _directoryWatcher;
        private readonly Action _waitForQuit;
        private readonly IFileManager _fileManager;

        public Watcher(IMessageSender<BusMessage> sender, Action waitForQuit, IDirectoryWatcher directoryWatcher, IFileManager fileManager)
        {
            _sender = sender ?? throw new ArgumentNullException(nameof(sender));
            _waitForQuit = waitForQuit ?? throw new ArgumentNullException(nameof(waitForQuit));
            _directoryWatcher = directoryWatcher ?? throw new ArgumentNullException(nameof(directoryWatcher));
            _fileManager = fileManager ?? throw new ArgumentNullException(nameof(fileManager));
            _directoryWatcher.NewFile += OnNewFile;
        }

        public void BeginWatch()
        {
            _directoryWatcher.BeginWatch();

            _waitForQuit();
        }

        private void OnNewFile(string fullPath)
        {
            if (!IsAllowed(fullPath)) 
                return;
            
            var content = _fileManager.Read(fullPath);
            if (content == null || content.Length == 0)
                return;
            var fileName = _fileManager.GetShortFilename(fullPath);
            
            _sender.Send(new BusMessage{
                Content = content, 
                FileName = fileName
            });
            _fileManager.Delete(fullPath);
        }

        private static bool IsAllowed(string fullPath)
        {
            var extensions = new string[] {".jpeg", ".png", ".bmp", ".jpg"};
            return extensions.Any(fullPath.EndsWith);
        }
    }
    
}