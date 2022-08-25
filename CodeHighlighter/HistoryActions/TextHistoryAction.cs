using CodeHighlighter.Model;

namespace CodeHighlighter.HistoryActions;

internal abstract class TextHistoryAction<TEditTextResult> : HistoryAction where TEditTextResult : EditTextResult
{
    protected readonly HistoryActionContext _context;
    protected TEditTextResult? _result;

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
        _context.InputModel.MoveCursorTo(_result!.OldCursorPosition);
    }

    protected void SetCursorToEndPosition()
    {
        _context.InputModel.MoveCursorTo(_result!.NewCursorPosition);
    }

    protected void RestoreSelection()
    {
        _context.TextSelection.Set(_result!.SelectionStart, _result.SelectionEnd);
    }

    protected void RestoreSelectionLineUp()
    {
        _context.TextSelection.Set(
            new(_result!.SelectionStart.LineIndex - 1, _result!.SelectionStart.ColumnIndex),
            new(_result.SelectionEnd.LineIndex - 1, _result.SelectionEnd.ColumnIndex));
    }

    protected void RestoreSelectionLineDown()
    {
        _context.TextSelection.Set(
            new(_result!.SelectionStart.LineIndex + 1, _result!.SelectionStart.ColumnIndex),
            new(_result.SelectionEnd.LineIndex + 1, _result.SelectionEnd.ColumnIndex));
    }

    protected void ClearLineIfVirtualCursor()
    {
        if (_result!.IsSelectionExist && _result!.SelectionStart.Kind == CursorPositionKind.Virtual)
        {
            _context.Text.GetLine(_result!.SelectionStart.LineIndex).Clear();
        }
        else if (!_result!.IsSelectionExist && _result!.OldCursorPosition.Kind == CursorPositionKind.Virtual)
        {
            _context.Text.GetLine(_result!.OldCursorPosition.LineIndex).Clear();
        }
    }
}
