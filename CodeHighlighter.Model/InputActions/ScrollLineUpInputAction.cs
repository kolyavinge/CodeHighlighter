namespace CodeHighlighter.InputActions;

internal interface IScrollLineUpInputAction
{
    void Do(InputActionContext context);
}

[InputAction]
internal class ScrollLineUpInputAction : IScrollLineUpInputAction
{
    public void Do(InputActionContext context)
    {
        context.Viewport.ScrollLineUp();
    }
}
