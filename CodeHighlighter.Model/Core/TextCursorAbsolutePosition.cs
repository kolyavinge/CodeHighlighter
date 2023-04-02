using CodeHighlighter.Common;

namespace CodeHighlighter.Core;

internal interface ITextCursorAbsolutePosition
{
    Point Position { get; }
}

internal class TextCursorAbsolutePosition : ITextCursorAbsolutePosition
{
    private readonly ITextCursor _cursor;
    private readonly ITextMeasuresInternal _measures;
    private readonly IExtendedLineNumberGenerator _lineNumberGenerator;

    public TextCursorAbsolutePosition(
        ITextCursor cursor, ITextMeasuresInternal measures, IExtendedLineNumberGenerator lineNumberGenerator)
    {
        _cursor = cursor;
        _measures = measures;
        _lineNumberGenerator = lineNumberGenerator;
    }

    public Point Position
    {
        get
        {
            var offsetY = _lineNumberGenerator.GetLineOffsetY(_cursor.LineIndex, _measures.LineHeight);
            return new(_cursor.ColumnIndex * _measures.LetterWidth, offsetY);
        }
    }
}
