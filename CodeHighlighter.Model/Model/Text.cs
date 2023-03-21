using System.Collections.Generic;
using System.Linq;
using static CodeHighlighter.Model.IText;

namespace CodeHighlighter.Model;

internal interface IText
{
    bool IsEmpty { get; }
    IEnumerable<TextLine> Lines { get; }
    int LinesCount { get; }
    string TextContent { get; set; }
    void AppendChar(CursorPosition position, char ch);
    void AppendChar(CursorPosition position, char ch, int count);
    void AppendNewLine(CursorPosition position);
    void DeleteLine(int lineIndex);
    void DeleteLines(int lineIndex, int count);
    DeleteSelectionResult DeleteSelection(IEnumerable<TextSelectionLine> selectedLines);
    CursorPosition GetCursorPositionAfterLeftDelete(CursorPosition current);
    TextLine GetLine(int lineIndex);
    InsertResult Insert(CursorPosition position, IText insertedText);
    CharDeleteResult LeftDelete(CursorPosition position);
    void ReplaceLines(int sourceLineIndex, int destinationLineIndex);
    CharDeleteResult RightDelete(CursorPosition position);
    void SetSelectedTextCase(IEnumerable<TextSelectionLine> selectedLines, TextCase textCase);
    string ToString();

    public readonly struct InsertResult
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

    public struct CharDeleteResult
    {
        public char DeletedChar;
        public bool IsLineDeleted;
        public bool HasDeleted => DeletedChar != 0 || IsLineDeleted;
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

internal class Text : IText
{
    private readonly List<TextLine> _lines = new();

    public IEnumerable<TextLine> Lines => _lines;

    public int LinesCount => _lines.Count;

    public bool IsEmpty => _lines.Count == 1 && !_lines[0].Any();

    public string TextContent
    {
        get => ToString();
        set
        {
            _lines.Clear();
            _lines.AddRange(value.Split('\n').Select(line => new TextLine(line.Replace("\r", ""))).ToList());
        }
    }

    public Text() : this("") { }

    internal Text(string text)
    {
        TextContent = text;
    }

    public TextLine GetLine(int lineIndex)
    {
        return _lines[lineIndex];
    }

    public void AppendNewLine(CursorPosition position)
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

    public void AppendChar(CursorPosition position, char ch)
    {
        if (NotAllowedSymbols.Value.Contains(ch)) throw new ArgumentException(nameof(ch));
        _lines[position.LineIndex].AppendChar(position.ColumnIndex, ch);
    }

    public void AppendChar(CursorPosition position, char ch, int count)
    {
        if (NotAllowedSymbols.Value.Contains(ch)) throw new ArgumentException(nameof(ch));
        for (int i = 0; i < count; i++)
        {
            _lines[position.LineIndex].AppendChar(position.ColumnIndex, ch);
        }
    }

    public InsertResult Insert(CursorPosition position, IText insertedText)
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

    public CursorPosition GetCursorPositionAfterLeftDelete(CursorPosition current)
    {
        if (current.ColumnIndex > 0) return new(current.LineIndex, current.ColumnIndex - 1);
        else if (current.LineIndex > 0) return new(current.LineIndex - 1, _lines[current.LineIndex - 1].Length);
        else return new(current.LineIndex, current.ColumnIndex);
    }

    public CharDeleteResult LeftDelete(CursorPosition position)
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

    public CharDeleteResult RightDelete(CursorPosition position)
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

    public DeleteSelectionResult DeleteSelection(IEnumerable<TextSelectionLine> selectedLines)
    {
        var selectedLinesList = selectedLines.ToList();
        if (selectedLinesList.Count == 1)
        {
            var selectedLine = selectedLinesList.First();
            var line = _lines[selectedLine.LineIndex];
            line.RemoveRange(selectedLine.LeftColumnIndex, selectedLine.RightColumnIndex - selectedLine.LeftColumnIndex);

            return new();
        }
        else
        {
            var firstSelectedLine = selectedLinesList.First();
            var lastSelectedLine = selectedLinesList.Last();
            var firstLine = _lines[firstSelectedLine.LineIndex];
            var lastLine = _lines[lastSelectedLine.LineIndex];
            firstLine.RemoveRange(firstSelectedLine.LeftColumnIndex, firstSelectedLine.RightColumnIndex - firstSelectedLine.LeftColumnIndex);
            firstLine.AppendLine(lastLine, lastSelectedLine.RightColumnIndex, lastLine.Length - lastSelectedLine.RightColumnIndex);
            var secondSelectedLine = selectedLinesList.Skip(1).First();
            _lines.RemoveRange(secondSelectedLine.LineIndex, selectedLinesList.Count - 1);

            return new(secondSelectedLine.LineIndex, selectedLinesList.Count - 1);
        }
    }

    public void DeleteLine(int lineIndex)
    {
        _lines.RemoveAt(lineIndex);
        if (!_lines.Any()) _lines.Add(new TextLine(""));
    }

    public void DeleteLines(int lineIndex, int count)
    {
        _lines.RemoveRange(lineIndex, count);
        if (!_lines.Any()) _lines.Add(new TextLine(""));
    }

    public void ReplaceLines(int sourceLineIndex, int destinationLineIndex)
    {
        var sourceLine = _lines[sourceLineIndex];
        _lines.RemoveAt(sourceLineIndex);
        _lines.Insert(destinationLineIndex, sourceLine);
    }

    public void SetSelectedTextCase(IEnumerable<TextSelectionLine> selectedLines, TextCase textCase)
    {
        foreach (var selectedLine in selectedLines)
        {
            var line = _lines[selectedLine.LineIndex];
            line.SetCase(selectedLine.LeftColumnIndex, selectedLine.RightColumnIndex - selectedLine.LeftColumnIndex, textCase);
        }
    }

    public override string ToString() => String.Join(Environment.NewLine, _lines.Select(line => line.ToString()));
}

internal static class TextExt
{
    public static int GetMaxLineWidth(this IText text) => text.Lines.Max(x => x.Length);
}

public static class NotAllowedSymbols
{
    public static readonly IReadOnlyCollection<char> Value = new HashSet<char>(new[] { '\n', '\r', '\b', '\u001B' });
}
