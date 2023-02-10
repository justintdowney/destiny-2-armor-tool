using System.Collections;
using Terminal.Gui;
using TestApp.Data;

namespace TestApp.TUI;

internal class SelectionDataSource<T> : IListDataSource
{
    private List<T> _list;
    private BitArray _marks;

    public List<T> List
    {
        get => _list;
        set
        {
            Count = value.Count;
            _marks = new BitArray(Count);
            _list = value;
            Length = GetMaxLength();
        }
    }

    public int Count { get; private set; }

    public int Length { get; private set; }

    public bool IsMarked(int item)
    {
        if (item < 0 || item > Count)
            return false;

        return _marks[item];
    }

    public void SetMark(int item, bool value)
    {
        if (item < 0 || item > Count)
            return;

        _marks[item] = value;
    }

    public void Render(ListView container, ConsoleDriver driver, bool selected, int item, int col, int line, int width,
        int start = 0)
    {
        container.Move(col, line);
        var t = _list[item]?.ToString();
        if (!_marks[item])
            driver.AddStr(Constants.Unmarked + t);
        else
            driver.AddStr(Constants.Marked + t);
    }

    public IList ToList()
    {
        return _list;
    }

    private int GetMaxLength()
    {
        if (_list.Count == 0) return 0;

        var len = 0;

        foreach (var item in _list)
        {
            var itemText = item?.ToString();
            if (itemText?.Length > len)
                len = itemText.Length;
        }

        return len;
    }
}