using MatthiWare.CommandLine.Core.Attributes;

namespace Watcher
{
    public class CommandLineOptions
    {
        [Name(null, "host")]
        [Description("Задание имени хоста(Должно совпадать у watcher и converter)")]
        [DefaultValue("localhost")]
        public string HostName { get; set; } = "localhost";
        
        [Name("f", "folder")]
        [Description("Путь к изменяемому файлу")]
        [DefaultValue("incoming")]
        public string FolderPath { get; set; } = "incoming";
        
        [Name("d", "delete")]
        [Description("Нужно ли удалять файл после отправки")]
        [DefaultValue(true)]
        public bool ShouldDelete { get; set; } = true;
    }
}