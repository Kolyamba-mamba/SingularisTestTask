using Converter;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using Watcher;

namespace Tests
{
    internal class TestMessageReceiver : IMessageReceiver<BusMessage>
    {
        public int SubscriberCount { get; private set; }
        
        public event IMessageReceiver<BusMessage>.NewMessageEventHandler NewMessageReceived
        {
            add => SubscriberCount += 1;
            remove => SubscriberCount -= 1;
        }
    }    
    
    [TestFixture]
    public class ConverterShould
    {
        private IImageConverter _imageConverter;
        private IMessageReceiver<BusMessage> _messageReceiver;
        private IDirectoryManager _directoryManager;
        private IFileManager _fileManager;

        [SetUp]
        public void Setup()
        {
            _imageConverter = A.Fake<IImageConverter>();
            _messageReceiver = A.Fake<IMessageReceiver<BusMessage>>();
            _directoryManager = A.Fake<IDirectoryManager>();
            _fileManager = A.Fake<IFileManager>();
        }

        [Test]
        public void SubscribeToMessageReceiver_WhenCreated()
        {
            // Arrange 
            var testMessageReceiver = new TestMessageReceiver();
            // Act
            new Converter.Converter(testMessageReceiver, _imageConverter, _fileManager, _directoryManager);
            
            // Assert
            testMessageReceiver.SubscriberCount.Should().Be(1);
        }

        [Test]
        public void ConvertsReceivedImage()
        {
            // Arrange 
            var busMessage = new BusMessage {FileName = "test.png", Content = new byte[0]};
            var converter = new Converter.Converter(_messageReceiver, _imageConverter, _fileManager, _directoryManager);
            // Act
            _messageReceiver.NewMessageReceived += Raise.FreeForm.With(busMessage);
            // Assert
            A.CallTo(() => _imageConverter.Convert(busMessage.Content)).MustHaveHappened();
        }

        [Test]
        public void SaveConvertedImage()
        {
            // Arrange 
            var test = new byte[0];
            A.CallTo(() => _imageConverter.Convert(A<byte[]>._)).Returns(test);
            var busMessage = new BusMessage {FileName = "test.png", Content = new byte[0]};
            var converter = new Converter.Converter(_messageReceiver, _imageConverter, _fileManager, _directoryManager);
            //Act
            _messageReceiver.NewMessageReceived += Raise.FreeForm.With(busMessage);
            // Assert
            A.CallTo(() => _fileManager.Write(A<string>._, test)).MustHaveHappened();
        }

        [Test]
        public void SavesWithFullPath()
        {
            // Arrange 
            var test = "full image path";
            var busMessage = new BusMessage {FileName = "test.png", Content = new byte[0]};
            A.CallTo(() => _directoryManager.GetFullFilePath(busMessage.FileName)).Returns(test);
            var converter = new Converter.Converter(_messageReceiver, _imageConverter, _fileManager, _directoryManager);
            //Act
            _messageReceiver.NewMessageReceived += Raise.FreeForm.With(busMessage);
            // Assert
            A.CallTo(() => _fileManager.Write(test, A<byte[]>._)).MustHaveHappened();
        }
    }
}