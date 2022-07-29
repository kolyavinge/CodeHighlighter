using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal class ScrollLineUpInputAction
{
    public static readonly ScrollLineUpInputAction Instance = new();

    public void Do(Viewport viewport, ICodeTextBox? codeTextBox)
    {
        viewport.ScrollLineUp();
        codeTextBox?.InvalidateVisual();
    }
}
