using System.Linq;
using CodeHighlighter.Common;

namespace CodeHighlighter.Model;

internal interface ITextCursorAbsolutePosition
{
    Point Position { get; }
}

internal class TextCursorAbsolutePosition : ITextCursorAbsolutePosition
{
    private readonly ITextCursor _cursor;
    private readonly ITextMeasuresInternal _measures;
    private readonly ILineGapCollection _gaps;

    public TextCursorAbsolutePosition(ITextCursor cursor, ITextMeasuresInternal measures, ILineGapCollection gaps)
    {
        _cursor = cursor;
        _measures = measures;
        _gaps = gaps;
    }

    public Point Position
    {
        get
        {
            var lineIndex = _cursor.LineIndex;

            if (_gaps.AnyItems)
            {
                lineIndex += (int)(Enumerable.Range(0, _cursor.LineIndex + 1).Sum(i => _gaps[i]?.CountBefore) ?? 0);
            }

            return new(_cursor.ColumnIndex * _measures.LetterWidth, lineIndex * _measures.LineHeight);
        }
    }
}
