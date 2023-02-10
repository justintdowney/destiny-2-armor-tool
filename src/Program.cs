using Terminal.Gui;
using TestApp.Presenter;
using TestApp.TUI;

namespace TestApp;

public static class Program
{
    public static void Main()
    {
        Application.Init();

        var armorView = new ArmorView();
        var detailedView = new DetailedView();
        var fragmentView = new FragmentView();
        var summaryView = new SummaryView();
        var mainView = new MainView(armorView, detailedView, fragmentView, summaryView);

        var armorPresenter = new ArmorPresenter(armorView);
        var detailedPresenter = new DetailedPresenter(detailedView);
        var fragmentPresenter = new FragmentPresenter(fragmentView);
        var summaryPresenter = new SummaryPresenter(summaryView);
        var mainPresenter = new MainPresenter(mainView);

        Application.Run(mainView);
        Application.Shutdown();
    }
}