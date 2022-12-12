using CodeHighlighter.InputActions;
using CodeHighlighter.Model;

namespace CodeHighlighter.HistoryActions;

internal class MoveSelectedLinesUpHistoryAction : TextHistoryAction<MoveSelectedLinesResult>
{
    public MoveSelectedLinesUpHistoryAction(InputActionContext context) : base(context)
    {
    }

    public override bool Do()
    {
        Result = MoveSelectedLinesUpInputAction.Instance.Do(_context);
        if (Result.HasMoved) _context.CodeTextBox.InvalidateVisual();

        return Result.HasMoved;
    }

    public override void Undo()
    {
        if (Result.IsSelectionExist)
        {
            RestoreSelectionLineUp();
        }
        else
        {
            ResetSelection();
            SetCursorToEndPosition();
        }
        MoveSelectedLinesDownInputAction.Instance.Do(_context);
        SetCursorToStartPosition();
        RestoreSelection();
        _context.CodeTextBox.InvalidateVisual();
    }

    public override void Redo()
    {
        SetCursorToStartPosition();
        if (Result.IsSelectionExist)
        {
            RestoreSelection();
        }
        else
        {
            ResetSelection();
            SetCursorToStartPosition();
        }
        MoveSelectedLinesUpInputAction.Instance.Do(_context);
        _context.CodeTextBox.InvalidateVisual();
    }
}
