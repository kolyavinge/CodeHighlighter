namespace CodeHighlighter.InputActions;

internal interface IScrollLineDownInputAction
{
    void Do(IInputActionContext context);
}

[InputAction]
internal class ScrollLineDownInputAction : IScrollLineDownInputAction
{
    public void Do(IInputActionContext context)
    {
        context.Viewport.ScrollLineDown();
    }
}
