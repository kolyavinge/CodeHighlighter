using System.Collections.Generic;
using System.Linq;

namespace CodeHighlighter.Model;

public interface IText
{
    IEnumerable<TextLine> Lines { get; }
    int LinesCount { get; }
    bool IsEmpty { get; }
    TextLine GetLine(int lineIndex);
}

public class Text : IText
{
    private readonly List<TextLine> _lines = new();

    public IEnumerable<TextLine> Lines => _lines;

    public int LinesCount => _lines.Count;

    public bool IsEmpty => _lines.Count == 1 && !_lines[0].Any();

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

    public TextLine GetLine(int lineIndex)
    {
        return _lines[lineIndex];
    }

    internal void AppendNewLine(CursorPosition position)
    {
        if (position.Kind == CursorPositionKind.Real)
        {
            var line = _lines[position.LineIndex];
            var remains = line.GetSubstring(position.ColumnIndex, line.Length - position.ColumnIndex);
            line.RemoveRange(position.ColumnIndex, line.Length - position.ColumnIndex);
            _lines.Insert(position.LineIndex + 1, new TextLine(remains));
        }
        else
        {
            _lines.Insert(position.LineIndex + 1, new TextLine(""));
        }
    }

    public static readonly IReadOnlyCollection<char> NotAllowedSymbols = new HashSet<char>(new[] { '\n', '\r', '\b', '\u001B' });

    internal void AppendChar(CursorPosition position, char ch)
    {
        if (NotAllowedSymbols.Contains(ch)) throw new ArgumentException(nameof(ch));
        _lines[position.LineIndex].AppendChar(position.ColumnIndex, ch);
    }

    internal void AppendChar(CursorPosition position, char ch, int count)
    {
        if (NotAllowedSymbols.Contains(ch)) throw new ArgumentException(nameof(ch));
        for (int i = 0; i < count; i++)
        {
            _lines[position.LineIndex].AppendChar(position.ColumnIndex, ch);
        }
    }

    internal InsertResult Insert(CursorPosition position, Text insertedText)
    {
        if (insertedText.IsEmpty) return default;
        CursorPosition endPosition;
        if (insertedText.LinesCount == 1)
        {
            var line = insertedText.Lines.First();
            _lines[position.LineIndex].InsertLine(position.ColumnIndex, line);
            endPosition = new(position.LineIndex, position.ColumnIndex + line.Length);
        }
        else
        {
            AppendNewLine(position);
            _lines[position.LineIndex].AppendLine(insertedText.Lines.First());
            for (int insertedLineIndex = 1; insertedLineIndex < insertedText.LinesCount - 1; insertedLineIndex++)
            {
                _lines.Insert(position.LineIndex + insertedLineIndex, insertedText.GetLine(insertedLineIndex));
            }
            _lines[position.LineIndex + insertedText.LinesCount - 1].InsertLine(0, insertedText.Lines.Last());
            endPosition = new(position.LineIndex + insertedText.LinesCount - 1, insertedText.Lines.Last().Length);
        }

        return new(position, endPosition);
    }

    internal CursorPosition GetCursorPositionAfterLeftDelete(CursorPosition current)
    {
        if (current.ColumnIndex > 0) return new(current.LineIndex, current.ColumnIndex - 1);
        else if (current.LineIndex > 0) return new(current.LineIndex - 1, _lines[current.LineIndex - 1].Length);
        else return new(current.LineIndex, current.ColumnIndex);
    }

    internal CharDeleteResult LeftDelete(CursorPosition position)
    {
        if (position.ColumnIndex > 0)
        {
            var deletedChar = _lines[position.LineIndex][position.ColumnIndex - 1];
            _lines[position.LineIndex].RemoveAt(position.ColumnIndex - 1);

            return new() { DeletedChar = deletedChar };
        }
        else if (position.LineIndex > 0)
        {
            _lines[position.LineIndex - 1].AppendLine(_lines[position.LineIndex]);
            _lines.RemoveAt(position.LineIndex);

            return new() { DeletedChar = '\n', IsLineDeleted = true };
        }
        else return default;
    }

    internal CharDeleteResult RightDelete(CursorPosition position)
    {
        if (position.ColumnIndex < _lines[position.LineIndex].Length)
        {
            var deletedChar = _lines[position.LineIndex][position.ColumnIndex];
            _lines[position.LineIndex].RemoveAt(position.ColumnIndex);

            return new() { DeletedChar = deletedChar };
        }
        else if (position.LineIndex < _lines.Count - 1)
        {
            _lines[position.LineIndex].AppendLine(_lines[position.LineIndex + 1]);
            _lines.RemoveAt(position.LineIndex + 1);

            return new() { DeletedChar = '\n', IsLineDeleted = true };
        }
        else return default;
    }

    internal DeleteSelectionResult DeleteSelection(TextSelection textSelection)
    {
        var selectedLines = textSelection.GetSelectedLines(this).ToList();
        if (selectedLines.Count == 1)
        {
            var selectedLine = selectedLines.First();
            var line = _lines[selectedLine.LineIndex];
            line.RemoveRange(selectedLine.LeftColumnIndex, selectedLine.RightColumnIndex - selectedLine.LeftColumnIndex);

            return new();
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

            return new(secondSelectedLine.LineIndex, selectedLines.Count - 1);
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

    internal void SetSelectedTextCase(TextSelection textSelection, TextCase textCase)
    {
        var selectedLines = textSelection.GetSelectedLines(this).ToList();
        foreach (var selectedLine in selectedLines)
        {
            var line = _lines[selectedLine.LineIndex];
            line.SetCase(selectedLine.LeftColumnIndex, selectedLine.RightColumnIndex - selectedLine.LeftColumnIndex, textCase);
        }
    }

    public override string ToString() => String.Join(Environment.NewLine, _lines.Select(line => line.ToString()));

    internal readonly struct InsertResult
    {
        public readonly CursorPosition StartPosition;
        public readonly CursorPosition EndPosition;
        public bool HasInserted => !StartPosition.Equals(EndPosition);

        public InsertResult(CursorPosition startPosition, CursorPosition endPosition)
        {
            StartPosition = startPosition;
            EndPosition = endPosition;
        }
    }

    internal struct CharDeleteResult
    {
        public char DeletedChar;
        public bool IsLineDeleted;
        public bool HasDeleted => DeletedChar != 0 || IsLineDeleted;
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
    public static int GetMaxLineWidth(this IText text) => text.Lines.Select(x => x.Length).Max();
}
