using System.Collections.Generic;

namespace CodeHighlighter.Model
{
    internal interface ITextSelection
    {
        int StartLineIndex { get; }
        int StartColumnIndex { get; }
        int EndLineIndex { get; }
        int EndColumnIndex { get; }
        (TextSelectionPosition, TextSelectionPosition) GetSortedPositions();
        IEnumerable<TextSelectionLine> GetSelectedLines(IText text);
    }

    internal struct TextSelectionPosition
    {
        public readonly int LineIndex;
        public readonly int ColumnIndex;

        public TextSelectionPosition(int lineIndex, int columnIndex)
        {
            LineIndex = lineIndex;
            ColumnIndex = columnIndex;
        }
    }

    internal struct TextSelectionLine
    {
        public readonly int LineIndex;
        public readonly int LeftColumnIndex;
        public readonly int RightColumnIndex;

        public TextSelectionLine(int lineIndex, int leftColumnIndex, int rightColumnIndex)
        {
            LineIndex = lineIndex;
            LeftColumnIndex = leftColumnIndex;
            RightColumnIndex = rightColumnIndex;
        }
    }

    class TextSelection : ITextSelection
    {
        public bool IsExist => EndLineIndex != -1;
        public bool InProgress { get; set; }
        public int StartLineIndex { get; set; }
        public int StartColumnIndex { get; set; }
        public int EndLineIndex { get; set; }
        public int EndColumnIndex { get; set; }

        public TextSelection()
        {
            Reset();
        }

        public TextSelection(int startLineIndex, int startColumnIndex, int endLineIndex, int endColumnIndex) : this()
        {
            StartLineIndex = startLineIndex;
            StartColumnIndex = startColumnIndex;
            EndLineIndex = endLineIndex;
            EndColumnIndex = endColumnIndex;
        }

        public (TextSelectionPosition, TextSelectionPosition) GetSortedPositions()
        {
            var start = new TextSelectionPosition(StartLineIndex, StartColumnIndex);
            var end = new TextSelectionPosition(EndLineIndex, EndColumnIndex);
            if (start.LineIndex < end.LineIndex) return (start, end);
            if (start.LineIndex > end.LineIndex) return (end, start);
            if (start.ColumnIndex < end.ColumnIndex) return (start, end);
            return (end, start);
        }

        public IEnumerable<TextSelectionLine> GetSelectedLines(IText text)
        {
            if (!IsExist) yield break;
            (var start, var end) = GetSortedPositions();
            if (start.LineIndex == end.LineIndex)
            {
                yield return new TextSelectionLine(start.LineIndex, start.ColumnIndex, end.ColumnIndex);
            }
            else
            {
                yield return new TextSelectionLine(start.LineIndex, start.ColumnIndex, text.GetLine(start.LineIndex).Length);
                for (int middleLineIndex = start.LineIndex + 1; middleLineIndex <= end.LineIndex - 1; middleLineIndex++)
                {
                    yield return new TextSelectionLine(middleLineIndex, 0, text.GetLine(middleLineIndex).Length);
                }
                yield return new TextSelectionLine(end.LineIndex, 0, end.ColumnIndex);
            }
        }

        public void Reset()
        {
            InProgress = false;
            EndLineIndex = -1;
        }
    }
}
