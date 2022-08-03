using CodeHighlighter.InputActions;
using CodeHighlighter.Model;

namespace CodeHighlighter.HistoryActions;

internal class ToLowerCaseHistoryAction : TextHistoryAction<CaseResult>
{
    public ToLowerCaseHistoryAction(InputActionContext context) : base(context)
    {
    }

    public override bool Do()
    {
        _result = ToLowerCaseInputAction.Instance.Do(_context);
        return _result.HasChanged;
    }

    public override void Undo()
    {
        RestoreSelection();
        InsertTextInputAction.Instance.Do(_context, _result!.DeletedSelectedText);
        SetCursorToStartPosition();
    }

    public override void Redo()
    {
        RestoreSelection();
        ToLowerCaseInputAction.Instance.Do(_context);
    }
}
