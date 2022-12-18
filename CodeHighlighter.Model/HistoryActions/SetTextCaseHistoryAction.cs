using CodeHighlighter.Infrastructure;
using CodeHighlighter.InputActions;
using CodeHighlighter.Model;

namespace CodeHighlighter.HistoryActions;

internal interface ISetTextCaseHistoryAction : IHistoryAction
{
    ISetTextCaseHistoryAction SetParams(TextCase textCase);
}

internal class SetTextCaseHistoryAction : TextHistoryAction<CaseResult>, ISetTextCaseHistoryAction
{
    private readonly IInputActionsFactory _inputActionsFactory;
    private TextCase? _textCase;

    public SetTextCaseHistoryAction(IInputActionsFactory inputActionsFactory, IInputActionContext context) : base(context)
    {
        _inputActionsFactory = inputActionsFactory;
    }

    public ISetTextCaseHistoryAction SetParams(TextCase textCase)
    {
        _textCase = textCase;
        return this;
    }

    public override bool Do()
    {
        Result = _inputActionsFactory.Get<ISetTextCaseInputAction>().Do(_context, _textCase!.Value);
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
        _inputActionsFactory.Get<ISetTextCaseInputAction>().Do(_context, _textCase!.Value);
        _context.CodeTextBox.InvalidateVisual();
    }
}
