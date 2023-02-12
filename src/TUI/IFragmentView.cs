using NStack;
using Terminal.Gui;

namespace DestinyArmorTool.TUI;

public interface IFragmentView
{
    public IListDataSource Source { get; set; }
    public ustring[] RadioLabels { get; set; }
    public event Action<SelectedItemChangedArgs> SubclassSelectionChanged;
    public event Action<ListViewItemEventArgs> FragmentMarked;
}