namespace CodeHighlighter.InputActions;

internal interface IScrollLineDownInputAction
{
    void Do(InputActionContext context);
}

[InputAction]
internal class ScrollLineDownInputAction : IScrollLineDownInputAction
{
    public void Do(InputActionContext context)
    {
        context.Viewport.ScrollLineDown();
    }
}
