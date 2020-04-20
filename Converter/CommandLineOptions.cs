using MatthiWare.CommandLine.Core.Attributes;

namespace Converter
{
    public class CommandLineOptions
    {
        [Name("o", "opacity")]
        [Description("Прозрачность водяного знака.")]
        [DefaultValue(100)]
        public int Opacity { get; set; } = 100;
        
        [Name("s", "relative-size")]
        [Description("Размер водяного знака относительно изначальной картинки.")]
        [DefaultValue(50)]
        public int WatermarkRelativeSize { get; set; } = 50;
        
        [Name(null, "position")]
        [Description("Позиция водяного знака на картинке.")]
        [DefaultValue(WatermarkPosition.Center)]
        public WatermarkPosition WatermarkPosition { get; set; } = WatermarkPosition.Center;

        [Name(null, "path")]
        [Description("Путь к водяному знаку")]
        [DefaultValue("logo.png")]
        public string WatermarkPath { get; set; } = "logo.png";

        [Name("r", "resize")]
        [Description("Нужно ли менять размер водяного знака.")]
        [DefaultValue(true)]
        public bool ShouldResizeWatermark { get; set; } = true;
        
        [Name(null, "host")]
        [Description("Задание имени хоста(Должно совпадать у watcher и converter)")]
        [DefaultValue("localhost")]
        public string HostName { get; set; } = "localhost";
        
        [Name("f", "folder")]
        [Description("Место сохранения измененного файла")]
        [DefaultValue("outcoming")]
        public string FolderPath { get; set; } = "outcoming";
    }
}