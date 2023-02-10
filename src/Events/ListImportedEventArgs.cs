namespace TestApp.Events;

public class ListImportedEventArgs : EventArgs
{
    public ListImportedEventArgs(string filePath)
    {
        FilePath = filePath;
    }

    public string FilePath { get; }
}