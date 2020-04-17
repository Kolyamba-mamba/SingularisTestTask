using System;
using EasyNetQ;

namespace Watcher
{
    public class MessageSender: IMessageSender, IDisposable
    {
        private readonly IBus _bus;

        public MessageSender()
            => _bus = RabbitHutch.CreateBus("host=localhost");

        public void Send(string filename) 
            => _bus.Publish(filename);

        public void Dispose() 
            => _bus.Dispose();
    }
}