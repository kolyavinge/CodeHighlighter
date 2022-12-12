namespace CodeHighlighter.InputActions;

internal class ScrollLineUpInputAction
{
    public static readonly ScrollLineUpInputAction Instance = new();

    public void Do(InputActionContext context)
    {
        context.Viewport.ScrollLineUp();
    }
}
