using CodeHighlighter.Infrastructure;
using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal interface IDeleteRightTokenInputAction
{
    DeleteTokenResult Do(IInputActionContext context);
}

internal class DeleteRightTokenInputAction : InputAction, IDeleteRightTokenInputAction
{
    private readonly IInputActionsFactory _inputActionsFactory;

    public DeleteRightTokenInputAction(IInputActionsFactory inputActionsFactory)
    {
        _inputActionsFactory = inputActionsFactory;
    }

    public DeleteTokenResult Do(IInputActionContext context)
    {
        var result = DeleteRightToken(context);
        context.CursorPositionCorrector.CorrectPosition();
        context.Viewport.UpdateScrollBarsMaximumValues();
        context.TextEvents.RaiseTextChanged();

        return result;
    }

    private DeleteTokenResult DeleteRightToken(IInputActionContext context)
    {
        var oldCursorPosition = context.TextCursor.Position;
        CursorPosition selectionStart, selectionEnd;
        if (!context.TextSelection.IsExist)
        {
            selectionStart = context.TextCursor.Position;
            if (context.TextCursor.Kind == CursorPositionKind.Virtual)
            {
                context.Text.AppendChar(new(context.TextCursor.LineIndex, 0), ' ', context.TextCursor.ColumnIndex);
                context.TextCursor.Kind = CursorPositionKind.Real;
            }
            var navigator = new TokenNavigator();
            var position = navigator.MoveRight(context.Text, context.Tokens, context.TextCursor.LineIndex, context.TextCursor.ColumnIndex);
            context.TextSelection.Set(position, new(context.TextCursor.LineIndex, context.TextCursor.ColumnIndex));
            selectionEnd = position;
        }
        else
        {
            (selectionStart, selectionEnd) = context.TextSelection.GetSortedPositions();
        }
        var deletedSelectedText = context.TextSelector.GetSelectedText();
        var deleteResult = _inputActionsFactory.Get<IRightDeleteInputAction>().Do(context);
        var newCursorPosition = context.TextCursor.Position;

        return new(oldCursorPosition, newCursorPosition, selectionStart, selectionEnd, deletedSelectedText, deleteResult.HasDeleted);
    }
}
