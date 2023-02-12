namespace DestinyArmorTool.Events;

internal class MessageAggregator : IMessageAggregator
{
    private static IMessageAggregator? _instance;
    private readonly ISubscriberStore _subscriberStore;

    private MessageAggregator()
    {
        _subscriberStore = new SubscriberStore();
    }

    public static IMessageAggregator Instance
    {
        get
        {
            _instance ??= new MessageAggregator();
            return _instance;
        }
    }

    public void Publish<TMessage>(object? sender, TMessage message)
    {
        foreach (var subscriber in _subscriberStore.Subscribers<TMessage>()) subscriber.HandleMessage(sender, message);
    }

    public void Subscribe<TMessage>(ISubscriber<TMessage> subscriber)
    {
        _subscriberStore.Add(subscriber);
    }
}