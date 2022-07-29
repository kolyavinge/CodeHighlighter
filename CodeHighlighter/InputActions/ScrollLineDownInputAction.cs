using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal class ScrollLineDownInputAction
{
    public static readonly ScrollLineDownInputAction Instance = new();

    public void Do(Viewport viewport, ICodeTextBox? codeTextBox)
    {
        viewport.ScrollLineDown();
        codeTextBox?.InvalidateVisual();
    }
}
