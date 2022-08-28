using CodeHighlighter.InputActions;
using CodeHighlighter.Model;

namespace CodeHighlighter.HistoryActions;

internal class MoveSelectedLinesDownHistoryAction : TextHistoryAction<MoveSelectedLinesResult>
{
    public MoveSelectedLinesDownHistoryAction(HistoryActionContext context) : base(context)
    {
    }

    public override bool Do()
    {
        _result = MoveSelectedLinesDownInputAction.Instance.Do(_context);
        if (_result.HasMoved) _context.CodeTextBox.InvalidateVisual();

        return _result.HasMoved;
    }

    public override void Undo()
    {
        if (_result!.IsSelectionExist)
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
        if (_result!.IsSelectionExist)
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
