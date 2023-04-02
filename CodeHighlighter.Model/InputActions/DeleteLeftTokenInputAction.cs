using CodeHighlighter.Core;
using CodeHighlighter.Infrastructure;

namespace CodeHighlighter.InputActions;

internal interface IDeleteLeftTokenInputAction
{
    DeleteTokenResult Do(IInputActionContext context);
}

internal class DeleteLeftTokenInputAction : InputAction, IDeleteLeftTokenInputAction
{
    private readonly IInputActionsFactory _inputActionsFactory;

    public DeleteLeftTokenInputAction(IInputActionsFactory inputActionsFactory)
    {
        _inputActionsFactory = inputActionsFactory;
    }

    public DeleteTokenResult Do(IInputActionContext context)
    {
        var result = DeleteLeftToken(context);
        context.CursorPositionCorrector.CorrectPosition();
        context.Viewport.UpdateScrollBarsMaximumValues();
        // LineFoldsUpdater.UpdateDeleteToken(result) calls in DeleteLeftToken
        context.TextEvents.RaiseTextChangedAfterDeleteToken(result);

        return result;
    }

    private DeleteTokenResult DeleteLeftToken(IInputActionContext context)
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
            var deleteResult = _inputActionsFactory.Get<ILeftDeleteInputAction>().Do(context);
            var newCursorPosition = context.TextCursor.Position;

            return new(
                oldCursorPosition,
                newCursorPosition,
                selectionStart,
                selectionEnd,
                deletedSelectedText,
                deleteResult.HasDeleted);
        }
        else
        {
            var deleteResult = _inputActionsFactory.Get<ILeftDeleteInputAction>().Do(context);
            var newCursorPosition = context.TextCursor.Position;

            return new(
                oldCursorPosition,
                newCursorPosition,
                deleteResult.SelectionStart,
                deleteResult.SelectionEnd,
                deleteResult.DeletedSelectedText,
                deleteResult.HasDeleted);
        }
    }
}
