using System;
using Common;

namespace Converter
{
    public class Converter
    {
        private readonly IImageConverter _imageConverter;
        private readonly IMessageReceiver<BusMessage> _messageReceiver;
        private readonly IFileManager _fileManager;
        private readonly IDirectoryManager _directoryManager;

        public Converter(IMessageReceiver<BusMessage> messageReceiver, IImageConverter imageConverter,
            IFileManager fileManager, IDirectoryManager directoryManager)
        {
            _messageReceiver = messageReceiver ?? throw new ArgumentNullException(nameof(messageReceiver));
            _imageConverter = imageConverter ?? throw new ArgumentNullException(nameof(imageConverter));
            _fileManager = fileManager ?? throw new ArgumentNullException(nameof(fileManager));
            _directoryManager = directoryManager ?? throw new ArgumentNullException(nameof(directoryManager));
            _messageReceiver.NewMessageReceived += OnNewMessage;
        }

        private void OnNewMessage(BusMessage image)
        {
            var convertedImage = _imageConverter.Convert(image.Content);
            _fileManager.Write(_directoryManager.GetFullFilePath(image.FileName), convertedImage);
        }
    }
}