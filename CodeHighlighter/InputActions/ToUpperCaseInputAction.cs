using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal class ToUpperCaseInputAction
{
    public static readonly ToUpperCaseInputAction Instance = new();

    public CaseResult Do(InputActionContext context)
    {
        var result = context.InputModel.SetSelectedTextCase(TextCase.Upper);
        context.RaiseTextChanged();

        return result;
    }
}
