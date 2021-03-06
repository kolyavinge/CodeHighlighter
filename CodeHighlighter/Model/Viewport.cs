using System.Windows;

namespace CodeHighlighter.Model;

interface IViewportContext
{
    double ActualWidth { get; }
    double ActualHeight { get; }
    double VerticalScrollBarValue { get; set; }
    double VerticalScrollBarMaximum { get; set; }
    double HorizontalScrollBarValue { get; set; }
    double HorizontalScrollBarMaximum { get; set; }
}

internal class Viewport
{
    private readonly IViewportContext _context;
    private readonly TextMeasures _textMeasures;

    public Viewport(IViewportContext context, TextMeasures textMeasures)
    {
        _context = context;
        _textMeasures = textMeasures;
    }

    public int GetLinesCountInViewport()
    {
        var result = (int)(_context.ActualHeight / _textMeasures.LineHeight) + 1;
        if (_context.ActualHeight % _textMeasures.LineHeight != 0) result++;

        return result;
    }

    public int GetCursorLineIndex(Point cursorClickPosition)
    {
        return (int)((cursorClickPosition.Y + _context.VerticalScrollBarValue) / _textMeasures.LineHeight);
    }

    public int GetCursorColumnIndex(Point cursorClickPosition)
    {
        return (int)((cursorClickPosition.X + _textMeasures.LetterWidth / 2.0 + _context.HorizontalScrollBarValue) / _textMeasures.LetterWidth);
    }

    public void CorrectByCursorPosition(ITextCursor textCursor)
    {
        CorrectByCursorPosition(textCursor.GetAbsolutePosition(_textMeasures));
    }

    public void CorrectByCursorPosition(Point cursorAbsolutePoint)
    {
        if (cursorAbsolutePoint.X < _context.HorizontalScrollBarValue)
        {
            _context.HorizontalScrollBarValue = cursorAbsolutePoint.X;
        }
        else if (cursorAbsolutePoint.X + _textMeasures.LetterWidth > _context.HorizontalScrollBarValue + _context.ActualWidth)
        {
            _context.HorizontalScrollBarValue = cursorAbsolutePoint.X - _context.ActualWidth + _textMeasures.LetterWidth;
        }

        if (cursorAbsolutePoint.Y < _context.VerticalScrollBarValue)
        {
            _context.VerticalScrollBarValue = cursorAbsolutePoint.Y;
        }
        else if (cursorAbsolutePoint.Y + _textMeasures.LineHeight > _context.VerticalScrollBarValue + _context.ActualHeight)
        {
            _context.VerticalScrollBarValue = cursorAbsolutePoint.Y - _context.ActualHeight + _textMeasures.LineHeight;
        }
    }

    public void UpdateScrollbarsMaximumValues(IText text)
    {
        var maxLineWidthInPixels = text.GetMaxLineWidth() * _textMeasures.LetterWidth;
        _context.HorizontalScrollBarMaximum = _context.ActualWidth < maxLineWidthInPixels ? maxLineWidthInPixels : 0;
        if (_context.HorizontalScrollBarMaximum == 0)
        {
            _context.HorizontalScrollBarValue = 0;
        }
        _context.VerticalScrollBarMaximum = text.LinesCount * _textMeasures.LineHeight;
    }

    public void ScrollLineUp()
    {
        _context.VerticalScrollBarValue = GetVerticalOffsetAfterScrollLineUp(_context.VerticalScrollBarValue, _textMeasures.LineHeight);
    }

    public static double GetVerticalOffsetAfterScrollLineUp(double verticalScrollBarValue, double lineHeight)
    {
        var offset = verticalScrollBarValue;
        var delta = verticalScrollBarValue % lineHeight;
        if (delta == 0) offset -= lineHeight;
        else offset -= delta;
        if (offset < 0) offset = 0;

        return offset;
    }

    public void ScrollLineDown()
    {
        _context.VerticalScrollBarValue = GetVerticalOffsetAfterScrollLineDown(_context.VerticalScrollBarValue, _context.VerticalScrollBarMaximum, _textMeasures.LineHeight);
    }

    public static double GetVerticalOffsetAfterScrollLineDown(double verticalScrollBarValue, double verticalScrollBarMaximum, double lineHeight)
    {
        var offset = verticalScrollBarValue;
        var delta = verticalScrollBarValue % lineHeight;
        if (delta == 0) offset += lineHeight;
        else offset += delta;
        if (offset > verticalScrollBarMaximum) offset = verticalScrollBarMaximum;

        return offset;
    }
}

internal class DummyViewportContext : IViewportContext
{
    public double ActualWidth { get; }
    public double ActualHeight { get; }
    public double VerticalScrollBarValue { get; set; }
    public double VerticalScrollBarMaximum { get; set; }
    public double HorizontalScrollBarValue { get; set; }
    public double HorizontalScrollBarMaximum { get; set; }
}
