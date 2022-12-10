using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal class RightDeleteInputAction : InputAction
{
    public static readonly RightDeleteInputAction Instance = new();

    public DeleteResult Do(InputActionContext context)
    {
        var result = RightDelete(context);
        context.Viewport.CorrectByCursorPosition(context.TextCursor);
        context.Viewport.UpdateScrollbarsMaximumValues(context.Text);
        context.RaiseTextChanged();

        return result;
    }

    private DeleteResult RightDelete(InputActionContext context)
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
