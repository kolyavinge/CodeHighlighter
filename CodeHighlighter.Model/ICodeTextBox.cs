namespace CodeHighlighter;

public interface ICodeTextBox : IViewportContext
{
    bool Focus();
    void InvalidateVisual();
}

public interface IViewportContext
{
    double ActualWidth { get; }
    double ActualHeight { get; }
    double VerticalScrollBarValue { get; set; }
    double VerticalScrollBarMaximum { get; set; }
    double HorizontalScrollBarValue { get; set; }
    double HorizontalScrollBarMaximum { get; set; }
}
