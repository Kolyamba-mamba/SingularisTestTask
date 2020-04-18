using System.IO;

namespace Watcher
{
    public class DirectoryManager: IDirectoryManager
    {
        private readonly DirectoryInfo _directoryInfo;

        public DirectoryManager(string directoryName)
        {
            _directoryInfo = new DirectoryInfo(directoryName);
            if (!_directoryInfo.Exists)
                _directoryInfo.Create();
        }
        
        public string GetFullFilePath(string filename)
        {
            return _directoryInfo.FullName + @"\" + filename;
        }
    }
}