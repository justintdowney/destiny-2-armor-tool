using DestinyArmorTool.Data;
using DestinyArmorTool.Events;
using DestinyArmorTool.TUI;
using NStack;
using Terminal.Gui;

namespace DestinyArmorTool.Presenter;

public class MainPresenter
{
    private readonly IMainView _mainView;
    private readonly IMessageAggregator _messageAggregator;

    public MainPresenter(IMainView view)
    {
        _mainView = view;
        _messageAggregator = MessageAggregator.Instance;
        _mainView.ListImported += OnListImport;
        _mainView.DialogRequested += HandleDialogRequest;
        _mainView.ExportRequested += HandleExportRequest;
    }

    private static void HandleDialogRequest(DialogRequestedEventArgs message)
    {
        var dialog = new Dialog
        {
            Height = Dim.Percent(30),
            TextAlignment = TextAlignment.Centered
        };
        var closeButton = new Button("Close", true);
        closeButton.Clicked += () => Application.RequestStop();
        var label = new Label
        {
            X = Pos.Center(),
            Y = Pos.Center(),
            Width = Dim.Percent(100),
            Height = Dim.Percent(100),
            TextAlignment = TextAlignment.Centered
        };
        ustring text;

        switch (message.DialogId)
        {
            case 0:
                text = Constants.HelpText;
                break;
            case 1:
                text = Constants.AboutText;
                break;
            default:
                throw new ArgumentOutOfRangeException(message.DialogId.ToString());
        }

        dialog.Text = text;
        dialog.Add(label);
        dialog.AddButton(closeButton);
        Application.Run(dialog);
    }

    private void HandleExportRequest(ExportRequestedEventArgs e)
    {
        _messageAggregator.Publish(null, e);
    }
    
    private void OnListImport(ListImportedEventArgs e)
    {
        _messageAggregator.Publish(null, e);
    }
}