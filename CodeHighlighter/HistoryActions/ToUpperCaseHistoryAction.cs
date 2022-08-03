using CodeHighlighter.InputActions;
using CodeHighlighter.Model;

namespace CodeHighlighter.HistoryActions;

internal class ToUpperCaseHistoryAction : TextHistoryAction<CaseResult>
{
    public ToUpperCaseHistoryAction(InputActionContext context) : base(context)
    {
    }

    public override bool Do()
    {
        _result = ToUpperCaseInputAction.Instance.Do(_context);
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
        ToUpperCaseInputAction.Instance.Do(_context);
    }
}
