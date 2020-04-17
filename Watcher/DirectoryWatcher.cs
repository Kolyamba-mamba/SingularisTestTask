using System;
using System.IO;

namespace Watcher
{
    public class DirectoryWatcher : IDirectoryWatcher, IDisposable
    {
        private readonly string _directoryToWatch;
        private readonly FileSystemWatcher _fileSystemWatcher;
        
        private IDirectoryWatcher.NewFileDelegate _newFileDelegates;
        
        public DirectoryWatcher(string directoryToWatch)
        {
            _directoryToWatch = directoryToWatch ?? throw new ArgumentNullException(nameof(directoryToWatch));
            _fileSystemWatcher = new FileSystemWatcher
            {
                Path = directoryToWatch,
                NotifyFilter = NotifyFilters.LastAccess
                               | NotifyFilters.LastWrite
                               | NotifyFilters.FileName
                               | NotifyFilters.DirectoryName,
                Filter = "*.*"
            };
        }
        

        public void AddNewFileHandler(IDirectoryWatcher.NewFileDelegate newFileDelegate)
            => _newFileDelegates += newFileDelegate;
        
        public void BeginWatch() 
            => _fileSystemWatcher.EnableRaisingEvents = true;

        public void Dispose() 
            => _fileSystemWatcher.Dispose();

        private void OnChanged(object source, FileSystemEventArgs e) 
            => _newFileDelegates(e.FullPath);
    }
}