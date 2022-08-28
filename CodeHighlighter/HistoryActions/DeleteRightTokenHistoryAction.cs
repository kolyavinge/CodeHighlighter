using CodeHighlighter.InputActions;
using CodeHighlighter.Model;

namespace CodeHighlighter.HistoryActions;

internal class DeleteRightTokenHistoryAction : TextHistoryAction<DeleteTokenResult>
{
    public DeleteRightTokenHistoryAction(HistoryActionContext context) : base(context)
    {
    }

    public override bool Do()
    {
        Result = DeleteRightTokenInputAction.Instance.Do(_context);
        if (Result.HasDeleted) _context.CodeTextBox.InvalidateVisual();

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
        RestoreSelection();
        DeleteRightTokenInputAction.Instance.Do(_context);
        _context.CodeTextBox.InvalidateVisual();
    }
}
