namespace CodeHighlighter.InputActions;

internal interface IScrollLineDownInputAction
{
    void Do(IInputActionContext context);
}

internal class ScrollLineDownInputAction : IScrollLineDownInputAction
{
    public void Do(IInputActionContext context)
    {
        context.Viewport.ScrollLineDown();
    }
}
