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
        _result = DeleteLeftTokenInputAction.Instance.Do(_context);
        if (_result.HasDeleted || _result.OldCursorPosition.Kind == CursorPositionKind.Virtual) _context.CodeTextBox.InvalidateVisual();

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
        if (!_result!.IsSelectionExist && _result!.OldCursorPosition.Kind == CursorPositionKind.Virtual)
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
