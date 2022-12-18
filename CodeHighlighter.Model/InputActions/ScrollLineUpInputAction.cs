namespace CodeHighlighter.InputActions;

internal interface IScrollLineUpInputAction
{
    void Do(IInputActionContext context);
}

internal class ScrollLineUpInputAction : IScrollLineUpInputAction
{
    public void Do(IInputActionContext context)
    {
        context.Viewport.ScrollLineUp();
    }
}
