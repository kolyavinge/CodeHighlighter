namespace CodeHighlighter.Model;

public interface ICodeTextBox : IViewportContext, IClipboard
{
    event EventHandler<FontSettingsChangedEventArgs>? FontSettingsChanged;

    bool Focus();
    void InvalidateVisual();
}

public interface IViewportContext
{
    event EventHandler? ViewportSizeChanged;
    double ActualWidth { get; }
    double ActualHeight { get; }
    double VerticalScrollBarValue { get; set; }
    double VerticalScrollBarMaximum { get; set; }
    double HorizontalScrollBarValue { get; set; }
    double HorizontalScrollBarMaximum { get; set; }
    bool IsHorizontalScrollBarVisible { get; set; }
}

public interface IClipboard
{
    void ClipboardSetText(string text);
    string ClipboardGetText();
}

public class FontSettingsChangedEventArgs : EventArgs
{
    public double LineHeight { get; }
    public double LetterWidth { get; }

    public FontSettingsChangedEventArgs(double lineHeight, double letterWidth)
    {
        LineHeight = lineHeight;
        LetterWidth = letterWidth;
    }
}
