using System.Globalization;
using CsvHelper;
using DestinyArmorTool.Data;
using DestinyArmorTool.Events;
using DestinyArmorTool.Model;
using DestinyArmorTool.TUI;
using NStack;
using Terminal.Gui;

namespace DestinyArmorTool.Presenter;

public class FragmentPresenter
{
    private readonly FragmentModel _fragmentModel;
    private readonly IFragmentView _fragmentView;
    private readonly IMessageAggregator _messageAggregator;
    private readonly SelectionDataSource<Fragment> _dataSource;

    public FragmentPresenter(IFragmentView fragmentView)
    {
        _fragmentModel = new FragmentModel();
        _fragmentModel.SelectedIndices = new List<int>();
        _fragmentView = fragmentView;
        _dataSource = new SelectionDataSource<Fragment>();
        _fragmentView.Source = _dataSource;
        LoadFragmentList();
        LoadSubclassLabels();
        _fragmentView.SubclassSelectionChanged += LoadDisplayedFragments;
        _fragmentView.FragmentMarked += PublishFragmentSelection;
        _messageAggregator = MessageAggregator.Instance;
    }

    private void LoadSubclassLabels()
    {
        var subclasses =
            _fragmentModel.Fragments.Select(x => x.Subclass).Distinct()
                .ToArray(); // could optimize this using a dictionary instead
        var subclassRadioLabels = new ustring[subclasses.Length];
        for (var i = 0; i < subclasses.Length; i++)
            subclassRadioLabels[i] = ustring.Make(subclasses[i]);
        _fragmentView.RadioLabels = subclassRadioLabels;
    }

    private void LoadFragmentList()
    {
        using var reader = new StreamReader(Constants.FragmentPath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        var records = csv.GetRecords<Fragment>();
        _fragmentModel.Fragments = records.ToList();
    }

    private void LoadDisplayedFragments(SelectedItemChangedArgs e)
    {
        var item = _fragmentView.RadioLabels[e.SelectedItem];
        var filtered = _fragmentModel.Fragments.Where(x => x.Subclass == item);
        _dataSource.List = filtered.ToList();
    }

    private void PublishFragmentSelection(ListViewItemEventArgs e)
    {
        if (_fragmentModel.SelectedIndices.Any(x => x == e.Item))
            _fragmentModel.SelectedIndices.Remove(e.Item);
        else
            _fragmentModel.SelectedIndices.Add(e.Item);

        _messageAggregator.Publish(null, new SelectedCountChangedEventArgs(_fragmentModel.SelectedIndices.Count, e.Value));
    }
}