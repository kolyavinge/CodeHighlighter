namespace CodeHighlighter.InputActions;

internal interface IScrollLineUpInputAction
{
    void Do(InputActionContext context);
}

[InputAction]
internal class ScrollLineUpInputAction : IScrollLineUpInputAction
{
    public static readonly ScrollLineUpInputAction Instance = new();

    public void Do(InputActionContext context)
    {
        context.Viewport.ScrollLineUp();
    }
}
