using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeHighlighter.Model;

internal interface IText
{
    int LinesCount { get; }
    int VisibleLinesCount { get; }
    string GetSubstring(int lineIndex, int startIndex, int length);
    TextLine GetLine(int lineIndex);
    TextLine GetFirstLine();
    TextLine GetLastLine();
    int GetMaxLineWidth();
    string ToString();
    event EventHandler? TextChanged;
}

internal class Text : IText
{
    private readonly List<TextLine> _lines = new();

    public event EventHandler? TextChanged;

    public int LinesCount => _lines.Count;

    public int VisibleLinesCount => _lines.Count == 1 && !_lines[0].Any() ? 0 : _lines.Count;

    public Text() : this("") { }

    public Text(string text)
    {
        SetText(text);
    }

    public void SetText(string text)
    {
        _lines.Clear();
        _lines.AddRange(text.Split('\n').Select(line => new TextLine(line.Replace("\r", ""))).ToList());
        RaiseTextChanged();
    }

    public string GetSubstring(int lineIndex, int startIndex, int length)
    {
        return _lines[lineIndex].GetSubstring(startIndex, length);
    }

    public TextLine GetLine(int lineIndex) => _lines[lineIndex];

    public TextLine GetFirstLine() => _lines.First();

    public TextLine GetLastLine() => _lines.Last();

    public int GetMaxLineWidth()
    {
        if (!_lines.Any()) return 0;
        return _lines.Select(x => x.Length).Max();
    }

    public void NewLine(int lineIndex, int columnIndex)
    {
        var line = _lines[lineIndex];
        var remains = line.GetSubstring(columnIndex, line.Length - columnIndex);
        line.RemoveRange(columnIndex, line.Length - columnIndex);
        _lines.Insert(lineIndex + 1, new TextLine(remains));
        RaiseTextChanged();
    }

    public void AppendChar(int lineIndex, int columnIndex, char ch)
    {
        _lines[lineIndex].AppendChar(columnIndex, ch);
        RaiseTextChanged();
    }

    public void Insert(int lineIndex, int columnIndex, IText insertedText)
    {
        if (insertedText.LinesCount == 0) return;
        if (insertedText.LinesCount == 1)
        {
            _lines[lineIndex].InsertLine(columnIndex, insertedText.GetFirstLine());
            RaiseTextChanged();
        }
        else
        {
            NewLine(lineIndex, columnIndex);
            _lines[lineIndex].AppendLine(insertedText.GetFirstLine());
            for (int insertedLineIndex = 1; insertedLineIndex < insertedText.LinesCount - 1; insertedLineIndex++)
            {
                _lines.Insert(lineIndex + insertedLineIndex, insertedText.GetLine(insertedLineIndex));
            }
            _lines[lineIndex + insertedText.LinesCount - 1].InsertLine(0, insertedText.GetLastLine());
            RaiseTextChanged();
        }
    }

    public (int, int) GetCursorPositionAfterLeftDelete(int currentLineIndex, int currentColumnIndex)
    {
        if (currentColumnIndex > 0)
        {
            return (currentLineIndex, currentColumnIndex - 1);
        }
        else if (currentLineIndex > 0)
        {
            return (currentLineIndex - 1, _lines[currentLineIndex - 1].Length);
        }
        else
        {
            return (currentLineIndex, currentColumnIndex);
        }
    }

    public DeleteResult LeftDelete(int lineIndex, int columnIndex)
    {
        if (columnIndex > 0)
        {
            _lines[lineIndex].RemoveAt(columnIndex - 1);
            RaiseTextChanged();
        }
        else if (lineIndex > 0)
        {
            _lines[lineIndex - 1].AppendLine(_lines[lineIndex]);
            _lines.RemoveAt(lineIndex);
            RaiseTextChanged();
            return new DeleteResult { IsLineDeleted = true };
        }

        return new DeleteResult { IsLineDeleted = false };
    }

    public DeleteResult RightDelete(int lineIndex, int columnIndex)
    {
        if (columnIndex < _lines[lineIndex].Length)
        {
            _lines[lineIndex].RemoveAt(columnIndex);
            RaiseTextChanged();
        }
        else if (lineIndex < _lines.Count - 1)
        {
            _lines[lineIndex].AppendLine(_lines[lineIndex + 1]);
            _lines.RemoveAt(lineIndex + 1);
            RaiseTextChanged();
            return new DeleteResult { IsLineDeleted = true };
        }

        return new DeleteResult { IsLineDeleted = false };
    }

    public DeleteSelectionResult DeleteSelection(ITextSelection textSelection)
    {
        var selectedLines = textSelection.GetSelectedLines(this).ToList();
        if (selectedLines.Count == 1)
        {
            var selectedLine = selectedLines.First();
            var line = _lines[selectedLine.LineIndex];
            line.RemoveRange(selectedLine.LeftColumnIndex, selectedLine.RightColumnIndex - selectedLine.LeftColumnIndex);
            RaiseTextChanged();

            return new DeleteSelectionResult();
        }
        else
        {
            var firstSelectedLine = selectedLines.First();
            var lastSelectedLine = selectedLines.Last();
            var firstLine = _lines[firstSelectedLine.LineIndex];
            var lastLine = _lines[lastSelectedLine.LineIndex];
            firstLine.RemoveRange(firstSelectedLine.LeftColumnIndex, firstSelectedLine.RightColumnIndex - firstSelectedLine.LeftColumnIndex);
            firstLine.AppendLine(lastLine, lastSelectedLine.RightColumnIndex, lastLine.Length - lastSelectedLine.RightColumnIndex);
            var secondSelectedLine = selectedLines.Skip(1).First();
            _lines.RemoveRange(secondSelectedLine.LineIndex, selectedLines.Count - 1);
            RaiseTextChanged();

            return new DeleteSelectionResult(secondSelectedLine.LineIndex, selectedLines.Count - 1);
        }
    }

    public void DeleteLine(int lineIndex)
    {
        _lines.RemoveAt(lineIndex);
        if (!_lines.Any()) _lines.Add(new TextLine(""));
        RaiseTextChanged();
    }

    public void DeleteLines(int lineIndex, int count)
    {
        _lines.RemoveRange(lineIndex, count);
        if (!_lines.Any()) _lines.Add(new TextLine(""));
        RaiseTextChanged();
    }

    public void ReplaceLines(int sourceLineIndex, int destinationLineIndex)
    {
        var sourceLine = _lines[sourceLineIndex];
        _lines.RemoveAt(sourceLineIndex);
        _lines.Insert(destinationLineIndex, sourceLine);
        RaiseTextChanged();
    }

    public void SetSelectedTextCase(ITextSelection textSelection, TextCase textCase)
    {
        var selectedLines = textSelection.GetSelectedLines(this).ToList();
        foreach (var selectedLine in selectedLines)
        {
            var line = _lines[selectedLine.LineIndex];
            line.SetCase(selectedLine.LeftColumnIndex, selectedLine.RightColumnIndex - selectedLine.LeftColumnIndex, textCase);
        }
    }

    private void RaiseTextChanged()
    {
        TextChanged?.Invoke(this, EventArgs.Empty);
    }

    public override string ToString()
    {
        return String.Join(Environment.NewLine, _lines.Select(line => line.ToString()));
    }

    public struct DeleteResult
    {
        public bool IsLineDeleted;
    }

    public readonly struct DeleteSelectionResult
    {
        public readonly int FirstDeletedLineIndex;
        public readonly int DeletedLinesCount;

        public DeleteSelectionResult(int firstDeletedLineIndex, int deletedLinesCount)
        {
            FirstDeletedLineIndex = firstDeletedLineIndex;
            DeletedLinesCount = deletedLinesCount;
        }
    }
}
