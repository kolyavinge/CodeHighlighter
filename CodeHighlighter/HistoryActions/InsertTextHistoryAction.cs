using CodeHighlighter.InputActions;
using CodeHighlighter.Model;

namespace CodeHighlighter.HistoryActions;

internal class InsertTextHistoryAction : TextHistoryAction<InsertTextResult>
{
    private readonly string _insertedText;

    public InsertTextHistoryAction(InputActionContext context, string insertedText) : base(context)
    {
        _insertedText = insertedText;
    }

    public override bool Do()
    {
        _result = InsertTextInputAction.Instance.Do(_context, _insertedText);
        return _result.HasInserted;
    }

    public override void Undo()
    {
        ResetSelection();
        if (_result!.IsSelectionExist)
        {
            _context.TextSelection.Set(_result.SelectionStart, new(_result.SelectionStart.LineIndex, _result.SelectionStart.ColumnIndex + _insertedText.Length));
            InsertTextInputAction.Instance.Do(_context, _result.DeletedSelectedText);
        }
        else
        {
            _context.TextSelection.Set(_result.InsertStartPosition, _result.InsertEndPosition);
            InsertTextInputAction.Instance.Do(_context, "");
        }
        SetCursorToStartPosition();
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
    }
}
