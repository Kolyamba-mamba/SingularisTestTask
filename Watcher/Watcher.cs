﻿using System;
using System.Linq;
using Common;

namespace Watcher
{
    public class Watcher
    {
        private readonly IMessageSender<BusMessage> _sender;
        private readonly IDirectoryWatcher _directoryWatcher;
        private readonly IFileManager _fileManager;
        private readonly bool _shouldDelete;

        public Watcher(IMessageSender<BusMessage> sender, IDirectoryWatcher directoryWatcher, IFileManager fileManager, bool? isDelete)
        {
            _sender = sender ?? throw new ArgumentNullException(nameof(sender));
            _directoryWatcher = directoryWatcher ?? throw new ArgumentNullException(nameof(directoryWatcher));
            _fileManager = fileManager ?? throw new ArgumentNullException(nameof(fileManager));
            if (isDelete != null) _shouldDelete = (bool) isDelete;
            _directoryWatcher.NewFile += OnNewFile;
            _directoryWatcher.BeginWatch();
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
            if (_shouldDelete) _fileManager.Delete(fullPath);
        }

        private static bool IsAllowed(string fullPath)
        {
            var extensions = new string[] {".jpeg", ".png", ".bmp", ".jpg"};
            return extensions.Any(fullPath.EndsWith);
        }
    }
    
}