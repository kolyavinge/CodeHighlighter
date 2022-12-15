using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal interface IInsertTextInputAction
{
    InsertTextResult Do(InputActionContext context, string insertedText);
}

[InputAction]
internal class InsertTextInputAction : InputAction, IInsertTextInputAction
{
    public InsertTextResult Do(InputActionContext context, string insertedText)
    {
        var insertResult = InsertText(context, insertedText);
        context.Viewport.CorrectByCursorPosition();
        context.Viewport.UpdateScrollbarsMaximumValues();
        context.TextEvents.RaiseTextChanged();

        return insertResult;
    }

    private InsertTextResult InsertText(InputActionContext context, string text)
    {
        var insertedText = new Text(text);
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
        var insertResult = context.Text.Insert(context.TextCursor.Position, insertedText);
        context.TextCursor.MoveTo(insertResult.EndPosition);
        var newCursorPosition = context.TextCursor.Position;
        if (insertedText.LinesCount > 1)
        {
            context.Tokens.InsertEmptyLines(insertResult.StartPosition.LineIndex, insertedText.LinesCount - 1);
        }
        UpdateTokensForLines(context, insertResult.StartPosition.LineIndex, insertedText.LinesCount);

        return new(oldCursorPosition, newCursorPosition, selectionStart, selectionEnd, deletedSelectedText, insertResult.StartPosition, insertResult.EndPosition, text, insertResult.HasInserted);
    }
}
