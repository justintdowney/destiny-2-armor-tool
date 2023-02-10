using NStack;
using Terminal.Gui;
using TestApp.Data;

namespace TestApp.TUI;

public sealed class DetailedView : FrameView, IDetailedView
{
    private readonly ListView _listView;
    private readonly RadioGroup _radioGroup;

    public DetailedView()
    {
        Title = "Detailed Armor Information";
        _listView = new ListView
        {
            X = 0,
            Y = 0,
            Width = Dim.Percent(60),
            Height = Dim.Fill(),
            AllowsMarking = false,
            AllowsMultipleSelection = false
        };

        _radioGroup = new RadioGroup
        {
            Visible = false,
            X = Pos.Right(_listView),
            Y = 0,
            Width = Dim.Percent(40),
            Height = Dim.Fill(),
            DisplayMode = DisplayModeLayout.Vertical,
            SelectedItem = 0
        };

        Add(_radioGroup, _listView);
    }

    public int SelectedModIndex
    {
        get => _radioGroup.SelectedItem;
        set => _radioGroup.SelectedItem = value == -1 ? 0 : value;
    }

    public void SetModLabels(ustring[] labels)
    {
        _radioGroup.RadioLabels = labels;
    }

    public void DisplayItem(Armor armor)
    {
        var properties = armor.GetType().GetProperties();
        var result = new List<string>();
        for (var i = 0; i < properties.Length; i++)
            result.Add($"{properties[i].Name}: {armor[properties[i].Name]}");
        _listView.SetSource(result);
        _listView.SetNeedsDisplay();
        if (!_radioGroup.Visible)
            _radioGroup.Visible = true;
    }

    public event Action<SelectedItemChangedArgs> ModSelectionChanged
    {
        add => _radioGroup.SelectedItemChanged += value;
        remove => _radioGroup.SelectedItemChanged -= value;
    }
}