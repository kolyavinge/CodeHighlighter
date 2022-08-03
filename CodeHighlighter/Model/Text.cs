using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeHighlighter.Model;

internal interface IText
{
    IEnumerable<TextLine> Lines { get; }
    int LinesCount { get; }
    TextLine GetLine(int lineIndex);
}

public class Text : IText
{
    private readonly List<TextLine> _lines = new();

    public IEnumerable<TextLine> Lines => _lines;

    public int LinesCount => _lines.Count;

    internal int VisibleLinesCount => _lines.Count == 1 && !_lines[0].Any() ? 0 : _lines.Count;

    internal string TextContent
    {
        get => ToString();
        set
        {
            _lines.Clear();
            _lines.AddRange(value.Split('\n').Select(line => new TextLine(line.Replace("\r", ""))).ToList());
        }
    }

    internal Text() : this("") { }

    internal Text(string text)
    {
        TextContent = text;
    }

    public TextLine GetLine(int lineIndex) => _lines[lineIndex];

    internal void NewLine(CursorPosition position)
    {
        var line = _lines[position.LineIndex];
        var remains = line.GetSubstring(position.ColumnIndex, line.Length - position.ColumnIndex);
        line.RemoveRange(position.ColumnIndex, line.Length - position.ColumnIndex);
        _lines.Insert(position.LineIndex + 1, new TextLine(remains));
    }

    internal static readonly HashSet<char> NotAllowedSymbols = new(new[] { '\n', '\r', '\b', '\u001B' });

    internal void AppendChar(CursorPosition position, char ch)
    {
        if (NotAllowedSymbols.Contains(ch)) throw new ArgumentException(nameof(ch));
        _lines[position.LineIndex].AppendChar(position.ColumnIndex, ch);
    }

    internal void Insert(CursorPosition position, Text insertedText)
    {
        if (insertedText.LinesCount == 0) return;
        if (insertedText.LinesCount == 1)
        {
            _lines[position.LineIndex].InsertLine(position.ColumnIndex, insertedText.Lines.First());
        }
        else
        {
            NewLine(position);
            _lines[position.LineIndex].AppendLine(insertedText.Lines.First());
            for (int insertedLineIndex = 1; insertedLineIndex < insertedText.LinesCount - 1; insertedLineIndex++)
            {
                _lines.Insert(position.LineIndex + insertedLineIndex, insertedText.GetLine(insertedLineIndex));
            }
            _lines[position.LineIndex + insertedText.LinesCount - 1].InsertLine(0, insertedText.Lines.Last());
        }
    }

    internal CursorPosition GetCursorPositionAfterLeftDelete(CursorPosition current)
    {
        if (current.ColumnIndex > 0) return new(current.LineIndex, current.ColumnIndex - 1);
        else if (current.LineIndex > 0) return new(current.LineIndex - 1, _lines[current.LineIndex - 1].Length);
        else return new(current.LineIndex, current.ColumnIndex);
    }

    internal DeleteResult LeftDelete(CursorPosition position)
    {
        if (position.ColumnIndex > 0)
        {
            _lines[position.LineIndex].RemoveAt(position.ColumnIndex - 1);
        }
        else if (position.LineIndex > 0)
        {
            _lines[position.LineIndex - 1].AppendLine(_lines[position.LineIndex]);
            _lines.RemoveAt(position.LineIndex);
            return new DeleteResult { IsLineDeleted = true };
        }

        return new DeleteResult { IsLineDeleted = false };
    }

    internal DeleteResult RightDelete(CursorPosition position)
    {
        if (position.ColumnIndex < _lines[position.LineIndex].Length)
        {
            _lines[position.LineIndex].RemoveAt(position.ColumnIndex);
        }
        else if (position.LineIndex < _lines.Count - 1)
        {
            _lines[position.LineIndex].AppendLine(_lines[position.LineIndex + 1]);
            _lines.RemoveAt(position.LineIndex + 1);
            return new DeleteResult { IsLineDeleted = true };
        }

        return new DeleteResult { IsLineDeleted = false };
    }

    internal DeleteSelectionResult DeleteSelection(ITextSelection textSelection)
    {
        var selectedLines = textSelection.GetSelectedLines(this).ToList();
        if (selectedLines.Count == 1)
        {
            var selectedLine = selectedLines.First();
            var line = _lines[selectedLine.LineIndex];
            line.RemoveRange(selectedLine.LeftColumnIndex, selectedLine.RightColumnIndex - selectedLine.LeftColumnIndex);

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

            return new DeleteSelectionResult(secondSelectedLine.LineIndex, selectedLines.Count - 1);
        }
    }

    internal void DeleteLine(int lineIndex)
    {
        _lines.RemoveAt(lineIndex);
        if (!_lines.Any()) _lines.Add(new TextLine(""));
    }

    internal void DeleteLines(int lineIndex, int count)
    {
        _lines.RemoveRange(lineIndex, count);
        if (!_lines.Any()) _lines.Add(new TextLine(""));
    }

    internal void ReplaceLines(int sourceLineIndex, int destinationLineIndex)
    {
        var sourceLine = _lines[sourceLineIndex];
        _lines.RemoveAt(sourceLineIndex);
        _lines.Insert(destinationLineIndex, sourceLine);
    }

    internal void SetSelectedTextCase(ITextSelection textSelection, TextCase textCase)
    {
        var selectedLines = textSelection.GetSelectedLines(this).ToList();
        foreach (var selectedLine in selectedLines)
        {
            var line = _lines[selectedLine.LineIndex];
            line.SetCase(selectedLine.LeftColumnIndex, selectedLine.RightColumnIndex - selectedLine.LeftColumnIndex, textCase);
        }
    }

    public override string ToString()
    {
        return String.Join(Environment.NewLine, _lines.Select(line => line.ToString()));
    }

    internal struct DeleteResult
    {
        public bool IsLineDeleted;
    }

    internal readonly struct DeleteSelectionResult
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

internal static class TextExt
{
    public static int GetMaxLineWidth(this IText text)
    {
        if (!text.Lines.Any()) return 0;
        return text.Lines.Select(x => x.Length).Max();
    }
}
