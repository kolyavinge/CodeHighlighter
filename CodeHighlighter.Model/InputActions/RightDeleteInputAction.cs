using CodeHighlighter.Model;
using static CodeHighlighter.Model.IText;

namespace CodeHighlighter.InputActions;

internal interface IRightDeleteInputAction
{
    DeleteResult Do(IInputActionContext context);
}

internal class RightDeleteInputAction : InputAction, IRightDeleteInputAction
{
    public DeleteResult Do(IInputActionContext context)
    {
        var result = RightDelete(context);
        context.CursorPositionCorrector.CorrectPosition();
        context.Viewport.UpdateScrollBarsMaximumValues();
        context.LineFoldsUpdater.UpdateRightDelete(result);
        context.TextEvents.RaiseTextChangedAfterRightDelete(result);

        return result;
    }

    private DeleteResult RightDelete(IInputActionContext context)
    {
        var charDeleteResult = default(CharDeleteResult);
        var oldCursorPosition = context.TextCursor.Position;
        var (selectionStart, selectionEnd) = context.TextSelection.GetSortedPositions();
        var deletedSelectedText = context.TextSelector.GetSelectedText();
        if (context.TextSelection.IsExist)
        {
            if (selectionStart.Kind == CursorPositionKind.Virtual)
            {
                context.Text.AppendChar(new(selectionStart.LineIndex, 0), ' ', selectionStart.ColumnIndex);
                context.TextCursor.Kind = CursorPositionKind.Real;
            }
            DeleteSelection(context);
        }
        else
        {
            if (context.TextCursor.Kind == CursorPositionKind.Virtual)
            {
                context.Text.AppendChar(new(context.TextCursor.LineIndex, 0), ' ', context.TextCursor.ColumnIndex);
                context.TextCursor.Kind = CursorPositionKind.Real;
            }
            charDeleteResult = context.Text.RightDelete(context.TextCursor.Position);
            if (charDeleteResult.IsLineDeleted) context.Tokens.DeleteLine(context.TextCursor.LineIndex + 1);
        }
        var newCursorPosition = context.TextCursor.Position;
        UpdateTokensForLines(context, context.TextCursor.LineIndex, 1);

        return new(oldCursorPosition, newCursorPosition, selectionStart, selectionEnd, deletedSelectedText, charDeleteResult);
    }
}
