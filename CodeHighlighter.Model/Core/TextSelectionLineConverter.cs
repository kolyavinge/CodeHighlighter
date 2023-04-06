using System.Collections.Generic;

namespace CodeHighlighter.Core;

internal interface ITextSelectionLineConverter
{
    IEnumerable<TextSelectionLine> GetSelectedLines(CursorPosition start, CursorPosition end);
}

internal class TextSelectionLineConverter : ITextSelectionLineConverter
{
    private readonly IText _text;

    public TextSelectionLineConverter(IText text)
    {
        _text = text;
    }

    public IEnumerable<TextSelectionLine> GetSelectedLines(CursorPosition start, CursorPosition end)
    {
        if (start.LineIndex == end.LineIndex)
        {
            yield return new TextSelectionLine(start.LineIndex, start.ColumnIndex, end.ColumnIndex);
        }
        else
        {
            var rightColumnIndex = start.Kind == CursorPositionKind.Real ? _text.GetLine(start.LineIndex).Length : start.ColumnIndex;
            yield return new TextSelectionLine(start.LineIndex, start.ColumnIndex, rightColumnIndex);
            for (int middleLineIndex = start.LineIndex + 1; middleLineIndex <= end.LineIndex - 1; middleLineIndex++)
            {
                yield return new TextSelectionLine(middleLineIndex, 0, _text.GetLine(middleLineIndex).Length);
            }
            yield return new TextSelectionLine(end.LineIndex, 0, end.ColumnIndex);
        }
    }
}
