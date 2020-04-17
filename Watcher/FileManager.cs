﻿using System.IO;

namespace Watcher
{
    public class FileManager : IFileManager
    {
        public byte[] Read(string path)
        {
            using var file = File.OpenRead(path);
            var arr = new byte[file.Length];
            file.Read(arr, 0, (int)file.Length);
            return arr;
        }

        public void Delete(string path)
        {
            var file = new FileInfo(path);
            file.Delete();
        }
    }
}