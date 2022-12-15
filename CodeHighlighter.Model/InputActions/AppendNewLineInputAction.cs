using System.Linq;
using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal interface IAppendNewLineInputAction
{
    AppendNewLineResult Do(IInputActionContext context);
}

[InputAction]
internal class AppendNewLineInputAction : InputAction, IAppendNewLineInputAction
{
    public AppendNewLineResult Do(IInputActionContext context)
    {
        var result = AppendNewLine(context);
        context.Viewport.CorrectByCursorPosition();
        context.Viewport.UpdateScrollbarsMaximumValues();
        context.TextEvents.RaiseTextChanged();

        return result;
    }

    private AppendNewLineResult AppendNewLine(IInputActionContext context)
    {
        var oldCursorPosition = context.TextCursor.Position;
        var (selectionStart, selectionEnd) = context.TextSelection.GetSortedPositions();
        var deletedSelectedText = context.TextSelector.GetSelectedText();
        if (context.TextSelection.IsExist)
        {
            if (selectionStart.Kind == CursorPositionKind.Virtual)
            {
                context.TextSelection.StartPosition = new(context.TextSelection.StartPosition.LineIndex, 0);
            }
            DeleteSelection(context);
        }
        context.Text.AppendNewLine(context.TextCursor.Position);
        context.Tokens.InsertEmptyLine(context.TextCursor.LineIndex + 1);
        context.TextCursor.MoveDown();
        context.TextCursor.MoveEndLine();
        if (context.Text.GetLine(context.TextCursor.LineIndex).Any()) context.TextCursor.MoveStartLine();
        var newCursorPosition = context.TextCursor.Position;
        UpdateTokensForLines(context, context.TextCursor.LineIndex - 1, 2);

        return new(oldCursorPosition, newCursorPosition, selectionStart, selectionEnd, deletedSelectedText);
    }
}
