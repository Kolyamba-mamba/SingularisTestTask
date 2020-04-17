using System;
using System.IO;

namespace Watcher
{
    public class DirectoryWatcher : IDirectoryWatcher, IDisposable
    {
        private readonly string _directoryToWatch;
        private readonly FileSystemWatcher _fileSystemWatcher;
        
        public DirectoryWatcher(string directoryToWatch)
        {
            _directoryToWatch = directoryToWatch ?? throw new ArgumentNullException(nameof(directoryToWatch));
            var dirInfo = new DirectoryInfo(_directoryToWatch);
            if (!dirInfo.Exists)
                dirInfo.Create();
            _fileSystemWatcher = new FileSystemWatcher
            {
                Path = dirInfo.FullName,
                NotifyFilter = NotifyFilters.LastAccess
                               | NotifyFilters.LastWrite
                               | NotifyFilters.FileName
                               | NotifyFilters.DirectoryName,
                Filter = "*.*"
            };
            _fileSystemWatcher.Created += OnChanged;
        }
        
        public event IDirectoryWatcher.NewFileEventHandler NewFile;

        public void BeginWatch() 
            => _fileSystemWatcher.EnableRaisingEvents = true;

        public void Dispose() 
            => _fileSystemWatcher.Dispose();

        private void OnChanged(object source, FileSystemEventArgs e) 
            => NewFile?.Invoke(e.FullPath);
    }
}