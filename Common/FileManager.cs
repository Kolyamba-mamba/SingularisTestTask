using System;
using System.IO;
using System.Threading;

namespace Common
{
    public class FileManager : IFileManager
    {
        public byte[] Read(string path)
        {
            using var file = GetFile(path);
            if (file == null)
                return null;
            var arr = new byte[file.Length];
            file.Read(arr, 0, (int) file.Length);
            return arr;
        }

        public void Delete(string path)
        {
            var file = new FileInfo(path);
            file.Delete();
        }

        public void Write(string path, byte[] content)
        {
            using var file = File.OpenWrite(path);
            file.Write(content);
        }

        public string GetShortFilename(string fullPath)
        {
            var file = new FileInfo(fullPath);
            return file.Name;
        }
        
        /// <summary>
        /// Требуется для обработки ситуаций, когда файл занят другим процессом
        /// </summary>
        private static Stream GetFile(string path)
        {
            var fileInfo = new FileInfo(path);
            if (!fileInfo.Exists)
                return null;
            FileStream file = null;
            while (true)
            {
                try
                {
                    file = File.OpenRead(path);
                    return file;
                }
                catch (IOException)
                {
                    file?.Dispose();
                    Thread.Sleep(TimeSpan.FromMilliseconds(100));
                }
            }
        }
    }
}