namespace CodeHighlighter.InputActions;

internal class ScrollLineDownInputAction
{
    public static readonly ScrollLineDownInputAction Instance = new();

    public void Do(InputActionContext context)
    {
        context.Viewport.ScrollLineDown();
    }
}
