using CodeHighlighter.InputActions;
using CodeHighlighter.Model;

namespace CodeHighlighter.HistoryActions;

internal class DeleteSelectedLinesHistoryAction : TextHistoryAction<DeleteSelectedLinesResult>
{
    public DeleteSelectedLinesHistoryAction(HistoryActionContext context) : base(context)
    {
    }

    public override bool Do()
    {
        Result = DeleteSelectedLinesInputAction.Instance.Do(_context);
        if (Result.HasDeleted || Result.OldCursorPosition.Kind == CursorPositionKind.Virtual) _context.CodeTextBox.InvalidateVisual();

        return Result.HasDeleted;
    }

    public override void Undo()
    {
        ResetSelection();
        if (Result.IsSelectionExist)
        {
            MoveCursorToInputAction.Instance.Do(_context, new(Result.SelectionStart.LineIndex, 0));
        }
        else
        {
            MoveCursorToInputAction.Instance.Do(_context, new(Result.OldCursorPosition.LineIndex, 0));
        }
        InsertTextInputAction.Instance.Do(_context, Result.DeletedSelectedText);
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
        DeleteSelectedLinesInputAction.Instance.Do(_context);
        _context.CodeTextBox.InvalidateVisual();
    }
}
