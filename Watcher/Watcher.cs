using System;
using System.IO;

namespace Watcher
{
    public class Watcher
    {
        private readonly IMessageSender<byte[]> _sender;
        private readonly IDirectoryWatcher _directoryWatcher;
        private readonly Action _waitForQuit;

        public Watcher(IMessageSender<byte[]> sender, Action waitForQuit, IDirectoryWatcher directoryWatcher, IFileManager fileManager)
        {
            _sender = sender ?? throw new ArgumentNullException(nameof(sender));
            _waitForQuit = waitForQuit ?? throw new ArgumentNullException(nameof(waitForQuit));
            _directoryWatcher = directoryWatcher ?? throw new ArgumentNullException(nameof(directoryWatcher));
            _directoryWatcher.NewFile += OnNewFile;
        }

        public void BeginWatch()
        {
            _directoryWatcher.BeginWatch();

            _waitForQuit();
        }

        private void OnNewFile(string fullPath)
        {
            if (!fullPath.EndsWith(".jpeg") && !fullPath.EndsWith(".png") 
                                           && !fullPath.EndsWith(".bmp") 
                                           && !fullPath.EndsWith(".jpg")) return;
            _sender.Send(fullPath);
        }
    }
    
}