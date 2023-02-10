﻿using TestApp.Events;

namespace TestApp.TUI;

public interface IMainView
{
    public event Action<ListImportedEventArgs> ListImported;
    public event Action<DialogRequestedEventArgs> DialogRequested;
    event Action<ExportRequestedEventArgs> ExportRequested;
}