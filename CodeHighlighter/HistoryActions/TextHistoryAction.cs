using CodeHighlighter.InputActions;
using CodeHighlighter.Model;

namespace CodeHighlighter.HistoryActions;

internal abstract class TextHistoryAction<TEditTextResult> : HistoryAction where TEditTextResult : EditTextResult
{
    protected readonly InputActionContext _context;
    protected TEditTextResult? _result;

    protected TextHistoryAction(InputActionContext context)
    {
        _context = context;
    }

    protected void ResetSelection()
    {
        _context.TextSelection.Reset();
    }

    protected void SetCursorToStartPosition()
    {
        _context.InputModel.MoveCursorTo(_result!.OldCursorPosition);
    }

    protected void SetCursorToEndPosition()
    {
        _context.InputModel.MoveCursorTo(_result!.NewCursorPosition);
    }

    protected void RestoreSelection()
    {
        _context.TextSelection.InProgress = false;
        _context.TextSelection.StartCursorLineIndex = _result!.SelectionStart.LineIndex;
        _context.TextSelection.StartCursorColumnIndex = _result!.SelectionStart.ColumnIndex;
        _context.TextSelection.EndCursorLineIndex = _result.SelectionEnd.LineIndex;
        _context.TextSelection.EndCursorColumnIndex = _result.SelectionEnd.ColumnIndex;
    }

    protected void RestoreSelectionLineUp()
    {
        _context.TextSelection.InProgress = false;
        _context.TextSelection.StartCursorLineIndex = _result!.SelectionStart.LineIndex - 1;
        _context.TextSelection.StartCursorColumnIndex = _result!.SelectionStart.ColumnIndex;
        _context.TextSelection.EndCursorLineIndex = _result.SelectionEnd.LineIndex - 1;
        _context.TextSelection.EndCursorColumnIndex = _result.SelectionEnd.ColumnIndex;
    }

    protected void RestoreSelectionLineDown()
    {
        _context.TextSelection.InProgress = false;
        _context.TextSelection.StartCursorLineIndex = _result!.SelectionStart.LineIndex + 1;
        _context.TextSelection.StartCursorColumnIndex = _result!.SelectionStart.ColumnIndex;
        _context.TextSelection.EndCursorLineIndex = _result.SelectionEnd.LineIndex + 1;
        _context.TextSelection.EndCursorColumnIndex = _result.SelectionEnd.ColumnIndex;
    }
}
