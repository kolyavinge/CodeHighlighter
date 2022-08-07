using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal class ToLowerCaseInputAction
{
    public static readonly ToLowerCaseInputAction Instance = new();

    public CaseResult Do(InputActionContext context)
    {
        var result = context.InputModel.SetSelectedTextCase(TextCase.Lower);
        context.RaiseTextChanged();

        return result;
    }
}
