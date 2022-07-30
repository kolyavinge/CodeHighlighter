namespace CodeHighlighter.InputActions;

internal class ToUpperCaseInputAction
{
    public static readonly ToUpperCaseInputAction Instance = new();

    public void Do(InputActionContext context)
    {
        context.InputModel.SetSelectedTextCase(TextCase.Upper);
        context.RaiseTextChanged();
        context.CodeTextBox?.InvalidateVisual();
    }
}
