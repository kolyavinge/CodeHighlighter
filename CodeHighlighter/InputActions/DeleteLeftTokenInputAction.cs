using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal class DeleteLeftTokenInputAction : InputAction
{
    public static readonly DeleteLeftTokenInputAction Instance = new();

    public DeleteTokenResult Do(InputActionContext context)
    {
        var result = DeleteLeftToken(context);
        context.Viewport.CorrectByCursorPosition(context.TextCursor);
        context.Viewport.UpdateScrollbarsMaximumValues();
        context.RaiseTextChanged();

        return result;
    }

    private DeleteTokenResult DeleteLeftToken(InputActionContext context)
    {
        var oldCursorPosition = context.TextCursor.Position;
        if (context.TextCursor.Kind == CursorPositionKind.Real)
        {
            if (!context.TextSelection.IsExist)
            {
                var navigator = new TokenNavigator();
                var position = navigator.MoveLeft(context.Text, context.Tokens, context.TextCursor.LineIndex, context.TextCursor.ColumnIndex);
                context.TextSelection.Set(position, new(context.TextCursor.LineIndex, context.TextCursor.ColumnIndex));
            }
            var (selectionStart, selectionEnd) = context.TextSelection.GetSortedPositions();
            var deletedSelectedText = context.TextSelector.GetSelectedText();
            var deleteResult = LeftDeleteInputAction.Instance.Do(context);
            var newCursorPosition = context.TextCursor.Position;

            return new(oldCursorPosition, newCursorPosition, selectionStart, selectionEnd, deletedSelectedText, deleteResult.HasDeleted);
        }
        else
        {
            var deleteResult = LeftDeleteInputAction.Instance.Do(context);
            var newCursorPosition = context.TextCursor.Position;

            return new(oldCursorPosition, newCursorPosition, deleteResult.SelectionStart, deleteResult.SelectionEnd, deleteResult.DeletedSelectedText, deleteResult.HasDeleted);
        }
    }
}
