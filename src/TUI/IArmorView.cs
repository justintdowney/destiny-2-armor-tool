using Terminal.Gui;

namespace DestinyArmorTool.TUI;

public interface IArmorView
{
    public IListDataSource Source { get; set; }
    int SelectedItem { get; set; }
    public event Action<ListViewItemEventArgs> SelectedItemChanged;
    public event Action<ListViewItemEventArgs> MarkChanged;

    //public void SetComputedLayout(Dim, Dim, Pos, Pos)
}