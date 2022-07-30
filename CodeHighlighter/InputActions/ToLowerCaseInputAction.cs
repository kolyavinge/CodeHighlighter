namespace CodeHighlighter.InputActions;

internal class ToLowerCaseInputAction
{
    public static readonly ToLowerCaseInputAction Instance = new();

    public void Do(InputActionContext context)
    {
        context.InputModel.SetSelectedTextCase(TextCase.Lower);
        context.RaiseTextChanged();
        context.CodeTextBox?.InvalidateVisual();
    }
}
