namespace Watcher
{
    public interface IFileManager
    {
        byte[] Read(string path);
        void Delete(string path);
    }
}