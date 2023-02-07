﻿namespace CodeHighlighter.Model;

public interface IViewport
{
    double ActualWidth { get; }
    double ActualHeight { get; }
    double VerticalScrollBarValue { get; set; }
    double VerticalScrollBarMaximum { get; set; }
    double HorizontalScrollBarValue { get; set; }
    double HorizontalScrollBarMaximum { get; set; }
    int GetLinesCountInViewport();
    void SetHorizontalScrollBarMaximumValueStrategy(IHorizontalScrollBarMaximumValueStrategy strategy);
    void UpdateScrollbarsMaximumValues();
}

internal interface IViewportInternal : IViewport
{
    IViewportContext Context { get; set; }
    void CorrectByCursorPosition();
    void ScrollLineDown();
    void ScrollLineUp();
}

internal class Viewport : IViewportInternal
{
    private readonly ITextCursorAbsolutePosition _textCursorAbsolutePosition;
    private readonly ITextMeasuresInternal _textMeasures;
    private readonly IViewportVerticalOffsetUpdater _verticalOffsetUpdater;
    private readonly IVerticalScrollBarMaximumValueStrategy _verticalScrollBarMaximumValueStrategy;
    private IHorizontalScrollBarMaximumValueStrategy _horizontalScrollBarMaximumValueStrategy;
    private IViewportContext _context;

    public IViewportContext Context
    {
        get => _context;
        set
        {
            _context = value;
            _context.ViewportSizeChanged += (s, e) => UpdateIsHorizontalScrollBarVisible();
        }
    }

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

    public Viewport(
        IViewportContext context,
        ITextCursorAbsolutePosition textCursorAbsolutePosition,
        ITextMeasuresInternal textMeasures,
        IViewportVerticalOffsetUpdater verticalOffsetUpdater,
        IVerticalScrollBarMaximumValueStrategy verticalScrollBarMaximumValueStrategy,
        IHorizontalScrollBarMaximumValueStrategy horizontalScrollBarMaximumValueStrategy)
    {
        _context = context;
        _textCursorAbsolutePosition = textCursorAbsolutePosition;
        _textMeasures = textMeasures;
        _verticalOffsetUpdater = verticalOffsetUpdater;
        _verticalScrollBarMaximumValueStrategy = verticalScrollBarMaximumValueStrategy;
        _horizontalScrollBarMaximumValueStrategy = horizontalScrollBarMaximumValueStrategy;
    }

    public int GetLinesCountInViewport()
    {
        var result = (int)Math.Ceiling(_context.ActualHeight / _textMeasures.LineHeight);
        if (_context.ActualHeight % _textMeasures.LineHeight != 0) result++;

        return result;
    }

    public void SetHorizontalScrollBarMaximumValueStrategy(IHorizontalScrollBarMaximumValueStrategy strategy)
    {
        _horizontalScrollBarMaximumValueStrategy = strategy;
    }

    public void CorrectByCursorPosition()
    {
        var cursorAbsolutePoint = _textCursorAbsolutePosition.Position;

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
        _context.HorizontalScrollBarMaximum = _horizontalScrollBarMaximumValueStrategy.GetValue();
        _context.VerticalScrollBarMaximum = _verticalScrollBarMaximumValueStrategy.GetValue();
        UpdateIsHorizontalScrollBarVisible();
    }

    private void UpdateIsHorizontalScrollBarVisible()
    {
        _context.IsHorizontalScrollBarVisible = _context.HorizontalScrollBarMaximum > _context.ActualWidth;
        if (!_context.IsHorizontalScrollBarVisible)
        {
            _context.HorizontalScrollBarValue = 0;
        }
    }

    public void ScrollLineUp()
    {
        _context.VerticalScrollBarValue = _verticalOffsetUpdater.GetVerticalOffsetAfterScrollLineUp(_context.VerticalScrollBarValue, _textMeasures.LineHeight);
    }

    public void ScrollLineDown()
    {
        _context.VerticalScrollBarValue = _verticalOffsetUpdater.GetVerticalOffsetAfterScrollLineDown(_context.VerticalScrollBarValue, _context.VerticalScrollBarMaximum, _textMeasures.LineHeight);
    }
}
