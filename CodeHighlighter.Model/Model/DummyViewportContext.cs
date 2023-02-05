namespace CodeHighlighter.Model;

internal class DummyViewportContext : IViewportContext
{
#pragma warning disable CS0067
    public event EventHandler? ViewportSizeChanged;
#pragma warning restore CS0067
    public double ActualWidth => 0;
    public double ActualHeight => 0;
    public double VerticalScrollBarValue { get; set; }
    public double VerticalScrollBarMaximum { get; set; }
    public double HorizontalScrollBarValue { get; set; }
    public double HorizontalScrollBarMaximum { get; set; }
    public bool IsHorizontalScrollBarVisible { get; set; }
}
