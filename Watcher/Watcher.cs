using System;
using System.IO;

namespace Watcher
{
    public class Watcher
    {
        private readonly IMessageSender _sender;
        private readonly IDirectoryWatcher _directoryWatcher;
        private readonly Action _waitForQuit;

        public Watcher(IMessageSender sender, Action waitForQuit, IDirectoryWatcher directoryWatcher)
        {
            _sender = sender ?? throw new ArgumentNullException(nameof(sender));
            _waitForQuit = waitForQuit ?? throw new ArgumentNullException(nameof(waitForQuit));
            _directoryWatcher = directoryWatcher ?? throw new ArgumentNullException(nameof(directoryWatcher));
            _directoryWatcher.AddNewFileHandler(OnNewFile);
        }

        public void BeginWatch()
        {
            _directoryWatcher.BeginWatch();

            _waitForQuit();
        }

        private void OnNewFile(string fullPath)
        {
            if (!fullPath.EndsWith(".jpg") && !fullPath.EndsWith(".png") && !fullPath.EndsWith(".bmp")) return;
            _sender.Send(fullPath);
        }
    }
    
}