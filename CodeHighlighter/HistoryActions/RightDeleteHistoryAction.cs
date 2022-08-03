using System;
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
        _result = RightDeleteInputAction.Instance.Do(_context);
        return _result.HasDeleted;
    }

    public override void Undo()
    {
        ResetSelection();
        SetCursorToEndPosition();
        var deletedSelectedText = !String.IsNullOrWhiteSpace(_result!.DeletedSelectedText) ? _result!.DeletedSelectedText : _result.CharCharDeleteResult.DeletedChar.ToString();
        InsertTextInputAction.Instance.Do(_context, deletedSelectedText);
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
        RightDeleteInputAction.Instance.Do(_context);
    }
}
