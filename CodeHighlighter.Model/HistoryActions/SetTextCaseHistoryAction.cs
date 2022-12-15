using CodeHighlighter.Infrastructure;
using CodeHighlighter.InputActions;
using CodeHighlighter.Model;

namespace CodeHighlighter.HistoryActions;

internal class SetTextCaseHistoryAction : TextHistoryAction<CaseResult>
{
    private readonly IInputActionsFactory _inputActionsFactory;
    private readonly TextCase _textCase;

    public SetTextCaseHistoryAction(IInputActionsFactory inputActionsFactory, IInputActionContext context, TextCase textCase) : base(context)
    {
        _inputActionsFactory = inputActionsFactory;
        _textCase = textCase;
    }

    public override bool Do()
    {
        Result = _inputActionsFactory.Get<ISetTextCaseInputAction>().Do(_context, _textCase);
        if (Result.HasChanged) _context.CodeTextBox.InvalidateVisual();

        return Result.HasChanged;
    }

    public override void Undo()
    {
        RestoreSelection();
        _inputActionsFactory.Get<IInsertTextInputAction>().Do(_context, Result.DeletedSelectedText);
        SetCursorToStartPosition();
        _context.CodeTextBox.InvalidateVisual();
    }

    public override void Redo()
    {
        RestoreSelection();
        _inputActionsFactory.Get<ISetTextCaseInputAction>().Do(_context, _textCase);
        _context.CodeTextBox.InvalidateVisual();
    }
}
