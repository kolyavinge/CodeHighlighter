using CodeHighlighter.InputActions;
using CodeHighlighter.Model;

namespace CodeHighlighter.HistoryActions;

internal class InsertTextHistoryAction : TextHistoryAction<InsertTextResult>
{
    private readonly string _insertedText;

    public InsertTextHistoryAction(HistoryActionContext context, string insertedText) : base(context)
    {
        _insertedText = insertedText;
    }

    public override bool Do()
    {
        _result = InsertTextInputAction.Instance.Do(_context, _insertedText);
        if (_result.HasInserted) _context.CodeTextBox?.InvalidateVisual();

        return _result.HasInserted;
    }

    public override void Undo()
    {
        ResetSelection();
        if (_result!.IsSelectionExist)
        {
            _context.TextSelection.Set(
                new(_result.SelectionStart.LineIndex, _result.SelectionStart.ColumnIndex),
                new(_result.SelectionStart.LineIndex, _result.SelectionStart.ColumnIndex + _insertedText.Length));

            InsertTextInputAction.Instance.Do(_context, _result.DeletedSelectedText);
        }
        else
        {
            _context.TextSelection.Set(_result.InsertStartPosition, _result.InsertEndPosition);
            LeftDeleteInputAction.Instance.Do(_context);
        }
        ClearLineIfVirtualCursor();
        SetCursorToStartPosition();
        _context.CodeTextBox?.InvalidateVisual();
    }

    public override void Redo()
    {
        if (_result!.IsSelectionExist)
        {
            RestoreSelection();
        }
        else
        {
            ResetSelection();
            SetCursorToStartPosition();
        }
        InsertTextInputAction.Instance.Do(_context, _insertedText);
        _context.CodeTextBox?.InvalidateVisual();
    }
}
