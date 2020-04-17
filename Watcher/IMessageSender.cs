namespace Watcher
{
    public interface IMessageSender<TMessage> where TMessage : class
    {
        void Send(TMessage value);
    }
}