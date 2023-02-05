namespace CodeHighlighter.Model;

internal class DummyCodeTextBox : ICodeTextBox
{
    public static readonly DummyCodeTextBox Instance = new();

#pragma warning disable CS0067
    public event EventHandler<FontSettingsChangedEventArgs>? FontSettingsChanged;
    public event EventHandler? ViewportSizeChanged;
#pragma warning restore CS0067

    public double ActualWidth => 0;
    public double ActualHeight => 0;
    public double VerticalScrollBarValue { get => 0; set { } }
    public double VerticalScrollBarMaximum { get => 0; set { } }
    public double HorizontalScrollBarValue { get => 0; set { } }
    public double HorizontalScrollBarMaximum { get => 0; set { } }
    public bool IsHorizontalScrollBarVisible { get => false; set { } }
    private DummyCodeTextBox() { }
    public bool Focus() => false;
    public void InvalidateVisual() { }
    public void ClipboardSetText(string text) { }
    public string ClipboardGetText() => "";
}
