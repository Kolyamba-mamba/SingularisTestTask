using System;
using EasyNetQ;

namespace Converter
{
    public class MessageReceiver<TMessage>:  IDisposable, IMessageReceiver<TMessage> where TMessage : class
    {
        private readonly IBus _bus;
        public MessageReceiver(string hostName)
        {
            _bus = RabbitHutch.CreateBus("host =" + hostName);
            _bus.Subscribe<TMessage>("id", OnNewMessage);
        }

        public event IMessageReceiver<TMessage>.NewMessageEventHandler NewMessageReceived;
        
        private void OnNewMessage(TMessage message)
            => NewMessageReceived?.Invoke(message);

        public void Dispose() 
            => _bus?.Dispose();
    }
}