using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal interface ISetTextCaseInputAction
{
    CaseResult Do(IInputActionContext context, TextCase textCase);
}

[InputAction]
internal class SetTextCaseInputAction : InputAction, ISetTextCaseInputAction
{
    public CaseResult Do(IInputActionContext context, TextCase textCase)
    {
        var cursorPosition = context.TextCursor.Position;
        var (selectionStart, selectionEnd) = context.TextSelection.GetSortedPositions();
        var deletedSelectedText = context.TextSelector.GetSelectedText();
        context.Text.SetSelectedTextCase(context.TextSelection.GetSelectedLines(), textCase);
        var changedText = context.TextSelector.GetSelectedText();
        UpdateTokensForLines(context, selectionStart.LineIndex, selectionEnd.LineIndex - selectionStart.LineIndex + 1);
        var result = new CaseResult(cursorPosition, selectionStart, selectionEnd, deletedSelectedText, changedText);
        context.TextEvents.RaiseTextChanged();

        return result;
    }
}
