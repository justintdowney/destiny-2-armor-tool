using NStack;
using Terminal.Gui;
using TestApp.Data;

namespace TestApp.TUI;

public interface IDetailedView
{
    int SelectedModIndex { get; set; }
    void DisplayItem(Armor armor);
    void SetModLabels(ustring[] labels);
    event Action<SelectedItemChangedArgs> ModSelectionChanged;
}