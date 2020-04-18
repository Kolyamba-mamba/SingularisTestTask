namespace Converter
{
    public interface IMessageReceiver<TMessage>
    {
        public delegate void NewMessageEventHandler(TMessage message);
        event NewMessageEventHandler NewMessageReceived;
    }
}