using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal interface IMoveSelectedLinesUpInputAction
{
    MoveSelectedLinesResult Do(IInputActionContext context);
}

internal class MoveSelectedLinesUpInputAction : InputAction, IMoveSelectedLinesUpInputAction
{
    public MoveSelectedLinesResult Do(IInputActionContext context)
    {
        var result = MoveSelectedLinesUp(context);
        context.Viewport.CorrectByCursorPosition();

        return result;
    }

    private MoveSelectedLinesResult MoveSelectedLinesUp(IInputActionContext context)
    {
        var oldCursorPosition = context.TextCursor.Position;
        var (selectionStart, selectionEnd) = context.TextSelection.GetSortedPositions();
        int sourceIndex, destinationIndex;
        if (context.TextSelection.IsExist)
        {
            sourceIndex = selectionStart.LineIndex - 1;
            if (sourceIndex < 0) return new(oldCursorPosition, oldCursorPosition, selectionStart, selectionEnd);
            destinationIndex = selectionEnd.LineIndex;
        }
        else
        {
            sourceIndex = context.TextCursor.LineIndex;
            destinationIndex = sourceIndex - 1;
            if (destinationIndex < 0) return new(oldCursorPosition, oldCursorPosition, selectionStart, selectionEnd);
        }
        context.Text.ReplaceLines(sourceIndex, destinationIndex);
        context.Tokens.ReplaceLines(sourceIndex, destinationIndex);
        context.TextCursor.MoveUp();
        context.TextSelection.StartPosition = new(context.TextSelection.StartPosition.LineIndex - 1, context.TextSelection.StartPosition.ColumnIndex);
        context.TextSelection.EndPosition = new(context.TextSelection.EndPosition.LineIndex - 1, context.TextSelection.EndPosition.ColumnIndex);
        var newCursorPosition = context.TextCursor.Position;

        return new(oldCursorPosition, newCursorPosition, selectionStart, selectionEnd);
    }
}
