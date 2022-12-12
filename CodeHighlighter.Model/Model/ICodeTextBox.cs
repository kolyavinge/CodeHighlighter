namespace CodeHighlighter.Model;

public interface ICodeTextBox : IViewportContext
{
    bool Focus();
    void InvalidateVisual();
}

internal class DummyCodeTextBox : ICodeTextBox
{
    public static readonly DummyCodeTextBox Instance = new();

    public double ActualWidth => 0;
    public double ActualHeight => 0;
    public double VerticalScrollBarValue { get => 0; set { } }
    public double VerticalScrollBarMaximum { get => 0; set { } }
    public double HorizontalScrollBarValue { get => 0; set { } }
    public double HorizontalScrollBarMaximum { get => 0; set { } }
    public bool Focus() => false;
    public void InvalidateVisual() { }
    private DummyCodeTextBox() { }
}
