using Terminal.Gui;
using Attribute = Terminal.Gui.Attribute;

namespace TestApp.TUI;

public sealed class ArmorView : FrameView, IArmorView
{
    private readonly ListView _listView;
    private ScrollBarView _scrollBar;

    public ArmorView()
    {
        Title = "Armor List";

        _listView = new ListView
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill(),
            AllowsMarking = false,
            AllowsMultipleSelection = false
        };
        Add(_listView);

        _listView.MouseClick += e =>
        {
            if (e.MouseEvent.Flags != MouseFlags.Button1Clicked)
                return;

            _listView.SelectedItem = _listView.TopItem + e.MouseEvent.Y;
            e.Handled = true;
            SetNeedsDisplay();
        };

        _listView.RowRender += RenderRowSelector;
    }

    public IListDataSource Source
    {
        get => _listView.Source;

        set
        {
            _listView.Source = value;
            InitializeScrollBar();
            _listView.SetNeedsDisplay();
        }
    }

    public int SelectedItem
    {
        get => _listView.SelectedItem;
        set => _listView.SelectedItem = value;
    }

    public event Action<ListViewItemEventArgs> MarkChanged
    {
        add => _listView.OpenSelectedItem += value;
        remove => _listView.OpenSelectedItem -= value;
    }

    public event Action<ListViewItemEventArgs> SelectedItemChanged
    {
        add => _listView.SelectedItemChanged += value;
        remove => _listView.SelectedItemChanged -= value;
    }

    private void InitializeScrollBar()
    {
        _scrollBar = new ScrollBarView(_listView, true);
        _scrollBar.ChangedPosition += () =>
        {
            _listView.TopItem = _scrollBar.Position;
            if (_listView.TopItem != _scrollBar.Position) _scrollBar.Position = _listView.TopItem;

            _listView.SetNeedsDisplay();
        };
        BringSubviewToFront(_scrollBar);

        _listView.DrawContent += e =>
        {
            _scrollBar.Size = _listView.Source.Count - 1;
            _scrollBar.Position = _listView.TopItem;
            _scrollBar.Refresh();
        };
    }

    private void RenderRowSelector(ListViewRowEventArgs obj)
    {
        if (obj.Row == _listView.SelectedItem)
            obj.RowAttribute = new Attribute(Color.Black, Color.White);
    }
}