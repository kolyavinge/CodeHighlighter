﻿using CodeHighlighter.Core;

namespace CodeHighlighter.InputActions;

internal interface IMoveSelectedLinesDownInputAction
{
    MoveSelectedLinesResult Do(IInputActionContext context);
}

internal class MoveSelectedLinesDownInputAction : InputAction, IMoveSelectedLinesDownInputAction
{
    public MoveSelectedLinesResult Do(IInputActionContext context)
    {
        var result = MoveSelectedLinesDown(context);
        context.CursorPositionCorrector.CorrectPosition();

        return result;
    }

    private MoveSelectedLinesResult MoveSelectedLinesDown(IInputActionContext context)
    {
        var oldCursorPosition = context.TextCursor.Position;
        var (selectionStart, selectionEnd) = context.TextSelection.GetSortedPositions();
        int sourceIndex, destinationIndex;
        if (context.TextSelection.IsExist)
        {
            sourceIndex = selectionEnd.LineIndex + 1;
            if (sourceIndex >= context.Text.LinesCount) return new(oldCursorPosition, oldCursorPosition, selectionStart, selectionEnd);
            destinationIndex = selectionStart.LineIndex;
        }
        else
        {
            sourceIndex = context.TextCursor.LineIndex;
            destinationIndex = sourceIndex + 1;
            if (destinationIndex >= context.Text.LinesCount) return new(oldCursorPosition, oldCursorPosition, selectionStart, selectionEnd);
        }
        context.Text.ReplaceLines(sourceIndex, destinationIndex);
        context.Tokens.ReplaceLines(sourceIndex, destinationIndex);
        context.TextCursor.MoveDown();
        context.TextSelection.StartPosition = new(context.TextSelection.StartPosition.LineIndex + 1, context.TextSelection.StartPosition.ColumnIndex);
        context.TextSelection.EndPosition = new(context.TextSelection.EndPosition.LineIndex + 1, context.TextSelection.EndPosition.ColumnIndex);
        var newCursorPosition = context.TextCursor.Position;

        return new(oldCursorPosition, newCursorPosition, selectionStart, selectionEnd);
    }
}
