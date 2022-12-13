using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal class SetTextCaseInputAction : InputAction
{
    public static readonly SetTextCaseInputAction Instance = new();

    public CaseResult Do(InputActionContext context, TextCase textCase)
    {
        var cursorPosition = context.TextCursor.Position;
        var (selectionStart, selectionEnd) = context.TextSelection.GetSortedPositions();
        var deletedSelectedText = context.TextSelector.GetSelectedText();
        context.Text.SetSelectedTextCase(context.TextSelection.GetSelectedLines(), textCase);
        var changedText = context.TextSelector.GetSelectedText();
        UpdateTokensForLines(context, selectionStart.LineIndex, selectionEnd.LineIndex - selectionStart.LineIndex + 1);
        var result = new CaseResult(cursorPosition, selectionStart, selectionEnd, deletedSelectedText, changedText);
        context.RaiseTextChanged();

        return result;
    }
}
