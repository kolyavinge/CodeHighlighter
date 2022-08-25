using CodeHighlighter.InputActions;
using CodeHighlighter.Model;

namespace CodeHighlighter.HistoryActions;

internal class AppendCharHistoryAction : TextHistoryAction<AppendCharResult>
{
    private readonly char _appendedChar;

    public AppendCharHistoryAction(HistoryActionContext context, char appendedChar) : base(context)
    {
        _appendedChar = appendedChar;
    }

    public override bool Do()
    {
        _result = AppendCharInputAction.Instance.Do(_context, _appendedChar);
        _context.CodeTextBox?.InvalidateVisual();

        return true;
    }

    public override void Undo()
    {
        ResetSelection();
        if (_result!.IsSelectionExist)
        {
            _context.TextSelection.Set(
                new(_result.SelectionStart.LineIndex, _result.SelectionStart.ColumnIndex),
                new(_result.SelectionStart.LineIndex, _result.SelectionStart.ColumnIndex + 1));

            InsertTextInputAction.Instance.Do(_context, _result.DeletedSelectedText);
        }
        else
        {
            SetCursorToStartPosition();
            RightDeleteInputAction.Instance.Do(_context);
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
        AppendCharInputAction.Instance.Do(_context, _appendedChar);
        _context.CodeTextBox?.InvalidateVisual();
    }
}
