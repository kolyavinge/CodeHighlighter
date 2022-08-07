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
        _result = ToUpperCaseInputAction.Instance.Do(_context);
        if (_result.HasChanged) _context.CodeTextBox?.InvalidateVisual();

        return _result.HasChanged;
    }

    public override void Undo()
    {
        RestoreSelection();
        InsertTextInputAction.Instance.Do(_context, _result!.DeletedSelectedText);
        SetCursorToStartPosition();
        _context.CodeTextBox?.InvalidateVisual();
    }

    public override void Redo()
    {
        RestoreSelection();
        ToUpperCaseInputAction.Instance.Do(_context);
        _context.CodeTextBox?.InvalidateVisual();
    }
}
