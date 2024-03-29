﻿using CodeHighlighter.Core;

namespace CodeHighlighter.InputActions;

internal interface IInsertTextInputAction
{
    InsertTextResult Do(IInputActionContext context, string insertedText);
}

internal class InsertTextInputAction : InputAction, IInsertTextInputAction
{
    public InsertTextResult Do(IInputActionContext context, string insertedText)
    {
        var result = InsertText(context, insertedText);
        context.CursorPositionCorrector.CorrectPosition();
        context.Viewport.UpdateScrollBarsMaximumValues();
        context.LineFoldsUpdater.UpdateInsertText(result);
        context.TextEvents.RaiseTextChangedAfterInsertText(result);

        return result;
    }

    private InsertTextResult InsertText(IInputActionContext context, string text)
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
