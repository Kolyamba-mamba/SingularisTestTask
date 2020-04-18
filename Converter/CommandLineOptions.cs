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
    }
}