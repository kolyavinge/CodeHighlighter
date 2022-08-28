using CodeHighlighter.InputActions;
using CodeHighlighter.Model;

namespace CodeHighlighter.HistoryActions;

internal class DeleteLeftTokenHistoryAction : TextHistoryAction<DeleteTokenResult>
{
    public DeleteLeftTokenHistoryAction(HistoryActionContext context) : base(context)
    {
    }

    public override bool Do()
    {
        Result = DeleteLeftTokenInputAction.Instance.Do(_context);
        if (Result.HasDeleted || Result.OldCursorPosition.Kind == CursorPositionKind.Virtual) _context.CodeTextBox.InvalidateVisual();

        return Result.HasDeleted;
    }

    public override void Undo()
    {
        ResetSelection();
        SetCursorToEndPosition();
        InsertTextInputAction.Instance.Do(_context, Result.DeletedSelectedText);
        ClearLineIfVirtualCursor();
        SetCursorToStartPosition();
        _context.CodeTextBox.InvalidateVisual();
    }

    public override void Redo()
    {
        if (!Result.IsSelectionExist && Result.OldCursorPosition.Kind == CursorPositionKind.Virtual)
        {
            ResetSelection();
            SetCursorToStartPosition();
        }
        else
        {
            RestoreSelection();
        }
        DeleteLeftTokenInputAction.Instance.Do(_context);
        _context.CodeTextBox.InvalidateVisual();
    }
}
