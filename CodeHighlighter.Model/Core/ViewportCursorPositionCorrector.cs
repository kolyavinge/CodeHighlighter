namespace CodeHighlighter.Core;

internal interface IViewportCursorPositionCorrector
{
    void CorrectPosition();
}

internal class ViewportCursorPositionCorrector : IViewportCursorPositionCorrector
{
    private readonly IViewportInternal _viewport;
    private readonly ITextMeasuresInternal _textMeasures;
    private readonly ITextCursorAbsolutePosition _textCursorAbsolutePosition;

    public ViewportCursorPositionCorrector(
        IViewportInternal viewport,
        ITextMeasuresInternal textMeasures,
        ITextCursorAbsolutePosition textCursorAbsolutePosition)
    {
        _viewport = viewport;
        _textMeasures = textMeasures;
        _textCursorAbsolutePosition = textCursorAbsolutePosition;
    }

    public void CorrectPosition()
    {
        var cursorAbsolutePosition = _textCursorAbsolutePosition.Position;

        if (cursorAbsolutePosition.X < _viewport.HorizontalScrollBarValue)
        {
            _viewport.HorizontalScrollBarValue = cursorAbsolutePosition.X;
        }
        else if (cursorAbsolutePosition.X + _textMeasures.LetterWidth > _viewport.HorizontalScrollBarValue + _viewport.ActualWidth)
        {
            _viewport.HorizontalScrollBarValue = cursorAbsolutePosition.X - _viewport.ActualWidth + _textMeasures.LetterWidth;
        }

        if (cursorAbsolutePosition.Y < _viewport.VerticalScrollBarValue)
        {
            _viewport.VerticalScrollBarValue = cursorAbsolutePosition.Y;
        }
        else if (cursorAbsolutePosition.Y + _textMeasures.LineHeight > _viewport.VerticalScrollBarValue + _viewport.ActualHeight)
        {
            _viewport.VerticalScrollBarValue = cursorAbsolutePosition.Y - _viewport.ActualHeight + _textMeasures.LineHeight;
        }
    }
}
