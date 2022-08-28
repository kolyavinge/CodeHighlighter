using CodeHighlighter.InputActions;
using CodeHighlighter.Model;

namespace CodeHighlighter.HistoryActions;

internal class RightDeleteHistoryAction : TextHistoryAction<DeleteResult>
{
    public RightDeleteHistoryAction(HistoryActionContext context) : base(context)
    {
    }

    public override bool Do()
    {
        _result = RightDeleteInputAction.Instance.Do(_context);
        if (_result.HasDeleted) _context.CodeTextBox.InvalidateVisual();

        return _result.HasDeleted;
    }

    public override void Undo()
    {
        ResetSelection();
        SetCursorToEndPosition();
        var deletedSelectedText = _result!.DeletedSelectedText != "" ? _result!.DeletedSelectedText : _result.CharCharDeleteResult.DeletedChar.ToString();
        InsertTextInputAction.Instance.Do(_context, deletedSelectedText);
        ClearLineIfVirtualCursor();
        SetCursorToStartPosition();
        _context.CodeTextBox.InvalidateVisual();
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
        RightDeleteInputAction.Instance.Do(_context);
        _context.CodeTextBox.InvalidateVisual();
    }
}
