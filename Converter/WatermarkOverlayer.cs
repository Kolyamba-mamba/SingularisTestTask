using System;
using System.IO;
using ImageProcessor;
using ImageProcessor.Imaging;
using System.Drawing;

namespace Converter
{
    public enum WatermarkPosition
    {
        Center,
        LeftTop,
        LeftBottom,
        RightTop,
        RightBottom
    }

    public struct WatermarkSettings
    {
        public int Opacity { get; set; }
        public int WatermarkRelativeSize { get; set; }
        public WatermarkPosition Position { get; set; }
    }
    
    public class WatermarkOverlayer : IImageConverter
    {
        private readonly Image _logo;
        private readonly WatermarkSettings _watermarkSettings;
        
        public WatermarkOverlayer(Image logo, WatermarkSettings watermarkSettings)
        {
            _logo = logo ?? throw new ArgumentNullException(nameof(logo));
            _watermarkSettings = watermarkSettings;
        }

        public byte[] Convert(byte[] imageBytes)
        {
            using var inputImageStream = new MemoryStream(imageBytes);
            var inputImage = Image.FromStream(inputImageStream);
            var resizedLogo = _logo.Resize(GetNewWatermarkSize(inputImage.Size));
            var logoPosition = _watermarkSettings.Position.ToActualPosition(inputImage.Size, resizedLogo.Size);
            var imageLayer = new ImageLayer{
                Image = resizedLogo, 
                Size = resizedLogo.Size, 
                Opacity = _watermarkSettings.Opacity, 
                Position = logoPosition
            };
            using var convertedImageStream = new MemoryStream();
            using var factory = new ImageFactory(preserveExifData:true);
            factory.Load(inputImage).Overlay(imageLayer).Save(convertedImageStream);
            return convertedImageStream.ToArray();
        }

        private Size GetNewWatermarkSize(Size inputImageSize)
        {
            var newWidth = inputImageSize.Width * (_watermarkSettings.WatermarkRelativeSize * 0.01);
            var newHeight = inputImageSize.Height * (_watermarkSettings.WatermarkRelativeSize * 0.01);
            return new Size((int)newWidth, (int)newHeight);
        }
    }

    internal static class Utils
    {
        public static Image Resize(this Image image, Size size)
        {
            using var factory = new ImageFactory(preserveExifData:true);
            using var resizedImageStream = new MemoryStream();
            var resizeLayer = new ResizeLayer(size: size, resizeMode : ResizeMode.Stretch);
            factory.Load(image).Resize(resizeLayer).Save(resizedImageStream);
            return Image.FromStream(resizedImageStream);
        }

        public static Point? ToActualPosition(this WatermarkPosition watermarkPosition, Size inputImageSize, Size watermarkSize) 
            => watermarkPosition switch
            {
                WatermarkPosition.Center => null,
                WatermarkPosition.LeftTop => new Point(0, 0),
                WatermarkPosition.LeftBottom => new Point(0, inputImageSize.Height - watermarkSize.Height),
                WatermarkPosition.RightTop => new Point(inputImageSize.Width - watermarkSize.Width, 0),
                WatermarkPosition.RightBottom => new Point(inputImageSize.Width - watermarkSize.Width,
                    inputImageSize.Height - watermarkSize.Height),
                _ => throw new ArgumentOutOfRangeException(nameof(watermarkPosition), watermarkPosition, null)
            };
    }
}