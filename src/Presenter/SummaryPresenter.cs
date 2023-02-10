using System.ComponentModel;
using TestApp.Data;
using TestApp.Events;
using TestApp.Model;
using TestApp.TUI;

namespace TestApp.Presenter;

public class SummaryPresenter : ISubscriber<ListImportedEventArgs>, ISubscriber<SelectedCountChangedEventArgs>,
    ISubscriber<PropertyChangedEventArgs>, ISubscriber<PropertyChangingEventArgs>
{
    private readonly IMessageAggregator _messageAggregator;
    private readonly SummaryModel _summaryModel;
    private readonly ISummaryView _summaryView;

    public SummaryPresenter(ISummaryView summaryView)
    {
        _summaryModel = new SummaryModel
        {
            StatTotals = new List<int> { 0, 0, 0, 0, 0, 0 }
        };

        _summaryView = summaryView;
        _messageAggregator = MessageAggregator.Instance;
        _messageAggregator.Subscribe<ListImportedEventArgs>(this);
        _messageAggregator.Subscribe<SelectedCountChangedEventArgs>(this);
        _messageAggregator.Subscribe<PropertyChangingEventArgs>(this);
        _messageAggregator.Subscribe<PropertyChangedEventArgs>(this);
    }

    public void HandleMessage(object? sender, ListImportedEventArgs message)
    {
        _summaryView.SubviewsVisible = true;
    }

    // Mod is changing on selectedarmor piece, subtract mod from stat total
    public void HandleMessage(object? sender, PropertyChangingEventArgs message)
    {
        var current = sender as Armor;
        if (current == null) return;
        var statList = new List<string>();
        for (var i = 0; i < _summaryModel.StatTotals.Count; i++)
        {
            _summaryModel.StatTotals[i] -= current.Mod[i];
            statList.Add($"{Constants.Stats[i]}: {_summaryModel.StatTotals[i]}");
        }

        _summaryView.UpdateStatTotals(statList);
    }

    // Mod changed on selectedarmor piece, add mod to stat total
    public void HandleMessage(object? sender, PropertyChangedEventArgs message)
    {
        var current = sender as Armor;
        if (current == null) return;
        var statList = new List<string>();
        for (var i = 0; i < _summaryModel.StatTotals.Count; i++)
        {
            _summaryModel.StatTotals[i] += current.Mod[i];
            statList.Add($"{Constants.Stats[i]}: {_summaryModel.StatTotals[i]}");
        }

        _summaryView.UpdateStatTotals(statList);
    }

    public void HandleMessage(object? sender, SelectedCountChangedEventArgs message)
    {
        var type = message.Item.GetType();
        if (type == typeof(Armor))
            HandleArmorMarkMessage(message);
        else if (type == typeof(Fragment))
            HandleFragmentMarkMessage(message);
    }

    private void HandleFragmentMarkMessage(SelectedCountChangedEventArgs message)
    {
        var fragment = (Fragment)message.Item;
        var statList = new List<string>();
        for (var i = 0; i < _summaryModel.StatTotals.Count; i++)
        {
            _summaryModel.StatTotals[i] +=
                message.SelectedCount > _summaryModel.FragmentsSelected || _summaryModel.FragmentsSelected == 0
                    ? fragment[i]
                    : -fragment[i];
            statList.Add($"{Constants.Stats[i]}: {_summaryModel.StatTotals[i]}");
        }

        _summaryModel.FragmentsSelected = message.SelectedCount;
        _summaryView.UpdateStatTotals(statList);
    }

    private void HandleArmorMarkMessage(SelectedCountChangedEventArgs message)
    {
        var armor = (Armor)message.Item;
        var statList = new List<string>();
        for (var i = 0; i < Constants.Stats.Length; i++)
        {
            _summaryModel.StatTotals[i] += message.SelectedCount > _summaryModel.ArmorSelected
                ? armor[i]
                : -armor[i];
            statList.Add($"{Constants.Stats[i]}: {_summaryModel.StatTotals[i]}");
        }

        _summaryView.UpdateSlotLabel(armor.Slot,
            message.SelectedCount > _summaryModel.ArmorSelected, armor.Tier == "Exotic");

        _summaryModel.ArmorSelected = message.SelectedCount;
        _summaryView.UpdateSelectedLabel(_summaryModel.ArmorSelected);
        _summaryView.UpdateStatTotals(statList);
    }
}