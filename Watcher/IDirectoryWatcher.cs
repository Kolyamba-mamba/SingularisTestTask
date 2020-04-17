namespace Watcher
{
    public interface IDirectoryWatcher
    {
        public delegate void NewFileDelegate(string fullPath);
        void AddNewFileHandler(NewFileDelegate newFileDelegate);
        void BeginWatch();
    }
}