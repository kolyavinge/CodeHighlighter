using CodeHighlighter.InputActions;
using CodeHighlighter.Model;

namespace CodeHighlighter.HistoryActions;

internal class DeleteLeftTokenHistoryAction : TextHistoryAction<DeleteTokenResult>
{
    public DeleteLeftTokenHistoryAction(InputActionContext context) : base(context)
    {
    }

    public override bool Do()
    {
        _result = DeleteLeftTokenInputAction.Instance.Do(_context);
        return _result.HasDeleted;
    }

    public override void Undo()
    {
        ResetSelection();
        SetCursorToEndPosition();
        InsertTextInputAction.Instance.Do(_context, _result!.DeletedSelectedText);
        SetCursorToStartPosition();
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
        DeleteLeftTokenInputAction.Instance.Do(_context);
    }
}
