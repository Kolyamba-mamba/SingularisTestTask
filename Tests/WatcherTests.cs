using System;
using System.Linq;
using Common;
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
        private IMessageSender<BusMessage> _messageSender;
        private IFileManager _fileManager;
        private bool _shouldDelete;
        [SetUp]
        public void Setup()
        {
            _directoryWatcher = A.Fake<IDirectoryWatcher>();
            _messageSender = A.Fake<IMessageSender<BusMessage>>();
            _fileManager = A.Fake<IFileManager>();
            _shouldDelete = true;
        }

        [Test]
        public void SubscribeToDirectory_WhenCreated()
        {
            // Arrange 
            var testDirectoryWatcher = new TestDirectoryWatcher();
            // Act
            new Watcher.Watcher(_messageSender, testDirectoryWatcher, _fileManager, _shouldDelete);
            
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
            var watcher = new Watcher.Watcher(_messageSender, _directoryWatcher, _fileManager, _shouldDelete);
            
            // Act
            foreach (var fileName in testFileNames)
            {
                _directoryWatcher.NewFile += Raise.FreeForm.With(fileName);
            }

            // Assert
            A.CallTo(() => _messageSender.Send(A<BusMessage>._)).MustHaveHappened(testFileNames.Length, Times.Exactly);
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
            var watcher = new Watcher.Watcher(_messageSender, _directoryWatcher, _fileManager, _shouldDelete);
            
            // Act
            foreach (var fileName in testFileNames)
            {
                _directoryWatcher.NewFile += Raise.FreeForm.With(fileName);
            }

            // Assert
            A.CallTo(() => _messageSender.Send(A<BusMessage>._)).MustHaveHappened(0, Times.Exactly);
        }

        [Test]
        public void ReadFile_WhenCreated()
        {
            // Arrange
            A.CallTo(() => _fileManager.Read(A<string>._)).Returns(new byte[0]);
            const string filePath = @"C:\Users\Nikolay\Downloads\third.bmp";
            var watcher = new Watcher.Watcher(_messageSender, _directoryWatcher, _fileManager, _shouldDelete);
            // Act
            _directoryWatcher.NewFile += Raise.FreeForm.With(filePath);
            // Assert
            A.CallTo(() => _fileManager.Read(filePath)).MustHaveHappened();
        }

        [Test]
        public void DeleteFile_AfterRead()
        {
            // Arrange
            A.CallTo(() => _fileManager.Read(A<string>._)).Returns(new byte[1]);
            const string filePath = @"C:\Users\Nikolay\Downloads\third.bmp";
            var watcher = new Watcher.Watcher(_messageSender, _directoryWatcher, _fileManager, _shouldDelete);
            var expectedCalls = new[] { "Read", "GetShortFilename", "Delete" };
            // Act
            _directoryWatcher.NewFile += Raise.FreeForm.With(filePath);
            // Assert
            var calls = Fake.GetCalls(_fileManager).ToList();
            calls.Count.Should().Be(expectedCalls.Length);
            calls.Select(call => call.Method.Name).Should().BeEquivalentTo(expectedCalls);
            A.CallTo(() => _fileManager.Delete(filePath)).MustHaveHappened();
        }
    }
}