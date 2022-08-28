using CodeHighlighter.InputActions;
using CodeHighlighter.Model;

namespace CodeHighlighter.HistoryActions;

internal class ToUpperCaseHistoryAction : TextHistoryAction<CaseResult>
{
    public ToUpperCaseHistoryAction(HistoryActionContext context) : base(context)
    {
    }

    public override bool Do()
    {
        Result = ToUpperCaseInputAction.Instance.Do(_context);
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
        ToUpperCaseInputAction.Instance.Do(_context);
        _context.CodeTextBox.InvalidateVisual();
    }
}
