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
        _result = DeleteRightTokenInputAction.Instance.Do(_context);
        if (_result.HasDeleted) _context.CodeTextBox.InvalidateVisual();

        return _result.HasDeleted;
    }

    public override void Undo()
    {
        ResetSelection();
        SetCursorToEndPosition();
        InsertTextInputAction.Instance.Do(_context, _result!.DeletedSelectedText);
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
