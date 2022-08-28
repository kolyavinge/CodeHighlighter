using CodeHighlighter.InputActions;
using CodeHighlighter.Model;

namespace CodeHighlighter.HistoryActions;

internal class ToLowerCaseHistoryAction : TextHistoryAction<CaseResult>
{
    public ToLowerCaseHistoryAction(HistoryActionContext context) : base(context)
    {
    }

    public override bool Do()
    {
        Result = ToLowerCaseInputAction.Instance.Do(_context);
        if (Result.HasChanged) _context.CodeTextBox.InvalidateVisual();

        return Result.HasChanged;
    }

    public override void Undo()
    {
        RestoreSelection();
        InsertTextInputAction.Instance.Do(_context, Result.DeletedSelectedText);
        SetCursorToStartPosition();
        _context.CodeTextBox.InvalidateVisual();
    }

    public override void Redo()
    {
        RestoreSelection();
        ToLowerCaseInputAction.Instance.Do(_context);
        _context.CodeTextBox.InvalidateVisual();
    }
}
