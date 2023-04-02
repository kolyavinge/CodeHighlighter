using System.Collections.Generic;
using System.Linq;
using CodeHighlighter.Common;

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
    private Dictionary<int, int> _foldedLines;

    public event EventHandler? ItemsSet;
    public event EventHandler<EventArgs>? Activated;
    public event EventHandler<EventArgs>? Deactivated;

    public LineFolds()
    {
        _items = new List<LineFold>();
        _foldedLines = new Dictionary<int, int>();
    }

    public IReadOnlyCollection<LineFold> Items
    {
        get => _items;
        set
        {
            ErrorIfFoldsIntersected(value.ToArray());
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
                if (_foldedLines.ContainsKey(i + lineFold.LineIndex))
                {
                    _foldedLines[i + lineFold.LineIndex]++;
                }
                else
                {
                    _foldedLines.Add(i + lineFold.LineIndex, 1);
                }
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
                if (_foldedLines.ContainsKey(i + lineFold.LineIndex))
                {
                    _foldedLines[i + lineFold.LineIndex]--;
                }
                else
                {
                    _foldedLines.Add(i + lineFold.LineIndex, 1);
                }
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
        return _foldedLines.ContainsKey(lineIndex) && _foldedLines[lineIndex] > 0;
    }

    public int GetUnfoldedLineIndexUp(int lineIndex)
    {
        while (IsFolded(lineIndex)) lineIndex--;
        return lineIndex;
    }

    public int GetUnfoldedLineIndexDown(int lineIndex)
    {
        while (IsFolded(lineIndex)) lineIndex++;
        return lineIndex;
    }

    public void UpdateAfterLineAdd(int startLineIndex, int linesCount)
    {
        _items.Where(x => x.LineIndex >= startLineIndex).Each(x => x.LineIndex += linesCount);
        _foldedLines = _foldedLines
            .Select(x => new { Key = x.Key >= startLineIndex ? x.Key + linesCount : x.Key, x.Value })
            .ToDictionary(x => x.Key, v => v.Value);
    }

    public void UpdateAfterLineDelete(int startLineIndex, int linesCount)
    {
        _items.Where(x => x.LineIndex >= startLineIndex).Each(x => x.LineIndex -= linesCount);
        _foldedLines = _foldedLines
            .Select(x => new { Key = x.Key >= startLineIndex ? x.Key - linesCount : x.Key, x.Value })
            .ToDictionary(x => x.Key, v => v.Value);
    }

    private void ErrorIfFoldsIntersected(LineFold[] lineFolds)
    {
        for (int i = 0; i < lineFolds.Length - 1; i++)
        {
            var x = lineFolds[i];
            for (int j = i + 1; j < lineFolds.Length; j++)
            {
                var y = lineFolds[j];
                if (x.LineIndex == y.LineIndex)
                {
                    throw new ArgumentException("Folds cannot be intersected");
                }
                if (x.LineIndex <= y.LineIndex && y.LineIndex < x.LineIndex + x.LinesCount
                    && x.LineIndex + x.LinesCount < y.LineIndex + y.LinesCount)
                {
                    throw new ArgumentException("Folds cannot be intersected");
                }
            }
        }
    }
}
