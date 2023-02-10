namespace TestApp.Events
{
    public class DialogRequestedEventArgs : EventArgs
    {
        public int DialogId { get; set; }

        public DialogRequestedEventArgs(int id)
        {
            DialogId = id;
        }
    }
}
