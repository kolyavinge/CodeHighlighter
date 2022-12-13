using CodeHighlighter.Common;

namespace CodeHighlighter.Model;

public interface IViewportContext
{
    double ActualWidth { get; }
    double ActualHeight { get; }
    double VerticalScrollBarValue { get; set; }
    double VerticalScrollBarMaximum { get; set; }
    double HorizontalScrollBarValue { get; set; }
    double HorizontalScrollBarMaximum { get; set; }
}

public interface IViewport
{
    void CorrectByCursorPosition();
    CursorPosition GetCursorPosition(Point cursorClickPosition);
    int GetLinesCountInViewport();
    void ScrollLineDown();
    void ScrollLineUp();
    void UpdateScrollbarsMaximumValues();
}

public class Viewport : IViewport
{
    private readonly IText _text;
    private readonly IViewportContext _context;
    private readonly TextCursor _textCursor;
    private readonly TextMeasures _textMeasures;

    public double ActualWidth => _context.ActualWidth;

    public double ActualHeight => _context.ActualHeight;

    public double VerticalScrollBarValue
    {
        get => _context.VerticalScrollBarValue;
        set => _context.VerticalScrollBarValue = value;
    }

    public double VerticalScrollBarMaximum
    {
        get => _context.VerticalScrollBarMaximum;
        set => _context.VerticalScrollBarMaximum = value;
    }

    public double HorizontalScrollBarValue
    {
        get => _context.HorizontalScrollBarValue;
        set => _context.HorizontalScrollBarValue = value;
    }

    public double HorizontalScrollBarMaximum
    {
        get => _context.HorizontalScrollBarMaximum;
        set => _context.HorizontalScrollBarMaximum = value;
    }


    public Viewport(IText text, IViewportContext context, TextCursor textCursor, TextMeasures textMeasures)
    {
        _text = text;
        _context = context;
        _textCursor = textCursor;
        _textMeasures = textMeasures;
    }

    public int GetLinesCountInViewport()
    {
        var result = (int)Math.Ceiling(_context.ActualHeight / _textMeasures.LineHeight);
        if (_context.ActualHeight % _textMeasures.LineHeight != 0) result++;

        return result;
    }

    public CursorPosition GetCursorPosition(Point cursorClickPosition)
    {
        var lineIndex = (int)((cursorClickPosition.Y + _context.VerticalScrollBarValue) / _textMeasures.LineHeight);
        var columnIndex = (int)((cursorClickPosition.X + _textMeasures.LetterWidth / 2.0 + _context.HorizontalScrollBarValue) / _textMeasures.LetterWidth);
        return new(lineIndex, columnIndex);
    }

    public void CorrectByCursorPosition()
    {
        CorrectByCursorPosition(_textCursor.GetAbsolutePosition(_textMeasures));
    }

    private void CorrectByCursorPosition(Point cursorAbsolutePoint)
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

    public void UpdateScrollbarsMaximumValues()
    {
        var maxLineWidthInPixels = _text.GetMaxLineWidth() * _textMeasures.LetterWidth;
        _context.HorizontalScrollBarMaximum = _context.ActualWidth < maxLineWidthInPixels ? maxLineWidthInPixels : 0;
        if (_context.HorizontalScrollBarMaximum == 0)
        {
            _context.HorizontalScrollBarValue = 0;
        }
        _context.VerticalScrollBarMaximum = _text.LinesCount * _textMeasures.LineHeight;
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
    public double ActualWidth => 0;
    public double ActualHeight => 0;
    public double VerticalScrollBarValue { get; set; }
    public double VerticalScrollBarMaximum { get; set; }
    public double HorizontalScrollBarValue { get; set; }
    public double HorizontalScrollBarMaximum { get; set; }
}
