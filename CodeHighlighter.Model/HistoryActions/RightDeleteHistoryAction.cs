using CodeHighlighter.InputActions;
using CodeHighlighter.Model;

namespace CodeHighlighter.HistoryActions;

internal class RightDeleteHistoryAction : TextHistoryAction<DeleteResult>
{
    public RightDeleteHistoryAction(InputActionContext context) : base(context)
    {
    }

    public override bool Do()
    {
        Result = RightDeleteInputAction.Instance.Do(_context);
        if (Result.HasDeleted) _context.CodeTextBox.InvalidateVisual();

        return Result.HasDeleted;
    }

    public override void Undo()
    {
        ResetSelection();
        SetCursorToEndPosition();
        var deletedSelectedText = Result.DeletedSelectedText != "" ? Result.DeletedSelectedText : Result.CharCharDeleteResult.DeletedChar.ToString();
        InsertTextInputAction.Instance.Do(_context, deletedSelectedText);
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
        RightDeleteInputAction.Instance.Do(_context);
        _context.CodeTextBox.InvalidateVisual();
    }
}
