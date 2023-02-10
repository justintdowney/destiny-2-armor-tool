using Terminal.Gui;
using TestApp.Data;

namespace TestApp.TUI;

public sealed class SummaryView : FrameView, ISummaryView
{
    private readonly ListView _listView;
    private readonly Label _selectedLabel;
    private readonly List<Label> _slotLabels;

    public SummaryView()
    {
        Title = "Summary Information";
        _selectedLabel = new Label($"Selected: 0/{Constants.MaxSlots}")
        {
            X = 0,
            Y = 0,
            Width = Dim.Percent(10),
            Height = Dim.Percent(10)
        };
        _slotLabels = new List<Label>
        {
            new(Constants.SlotHelmet),
            new(Constants.SlotGauntlets),
            new(Constants.SlotChest),
            new(Constants.SlotLeg),
            new(Constants.SlotClass)
        };
        for (var i = 0; i < _slotLabels.Count; i++)
        {
            _slotLabels[i].X = Pos.AnchorEnd(_slotLabels[i].Text.Length);
            _slotLabels[i].Y = i;
            _slotLabels[i].Visible = false;
        }

        var source = new string[Constants.Stats.Length];
        for (var i = 0; i < source.Length; i++) source[i] = Constants.Stats[i] + ":";

        _listView = new ListView(source)
        {
            X = 0,
            Y = 1, // Padded for _selectedLabel
            Width = Dim.Percent(70),
            Height = Dim.Percent(90),
            AllowsMarking = false,
            AllowsMultipleSelection = false
        };

        Add(_selectedLabel, _listView);
        Add(_slotLabels.ToArray());
    }

    public bool SubviewsVisible
    {
        get => _listView.Visible && _selectedLabel.Visible;
        set
        {
            _listView.Visible = value;
            _selectedLabel.Visible = value;
        }
    }

    public void UpdateSelectedLabel(int selectedCount)
    {
        _selectedLabel.Text = $"Selected: {selectedCount}/{Constants.MaxSlots}";
        _selectedLabel.SetNeedsDisplay();
    }

    public void UpdateSlotLabel(string slot, bool visible, bool exotic)
    {
        var slotIndex = Armor.GetSlotIndex(slot);
        _slotLabels[slotIndex].Visible = visible;
        _slotLabels[slotIndex].ColorScheme = exotic ? Colors.ColorSchemes["Exotic"] : Colors.ColorSchemes["Base"];
    }

    public void UpdateStatTotals(List<string> source)
    {
        _listView.SetSource(source);
        _listView.SetNeedsDisplay();
    }
}