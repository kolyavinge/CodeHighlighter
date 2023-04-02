using CodeHighlighter.Model;

namespace CodeHighlighter.Core;

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
    void UpdateScrollBarsMaximumValues();
}

internal interface IViewportInternal : IViewport
{
    void SetContext(IViewportContext context);
    void ScrollLineDown();
    void ScrollLineUp();
}

internal class Viewport : IViewportInternal
{
    private readonly ITextMeasuresInternal _textMeasures;
    private readonly IViewportVerticalOffsetUpdater _verticalOffsetUpdater;
    private readonly IVerticalScrollBarMaximumValueStrategy _verticalScrollBarMaximumValueStrategy;
    private IHorizontalScrollBarMaximumValueStrategy _horizontalScrollBarMaximumValueStrategy;
    private IViewportContext _context;

    public double ActualWidth => _context.ActualWidth;

    public double ActualHeight => _context.ActualHeight;

    public double VerticalScrollBarValue
    {
        get => _context.VerticalScrollBarValue;
        set
        {
            if (value >= 0)
            {
                _context.VerticalScrollBarValue = value;
            }
            else
            {
                _context.VerticalScrollBarValue = 0;
            }
        }
    }

    public double VerticalScrollBarMaximum
    {
        get => _context.VerticalScrollBarMaximum;
        set => _context.VerticalScrollBarMaximum = value;
    }

    public double HorizontalScrollBarValue
    {
        get => _context.HorizontalScrollBarValue;
        set
        {
            if (value >= 0)
            {
                _context.HorizontalScrollBarValue = value;
            }
            else
            {
                _context.HorizontalScrollBarValue = 0;
            }
        }
    }

    public double HorizontalScrollBarMaximum
    {
        get => _context.HorizontalScrollBarMaximum;
        set => _context.HorizontalScrollBarMaximum = value;
    }

    public Viewport(
        ITextMeasuresInternal textMeasures,
        IViewportVerticalOffsetUpdater verticalOffsetUpdater,
        IVerticalScrollBarMaximumValueStrategy verticalScrollBarMaximumValueStrategy,
        IHorizontalScrollBarMaximumValueStrategy horizontalScrollBarMaximumValueStrategy)
    {
        _context = new DummyViewportContext();
        _textMeasures = textMeasures;
        _verticalOffsetUpdater = verticalOffsetUpdater;
        _verticalScrollBarMaximumValueStrategy = verticalScrollBarMaximumValueStrategy;
        _horizontalScrollBarMaximumValueStrategy = horizontalScrollBarMaximumValueStrategy;
    }

    public void SetContext(IViewportContext context)
    {
        _context = context;
        _context.ViewportSizeChanged += (s, e) => UpdateIsHorizontalScrollBarVisible();
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

    public void UpdateScrollBarsMaximumValues()
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
