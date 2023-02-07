using CodeHighlighter.Model;
using static CodeHighlighter.Model.IText;

namespace CodeHighlighter.InputActions;

internal interface ILeftDeleteInputAction
{
    DeleteResult Do(IInputActionContext context);
}

internal class LeftDeleteInputAction : InputAction, ILeftDeleteInputAction
{
    public DeleteResult Do(IInputActionContext context)
    {
        var result = LeftDelete(context);
        context.CursorPositionCorrector.CorrectPosition();
        context.Viewport.UpdateScrollBarsMaximumValues();
        context.TextEvents.RaiseTextChanged();

        return result;
    }

    private DeleteResult LeftDelete(IInputActionContext context)
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
            UpdateTokensForLines(context, context.TextCursor.LineIndex, 1);
        }
        else
        {
            if (context.TextCursor.Kind == CursorPositionKind.Virtual)
            {
                context.TextCursor.MoveTo(new(context.TextCursor.LineIndex, 0));
            }
            else
            {
                var newPosition = context.Text.GetCursorPositionAfterLeftDelete(context.TextCursor.Position);
                charDeleteResult = context.Text.LeftDelete(context.TextCursor.Position);
                if (charDeleteResult.IsLineDeleted) context.Tokens.DeleteLine(context.TextCursor.LineIndex);
                context.TextCursor.MoveTo(newPosition);
                UpdateTokensForLines(context, context.TextCursor.LineIndex, 1);
            }
        }
        var newCursorPosition = context.TextCursor.Position;

        return new(oldCursorPosition, newCursorPosition, selectionStart, selectionEnd, deletedSelectedText, charDeleteResult);
    }
}
