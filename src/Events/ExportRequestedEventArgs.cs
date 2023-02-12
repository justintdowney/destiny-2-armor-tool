namespace DestinyArmorTool.Events
{
    public class ExportRequestedEventArgs : EventArgs
    {
        public int ExportId { get; set; }

        public ExportRequestedEventArgs(int id)
        {
            ExportId = id;
        }
    }
}
