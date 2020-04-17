namespace Watcher
{
    public interface IDirectoryWatcher
    {
        public delegate void NewFileEventHandler(string fullPath);
        
        event NewFileEventHandler NewFile;
       
        void BeginWatch();
    }
}