namespace TestApp.Events;

public class SubscriberStore : ISubscriberStore
{
    private readonly Dictionary<Type, List<object>> _subscribers;

    public SubscriberStore()
    {
        _subscribers = new Dictionary<Type, List<object>>();
    }

    public void Add<TMessage>(ISubscriber<TMessage> subscriber)
    {
        if (!_subscribers.ContainsKey(typeof(TMessage)))
            _subscribers.Add(typeof(TMessage), new List<object>());
        _subscribers[typeof(TMessage)].Add(subscriber);
    }

    public IEnumerable<ISubscriber<TMessage>> Subscribers<TMessage>()
    {
        _subscribers.TryGetValue(typeof(TMessage), out var result);

        if (result == null)
            throw new NullReferenceException();

        foreach (var subscriber in result)
            if (subscriber == null)
                result.Remove(subscriber);

        return result.Cast<ISubscriber<TMessage>>().ToList();
    }
}