using CodeHighlighter.InputActions;
using CodeHighlighter.Model;

namespace CodeHighlighter.HistoryActions;

internal class MoveSelectedLinesDownHistoryAction : TextHistoryAction<MoveSelectedLinesResult>
{
    public MoveSelectedLinesDownHistoryAction(InputActionContext context) : base(context)
    {
    }

    public override bool Do()
    {
        Result = MoveSelectedLinesDownInputAction.Instance.Do(_context);
        if (Result.HasMoved) _context.CodeTextBox.InvalidateVisual();

        return Result.HasMoved;
    }

    public override void Undo()
    {
        if (Result.IsSelectionExist)
        {
            RestoreSelectionLineDown();
        }
        else
        {
            ResetSelection();
            SetCursorToEndPosition();
        }
        MoveSelectedLinesUpInputAction.Instance.Do(_context);
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
        MoveSelectedLinesDownInputAction.Instance.Do(_context);
        _context.CodeTextBox.InvalidateVisual();
    }
}
