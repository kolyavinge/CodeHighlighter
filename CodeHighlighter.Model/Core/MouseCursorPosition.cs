using CodeHighlighter.Common;

namespace CodeHighlighter.Core;

internal interface IMouseCursorPosition
{
    CursorPosition GetCursorPosition(Point mousePosition);
}

internal class MouseCursorPosition : IMouseCursorPosition
{
    private readonly IViewport _viewport;
    private readonly ITextMeasures _textMeasures;
    private readonly IExtendedLineNumberGenerator _lineNumberGenerator;

    public MouseCursorPosition(
        IViewport viewport,
        ITextMeasures textMeasures,
        IExtendedLineNumberGenerator lineNumberGenerator)
    {
        _viewport = viewport;
        _textMeasures = textMeasures;
        _lineNumberGenerator = lineNumberGenerator;
    }

    public CursorPosition GetCursorPosition(Point mousePosition)
    {
        var lineIndex = _lineNumberGenerator.GetLineIndex(mousePosition.Y, _viewport.ActualHeight, _viewport.VerticalScrollBarValue, _textMeasures.LineHeight, int.MaxValue);
        var columnIndex = (int)((mousePosition.X + _textMeasures.LetterWidth / 2.0 + _viewport.HorizontalScrollBarValue) / _textMeasures.LetterWidth);

        return new(lineIndex, columnIndex);
    }
}
