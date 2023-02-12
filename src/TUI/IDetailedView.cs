using DestinyArmorTool.Data;
using NStack;
using Terminal.Gui;

namespace DestinyArmorTool.TUI;

public interface IDetailedView
{
    int SelectedModIndex { get; set; }
    void DisplayItem(Armor armor);
    void SetModLabels(ustring[] labels);
    event Action<SelectedItemChangedArgs> ModSelectionChanged;
}