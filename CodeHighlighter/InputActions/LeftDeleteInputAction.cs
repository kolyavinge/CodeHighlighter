using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal class LeftDeleteInputAction : InputAction
{
    public static readonly LeftDeleteInputAction Instance = new();

    public DeleteResult Do(InputActionContext context)
    {
        var result = LeftDelete(context);
        context.Viewport.CorrectByCursorPosition(context.TextCursor);
        context.Viewport.UpdateScrollbarsMaximumValues(context.Text);
        context.RaiseTextChanged();

        return result;
    }

    private DeleteResult LeftDelete(InputActionContext context)
    {
        var charDeleteResult = default(Text.CharDeleteResult);
        var oldCursorPosition = context.TextCursor.Position;
        var (selectionStart, selectionEnd) = context.TextSelection.GetSortedPositions();
        var deletedSelectedText = GetSelectedText(context);
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
