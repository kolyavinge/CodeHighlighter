using CodeHighlighter.InputActions;
using CodeHighlighter.Model;

namespace CodeHighlighter.HistoryActions;

internal class SetTextCaseHistoryAction : TextHistoryAction<CaseResult>
{
    private readonly TextCase _textCase;

    public SetTextCaseHistoryAction(InputActionContext context, TextCase textCase) : base(context)
    {
        _textCase = textCase;
    }

    public override bool Do()
    {
        Result = SetTextCaseInputAction.Instance.Do(_context, _textCase);
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
        SetTextCaseInputAction.Instance.Do(_context, _textCase);
        _context.CodeTextBox.InvalidateVisual();
    }
}
