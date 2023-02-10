using System.ComponentModel;
using System.Globalization;
using CsvHelper;
using Terminal.Gui;
using TestApp.Data;
using TestApp.Events;
using TestApp.Model;
using TestApp.TUI;

namespace TestApp.Presenter;

public class ArmorPresenter : ISubscriber<ListImportedEventArgs>, ISubscriber<ExportRequestedEventArgs>
{
    private readonly ArmorModel _armorModel;
    private readonly IArmorView _armorView;
    private readonly IMessageAggregator _messageAggregator;

    public ArmorPresenter(IArmorView view)
    {
        _armorModel = new ArmorModel();
        _armorView = view;
        _armorModel.SelectedIndices = new List<int>();
        _armorView.SelectedItemChanged += SelectedArmorChanged;
        _armorView.MarkChanged += MarkStatusChanged;
        _messageAggregator = MessageAggregator.Instance;
        _messageAggregator.Subscribe<ListImportedEventArgs>(this);
        _messageAggregator.Subscribe<ExportRequestedEventArgs>(this);
    }

    public void HandleMessage(object? sender, ListImportedEventArgs message)
    {
        using var reader = new StreamReader(message.FilePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        var records = csv.GetRecords<Armor>();
        _armorModel.Armor = records.ToList();

        var dataSource = new SelectionDataSource<Armor>
        {
            List = _armorModel.Armor
        };
        _armorView.Source = dataSource;
        _messageAggregator.Publish(_armorView, new ListViewItemEventArgs(0, _armorModel.Armor[0]));
    }

    private void SelectedArmorChanged(ListViewItemEventArgs e)
    {
        _messageAggregator.Publish(null, e);
    }

    private void OnModChanging(object? sender, PropertyChangingEventArgs e)
    {
        if (sender == null) return;
        _messageAggregator.Publish(sender, e);
    }

    private void OnModChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender == null) return;
        _messageAggregator.Publish(sender, e);
    }

    private void MarkStatusChanged(ListViewItemEventArgs e)
    {
        var candidate = _armorModel.Armor[e.Item];
        var itemMarked = _armorView.Source.IsMarked(e.Item);

        if (itemMarked)
        {
            _armorView.Source.SetMark(e.Item, false);
            _armorModel.SelectedIndices.Remove(e.Item);
            ChangePropertyEvents(candidate, false);
            _messageAggregator.Publish(null, new SelectedCountChangedEventArgs(_armorModel.SelectedIndices.Count, e.Value));
            return;
        }

        if (ExoticSelected(candidate) || SlotSelected(candidate) || ClassDiffersFromSelected(candidate))
            return;

        _armorView.Source.SetMark(e.Item, true);
        _armorModel.SelectedIndices.Add(e.Item);
        ChangePropertyEvents(candidate, true);
        _messageAggregator.Publish(null, new SelectedCountChangedEventArgs(_armorModel.SelectedIndices.Count, e.Value));
    }

    private bool ExoticSelected(Armor armor) // Only 1 Exotic tier armor can be selected
    {
        return _armorModel.SelectedIndices.Any(x => _armorModel.Armor[x].Tier == "Exotic" && armor.Tier == "Exotic");
    }

    private bool SlotSelected(Armor armor)
    {
        return _armorModel.SelectedIndices.Any(x => _armorModel.Armor[x].Slot == armor.Slot);
    }

    private bool ClassDiffersFromSelected(Armor armor)
    {
        return _armorModel.SelectedIndices.Any(x => _armorModel.Armor[x].Class != armor.Class);
    }

    private void ChangePropertyEvents(Armor armor, bool subscribe)
    {
        if (subscribe)
        {
            armor.PropertyChanging += OnModChanging;
            armor.PropertyChanged += OnModChanged;
        }
        else
        {
            armor.PropertyChanging -= OnModChanging;
            armor.PropertyChanged -= OnModChanged;
        }
    }

    public void HandleMessage(object? sender, ExportRequestedEventArgs message)
    {
        if (!Clipboard.IsSupported)
            return;
        var text = _armorModel.SelectedIndices.ConvertAll(x => _armorModel.Armor[x].Id);
        Clipboard.TrySetClipboardData("id:" + String.Join(" or id:", text));
    }
}