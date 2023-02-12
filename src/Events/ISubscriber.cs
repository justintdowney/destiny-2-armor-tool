namespace DestinyArmorTool.Events;

public interface ISubscriber<in TMessage>
{
    void HandleMessage(object? sender, TMessage message);
}