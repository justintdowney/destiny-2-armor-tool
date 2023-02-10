using System.Globalization;
using CsvHelper;
using NStack;
using Terminal.Gui;
using TestApp.Data;
using TestApp.Events;
using TestApp.Model;
using TestApp.TUI;

namespace TestApp.Presenter;

public class DetailedPresenter : ISubscriber<ListViewItemEventArgs>
{
    private readonly IDetailedView _detailedView;
    private readonly IMessageAggregator _messageAggregator;
    private readonly ModModel _modModel;

    public DetailedPresenter(IDetailedView detailedView)
    {
        _modModel = new ModModel();
        LoadModList();
        _detailedView = detailedView;
        _messageAggregator = MessageAggregator.Instance;
        _messageAggregator.Subscribe(this);
        var modNames = new ustring[_modModel.Mods.Count];
        for (var i = 0; i < _modModel.Mods.Count; i++) modNames[i] = ustring.Make(_modModel.Mods[i].Name);
        _detailedView.SetModLabels(modNames);
        _detailedView.ModSelectionChanged += UpdateModData;
    }

    public void HandleMessage(object? sender, ListViewItemEventArgs message)
    {
        if (message.Value.GetType() != typeof(Armor))
            return;

        var armor = (Armor)message.Value;
        _modModel.DisplayedArmor = armor;
        _detailedView.DisplayItem(_modModel.DisplayedArmor);
        _detailedView.SelectedModIndex = _modModel.Mods.IndexOf(_modModel.DisplayedArmor.Mod);
    }

    private void LoadModList()
    {
        var list = new List<Mod>
        {
            new()
        };
        using var reader = new StreamReader(Constants.ModPath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        var records = csv.GetRecords<Mod>();
        list.AddRange(records.ToList());
        _modModel.Mods = list;
    }

    private void UpdateModData(SelectedItemChangedArgs e)
    {
        _modModel.DisplayedArmor.Mod = _modModel.Mods[e.SelectedItem];
        _detailedView.DisplayItem(_modModel.DisplayedArmor);
    }
}