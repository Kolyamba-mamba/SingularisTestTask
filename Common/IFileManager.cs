namespace Watcher
{
    public interface IFileManager
    {
        byte[] Read(string path);
        void Delete(string path);
        void Write(string path, byte[] content);
        string GetShortFilename(string fullPath);
    }
}