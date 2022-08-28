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
        Result = InsertTextInputAction.Instance.Do(_context, _insertedText);
        if (Result.HasInserted) _context.CodeTextBox.InvalidateVisual();

        return Result.HasInserted;
    }

    public override void Undo()
    {
        ResetSelection();
        if (Result.IsSelectionExist)
        {
            _context.TextSelection.Set(
                new(Result.SelectionStart.LineIndex, Result.SelectionStart.ColumnIndex),
                new(Result.SelectionStart.LineIndex, Result.SelectionStart.ColumnIndex + _insertedText.Length));

            InsertTextInputAction.Instance.Do(_context, Result.DeletedSelectedText);
        }
        else
        {
            _context.TextSelection.Set(Result.InsertStartPosition, Result.InsertEndPosition);
            LeftDeleteInputAction.Instance.Do(_context);
        }
        ClearLineIfVirtualCursor();
        SetCursorToStartPosition();
        _context.CodeTextBox.InvalidateVisual();
    }

    public override void Redo()
    {
        if (Result.IsSelectionExist)
        {
            RestoreSelection();
        }
        else
        {
            ResetSelection();
            SetCursorToStartPosition();
        }
        InsertTextInputAction.Instance.Do(_context, _insertedText);
        _context.CodeTextBox.InvalidateVisual();
    }
}
