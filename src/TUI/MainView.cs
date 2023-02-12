using DestinyArmorTool.Events;
using Terminal.Gui;
using Attribute = Terminal.Gui.Attribute;

namespace DestinyArmorTool.TUI;

public sealed class MainView : Toplevel, IMainView
{

    private MenuBar _menuBar;

    public MainView(View armorView, View detailedView, View fragmentView, View summaryView)
    {
        ColorScheme baseColorScheme = new()
        {
            Focus = new Attribute(Color.White, Color.Black),
            Normal = new Attribute(Color.White, Color.Black),
            Disabled = new Attribute(Color.White, Color.Black),
            HotFocus = new Attribute(Color.White, Color.Black),
            HotNormal = new Attribute(Color.White, Color.Black)
        };

        ColorScheme menuColorScheme = new()
        {
            Focus = new Attribute(Color.Black, Color.White),
            Normal = new Attribute(Color.Black, Color.White),
            Disabled = new Attribute(Color.Black, Color.White),
            HotFocus = new Attribute(Color.Black, Color.White),
            HotNormal = new Attribute(Color.Black, Color.White)
        };

        ColorScheme exoticColorScheme = new()
        {
            Focus = new Attribute(Color.Black, Color.BrightYellow),
            Normal = new Attribute(Color.Black, Color.BrightYellow),
            Disabled = new Attribute(Color.Black, Color.BrightYellow),
            HotFocus = new Attribute(Color.Black, Color.BrightYellow),
            HotNormal = new Attribute(Color.Black, Color.BrightYellow)
        };

        Colors.ColorSchemes["Base"] = baseColorScheme;
        Colors.ColorSchemes["Dialog"] = baseColorScheme;
        Colors.ColorSchemes["Menu"] = menuColorScheme;
        Colors.ColorSchemes.Add("Exotic", exoticColorScheme);

        ColorScheme = Colors.ColorSchemes["Base"];

        _menuBar = new MenuBar(new[]
        {
            new MenuBarItem("File  |", new[]
            {
                new MenuItem("_Import", "", () => OnListImport()),
                new MenuItem("Export to DIM", "", () => ExportClicked()),
                new MenuItem("_Exit", "", () => Application.RequestStop())
            }),
            new MenuBarItem("Edit  |", new[]
            {
                new MenuItem("_Add armor filter", "", () => Application.Refresh()) // Needs to be implemented
            }),
            new MenuBarItem("Help  |", new[]
            {
                new MenuItem("_Help", "", () => HelpClicked()),
                new MenuItem("_About", "", () => AboutClicked())
            })
        });
        _menuBar.ColorScheme = Colors.ColorSchemes["Menu"];
        Add(_menuBar);

        armorView.X = 0;
        armorView.Y = 1;
        armorView.Width = Dim.Percent(50);
        armorView.Height = Dim.Percent(50);

        detailedView.X = Pos.Right(armorView);
        detailedView.Y = 1;
        detailedView.Width = Dim.Percent(50);
        detailedView.Height = Dim.Percent(50);

        fragmentView.X = 0;
        fragmentView.Y = Pos.Bottom(armorView);
        fragmentView.Width = Dim.Percent(50);
        fragmentView.Height = Dim.Percent(50) - 1; // - 1 other wise it would run out of frame

        summaryView.X = Pos.Right(fragmentView);
        summaryView.Y = Pos.Bottom(detailedView);
        summaryView.Width = Dim.Percent(50);
        summaryView.Height = Dim.Percent(50) - 1; // - 1 other wise it would run out of frame

        Add(armorView, detailedView, fragmentView, summaryView);
    }

    public event Action<ListImportedEventArgs> ListImported;
    public event Action<DialogRequestedEventArgs> DialogRequested;
    public event Action<ExportRequestedEventArgs> ExportRequested;

    private void HelpClicked()
    {
        DialogRequested.Invoke(new DialogRequestedEventArgs(0));
    }

    private void AboutClicked()
    {
        DialogRequested.Invoke(new DialogRequestedEventArgs(1));
    }

    private void ExportClicked()
    {
        ExportRequested.Invoke(new ExportRequestedEventArgs(0));
    }

    private void OnListImport()
    {
        var openDialog = new OpenDialog("Import...", "Choose your destinyArmor.csv")
        {
            AllowsMultipleSelection = false,
            AllowedFileTypes = new[] { ".csv" },
            DirectoryPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ColorScheme = Colors.ColorSchemes["Base"]
        };
        Application.Run(openDialog);
        if(!openDialog.Canceled)
            ListImported.Invoke(new ListImportedEventArgs(openDialog.FilePath.ToString()));
    }
}