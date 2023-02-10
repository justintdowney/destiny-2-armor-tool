namespace TestApp.TUI;

public interface ISummaryView
{
    public bool SubviewsVisible { get; set; }
    void UpdateSelectedLabel(int selected);
    void UpdateSlotLabel(string slot, bool visible, bool exotic);
    void UpdateStatTotals(List<string> source);
}