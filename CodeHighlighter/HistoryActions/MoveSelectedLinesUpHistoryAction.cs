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
        _result = MoveSelectedLinesUpInputAction.Instance.Do(_context);
        return _result.HasMoved;
    }

    public override void Undo()
    {
        if (_result!.IsSelectionExist)
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
        MoveSelectedLinesUpInputAction.Instance.Do(_context);
    }
}
