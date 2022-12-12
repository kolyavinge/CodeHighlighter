using CodeHighlighter.InputActions;
using CodeHighlighter.Model;

namespace CodeHighlighter.HistoryActions;

internal class AppendCharHistoryAction : TextHistoryAction<AppendCharResult>
{
    private readonly char _appendedChar;

    public AppendCharHistoryAction(InputActionContext context, char appendedChar) : base(context)
    {
        _appendedChar = appendedChar;
    }

    public override bool Do()
    {
        Result = AppendCharInputAction.Instance.Do(_context, _appendedChar);
        _context.CodeTextBox.InvalidateVisual();

        return true;
    }

    public override void Undo()
    {
        ResetSelection();
        if (Result.IsSelectionExist)
        {
            _context.TextSelection.Set(
                new(Result.SelectionStart.LineIndex, Result.SelectionStart.ColumnIndex),
                new(Result.SelectionStart.LineIndex, Result.SelectionStart.ColumnIndex + 1));

            InsertTextInputAction.Instance.Do(_context, Result.DeletedSelectedText);
        }
        else
        {
            SetCursorToStartPosition();
            RightDeleteInputAction.Instance.Do(_context);
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
        AppendCharInputAction.Instance.Do(_context, _appendedChar);
        _context.CodeTextBox.InvalidateVisual();
    }
}
