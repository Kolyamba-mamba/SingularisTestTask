using FakeItEasy;
using NUnit.Framework;
using Watcher;
using FluentAssertions;

namespace Tests
{
    internal class TestDirectoryWatcher : IDirectoryWatcher
    {
        public int SubscriberCount { get; private set; }
        public event IDirectoryWatcher.NewFileEventHandler NewFile
        {
            add => SubscriberCount += 1;
            remove => SubscriberCount -= 1;
        }
        public void BeginWatch()
        {
            
        }
    }    
    
    public class WatcherShould
    {
        private IDirectoryWatcher _directoryWatcher;
        private IMessageSender<byte[]> _messageSender;
        private IFileManager _fileManager;
        [SetUp]
        public void Setup()
        {
            _directoryWatcher = A.Fake<IDirectoryWatcher>();
            _messageSender = A.Fake<IMessageSender<byte[]>>();
            _fileManager = A.Fake<IFileManager>();
        }

        [Test]
        public void SubscribeToDirectory_WhenCreated()
        {
            // Arrange 
            var testDirectoryWatcher = new TestDirectoryWatcher();
            // Act
            new Watcher.Watcher(_messageSender, () => { }, testDirectoryWatcher, _fileManager);
            
            // Assert
            testDirectoryWatcher.SubscriberCount.Should().Be(1);
        }

        [Test]
        public void SendFiles_WhenAllowed()
        {
            // Arrange 
            var testFileNames = new [] { @"C:\Users\Nikolay\Downloads\first.png", 
                @"C:\Users\Nikolay\Downloads\second.jpeg",
                @"C:\Users\Nikolay\Downloads\third.bmp",
                @"C:\Users\Nikolay\Downloads\fourth.jpg"
            };
            var watcher = new Watcher.Watcher(_messageSender, () => { }, _directoryWatcher, _fileManager);
            
            // Act
            foreach (var fileName in testFileNames)
            {
                _directoryWatcher.NewFile += Raise.FreeForm.With(fileName);
            }

            // Assert
            A.CallTo(() => _messageSender.Send(A<byte[]>._)).MustHaveHappened(testFileNames.Length, Times.Exactly);
        }

        [Test]
        public void NotSendFiles_WhenDisallowed()
        {
            // Arrange 
            var testFileNames = new [] { @"C:\Users\Nikolay\Downloads\first.txt", 
                @"C:\Users\Nikolay\Downloads\second.doc",
                @"C:\Users\Nikolay\Downloads\third.html",
                @"C:\Users\Nikolay\Downloads\fourth.exe"
            };
            var watcher = new Watcher.Watcher(_messageSender, () => { }, _directoryWatcher, _fileManager);
            
            // Act
            foreach (var fileName in testFileNames)
            {
                _directoryWatcher.NewFile += Raise.FreeForm.With(fileName);
            }

            // Assert
            A.CallTo(() => _messageSender.Send(A<byte[]>._)).MustHaveHappened(0, Times.Exactly);
        }

        }
    }
}