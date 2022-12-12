using CodeHighlighter.InputActions;
using CodeHighlighter.Model;

namespace CodeHighlighter.HistoryActions;

internal class AppendNewLineHistoryAction : TextHistoryAction<AppendNewLineResult>
{
    public AppendNewLineHistoryAction(InputActionContext context) : base(context)
    {
    }

    public override bool Do()
    {
        Result = AppendNewLineInputAction.Instance.Do(_context);
        _context.CodeTextBox.InvalidateVisual();

        return true;
    }

    public override void Undo()
    {
        ResetSelection();
        if (Result.IsSelectionExist)
        {
            _context.TextCursor.MoveTo(new(Result.SelectionStart.LineIndex, Result.SelectionStart.ColumnIndex));
        }
        else
        {
            SetCursorToStartPosition();
        }
        RightDeleteInputAction.Instance.Do(_context);
        if (Result.IsSelectionExist)
        {
            InsertTextInputAction.Instance.Do(_context, Result.DeletedSelectedText);
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
        AppendNewLineInputAction.Instance.Do(_context);
        _context.CodeTextBox.InvalidateVisual();
    }
}
