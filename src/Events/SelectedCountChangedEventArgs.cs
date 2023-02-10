namespace TestApp.Events;

public class SelectedCountChangedEventArgs : EventArgs
{
    public SelectedCountChangedEventArgs(int selectedCount, object item)
    {
        SelectedCount = selectedCount;
        Item = item;
    }

    public int SelectedCount { get; set; }
    public object Item { get; set; }
}