namespace TestApp.Events;

public interface ISubscriberStore
{
    void Add<TMessage>(ISubscriber<TMessage> subscriber);
    IEnumerable<ISubscriber<TMessage>> Subscribers<TMessage>();
}