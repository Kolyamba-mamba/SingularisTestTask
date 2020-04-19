using System;
using EasyNetQ;

namespace Watcher
{
    public class MessageSender<TMessage> : IDisposable, IMessageSender<TMessage> where TMessage : class
    {
        private readonly IBus _bus;

        public MessageSender(string hostName)
            => _bus = RabbitHutch.CreateBus("host =" + hostName);

        public void Send(TMessage value) 
            => _bus.Publish<TMessage>(value);

        public void Dispose() 
            => _bus.Dispose();
    }
}