namespace CodeHighlighter.Model;

internal interface ICodeTextBox
{
    bool Focus();
    void InvalidateVisual();
}

internal class DummyCodeTextBox : ICodeTextBox
{
    public static readonly DummyCodeTextBox Instance = new();

    public bool Focus() => false;

    public void InvalidateVisual() { }

    private DummyCodeTextBox() { }
}
