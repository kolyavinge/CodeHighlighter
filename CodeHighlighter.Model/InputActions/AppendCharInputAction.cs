using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal class AppendCharInputAction : InputAction
{
    public static readonly AppendCharInputAction Instance = new();

    public AppendCharResult Do(InputActionContext context, char ch)
    {
        var result = AppendChar(context, ch);
        context.Viewport.CorrectByCursorPosition(context.TextCursor);
        context.Viewport.UpdateScrollbarsMaximumValues();
        context.RaiseTextChanged();

        return result;
    }

    private AppendCharResult AppendChar(InputActionContext context, char ch)
    {
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
        else if (context.TextCursor.Kind == CursorPositionKind.Virtual)
        {
            context.Text.AppendChar(new(context.TextCursor.LineIndex, 0), ' ', context.TextCursor.ColumnIndex);
            context.TextCursor.Kind = CursorPositionKind.Real;
        }
        context.Text.AppendChar(context.TextCursor.Position, ch);
        context.TextCursor.MoveRight();
        var newCursorPosition = context.TextCursor.Position;
        UpdateTokensForLines(context, context.TextCursor.LineIndex, 1);

        return new(oldCursorPosition, newCursorPosition, selectionStart, selectionEnd, deletedSelectedText, ch);
    }
}
