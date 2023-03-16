namespace CodeHighlighter.Model;

public interface ILineFoldingPanel
{
    double ActualHeight { get; }
    double VerticalScrollBarValue { get; }
    int TextLinesCount { get; }
    double TextLineHeight { get; }
    void InvalidateVisual();
}
