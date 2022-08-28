﻿using CodeHighlighter.Model;

namespace CodeHighlighter.HistoryActions;

internal abstract class TextHistoryAction<TEditTextResult> : HistoryAction where TEditTextResult : EditTextResult
{
    private TEditTextResult? _result;
    protected readonly HistoryActionContext _context;

    protected TEditTextResult Result
    {
        get => _result ?? throw new InvalidOperationException();
        set => _result = value;
    }

    protected TextHistoryAction(HistoryActionContext context)
    {
        _context = context;
    }

    protected void ResetSelection()
    {
        _context.TextSelection.Reset();
    }

    protected void SetCursorToStartPosition()
    {
        _context.InputModel.MoveCursorTo(Result.OldCursorPosition);
    }

    protected void SetCursorToEndPosition()
    {
        _context.InputModel.MoveCursorTo(Result.NewCursorPosition);
    }

    protected void RestoreSelection()
    {
        _context.TextSelection.Set(Result.SelectionStart, Result.SelectionEnd);
    }

    protected void RestoreSelectionLineUp()
    {
        _context.TextSelection.Set(
            new(Result.SelectionStart.LineIndex - 1, Result.SelectionStart.ColumnIndex),
            new(Result.SelectionEnd.LineIndex - 1, Result.SelectionEnd.ColumnIndex));
    }

    protected void RestoreSelectionLineDown()
    {
        _context.TextSelection.Set(
            new(Result.SelectionStart.LineIndex + 1, Result.SelectionStart.ColumnIndex),
            new(Result.SelectionEnd.LineIndex + 1, Result.SelectionEnd.ColumnIndex));
    }

    protected void ClearLineIfVirtualCursor()
    {
        if (Result.IsSelectionExist && Result.SelectionStart.Kind == CursorPositionKind.Virtual)
        {
            _context.Text.GetLine(Result.SelectionStart.LineIndex).Clear();
        }
        else if (!Result.IsSelectionExist && Result.OldCursorPosition.Kind == CursorPositionKind.Virtual)
        {
            _context.Text.GetLine(Result.OldCursorPosition.LineIndex).Clear();
        }
    }
}
