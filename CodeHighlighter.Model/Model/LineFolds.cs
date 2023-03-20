using System.Collections.Generic;
using System.Linq;
using CodeHighlighter.Utils;

namespace CodeHighlighter.Model;

public class LineFold
{
    public int LineIndex { get; internal set; }
    public int LinesCount { get; }
    public bool IsActive { get; internal set; }

    public LineFold(int lineIndex, int linesCount)
    {
        LineIndex = lineIndex;
        LinesCount = linesCount;
        IsActive = false;
    }
}

public interface ILineFolds
{
    event EventHandler? ItemsSet;
    event EventHandler<EventArgs>? Activated;
    event EventHandler<EventArgs>? Deactivated;
    IReadOnlyCollection<LineFold> Items { get; set; }
    bool AnyFoldedItems { get; }
    int FoldedLinesCount { get; }
    void Activate(IEnumerable<int> lineIndexCollection);
    void Deactivate(IEnumerable<int> lineIndexCollection);
    void Switch(int lineIndex);
    bool IsFolded(int lineIndex);
    int GetUnfoldedLineIndexUp(int lineIndex);
    int GetUnfoldedLineIndexDown(int lineIndex);
    void UpdateAfterLineAdd(int startLineIndex, int linesCount);
    void UpdateAfterLineDelete(int startLineIndex, int linesCount);
}

internal class LineFolds : ILineFolds
{
    private readonly List<LineFold> _items;
    private readonly HashSet<int> _foldedLines;

    public event EventHandler? ItemsSet;
    public event EventHandler<EventArgs>? Activated;
    public event EventHandler<EventArgs>? Deactivated;

    public LineFolds()
    {
        _items = new List<LineFold>();
        _foldedLines = new HashSet<int>();
    }

    public IReadOnlyCollection<LineFold> Items
    {
        get => _items;
        set
        {
            _items.Clear();
            _items.AddRange(value);
            _foldedLines.Clear();
            Activate(_items.Where(x => x.IsActive).Select(x => x.LineIndex).ToArray());
            ItemsSet?.Invoke(this, EventArgs.Empty);
        }
    }

    public bool AnyFoldedItems => _items.Any(x => x.IsActive);

    public int FoldedLinesCount => Items.Where(x => x.IsActive).Sum(x => x.LinesCount);

    public void Activate(IEnumerable<int> lineIndexCollection)
    {
        var flag = false;
        foreach (var lineFold in _items.Join(lineIndexCollection, o => o.LineIndex, i => i, (i, o) => i))
        {
            flag = true;
            lineFold.IsActive = true;
            for (int i = 1; i <= lineFold.LinesCount; i++)
            {
                _foldedLines.Add(i + lineFold.LineIndex);
            }
        }
        if (flag)
        {
            Activated?.Invoke(this, EventArgs.Empty);
        }
    }

    public void Deactivate(IEnumerable<int> lineIndexCollection)
    {
        var flag = false;
        foreach (var lineFold in _items.Join(lineIndexCollection, o => o.LineIndex, i => i, (i, o) => i))
        {
            flag = true;
            lineFold.IsActive = false;
            for (int i = 1; i <= lineFold.LinesCount; i++)
            {
                _foldedLines.Remove(i + lineFold.LineIndex);
            }
        }
        if (flag)
        {
            Deactivated?.Invoke(this, EventArgs.Empty);
        }
    }

    public void Switch(int lineIndex)
    {
        var fold = _items.FirstOrDefault(x => x.LineIndex == lineIndex);
        if (fold == null) return;
        if (fold.IsActive)
        {
            Deactivate(new[] { lineIndex });
        }
        else
        {
            Activate(new[] { lineIndex });
        }
    }

    public bool IsFolded(int lineIndex)
    {
        return _foldedLines.Contains(lineIndex);
    }

    public int GetUnfoldedLineIndexUp(int lineIndex)
    {
        while (_foldedLines.Contains(lineIndex)) lineIndex--;
        return lineIndex;
    }

    public int GetUnfoldedLineIndexDown(int lineIndex)
    {
        while (_foldedLines.Contains(lineIndex)) lineIndex++;
        return lineIndex;
    }

    public void UpdateAfterLineAdd(int startLineIndex, int linesCount)
    {
        _items.Where(x => x.LineIndex >= startLineIndex).Each(x => x.LineIndex += linesCount);
        var foldedLinesOld = _foldedLines.ToList();
        _foldedLines.Clear();
        foreach (var foldedLineOld in foldedLinesOld)
        {
            if (foldedLineOld >= startLineIndex)
            {
                _foldedLines.Add(foldedLineOld + linesCount);
            }
            else
            {
                _foldedLines.Add(foldedLineOld);
            }
        }
    }

    public void UpdateAfterLineDelete(int startLineIndex, int linesCount)
    {
        _items.Where(x => x.LineIndex >= startLineIndex).Each(x => x.LineIndex -= linesCount);
        var foldedLinesOld = _foldedLines.ToList();
        _foldedLines.Clear();
        foreach (var foldedLineOld in foldedLinesOld)
        {
            if (foldedLineOld >= startLineIndex)
            {
                _foldedLines.Add(foldedLineOld - linesCount);
            }
            else
            {
                _foldedLines.Add(foldedLineOld);
            }
        }
    }
}
