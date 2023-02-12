namespace DestinyArmorTool.Events;

public interface IMessageAggregator
{
    public static IMessageAggregator Instance { get; }
    void Subscribe<TMessage>(ISubscriber<TMessage> subscriber);
    void Publish<TMessage>(object? sender, TMessage message);
}