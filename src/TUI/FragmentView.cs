using NStack;
using Terminal.Gui;

namespace TestApp.TUI;

public sealed class FragmentView : FrameView, IFragmentView
{
    private readonly ListView _listView;
    private readonly RadioGroup _radioGroup;

    public FragmentView()
    {
        Title = "Fragment Information";

        _radioGroup = new RadioGroup
        {
            X = 0,
            Y = 0,
            DisplayMode = DisplayModeLayout.Horizontal,
            Width = Dim.Fill(),
            Height = Dim.Percent(10)
        };

        _listView = new ListView
        {
            X = 0,
            Y = Pos.Bottom(_radioGroup),
            Width = Dim.Fill(),
            Height = Dim.Percent(90),
            Visible = false
        };

        _listView.MouseClick += e =>
        {
            if (e.MouseEvent.Flags != MouseFlags.Button1DoubleClicked || e.MouseEvent.View != _listView)
                return;
            _listView.SelectedItem = _listView.TopItem + e.MouseEvent.Y;
            _listView.Source.SetMark(_listView.SelectedItem, !_listView.Source.IsMarked(_listView.SelectedItem));
            _listView.SetNeedsDisplay();
        };

        _radioGroup.ClearKeybindings();
        _listView.ClearKeybindings();

        Add(_radioGroup, _listView);
    }

    public IListDataSource Source
    {
        get => _listView.Source;

        set
        {
            _listView.Source = value;
            _listView.Visible = true;
            _listView.SetNeedsDisplay();
        }
    }

    public ustring[] RadioLabels
    {
        get => _radioGroup.RadioLabels;
        set
        {
            _radioGroup.RadioLabels = value;
            _radioGroup.SelectedItem = -1;
        }
    }

    public event Action<ListViewItemEventArgs> FragmentMarked
    {
        add => _listView.OpenSelectedItem += value;
        remove => _listView.OpenSelectedItem -= value;
    }

    public event Action<SelectedItemChangedArgs> SubclassSelectionChanged
    {
        add => _radioGroup.SelectedItemChanged += value;
        remove => _radioGroup.SelectedItemChanged -= value;
    }
}