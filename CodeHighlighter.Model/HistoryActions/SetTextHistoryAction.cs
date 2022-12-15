using CodeHighlighter.Infrastructure;
using CodeHighlighter.InputActions;
using CodeHighlighter.Model;

namespace CodeHighlighter.HistoryActions;

internal interface ISetTextHistoryAction : IHistoryAction
{
    ISetTextHistoryAction SetParams(string text);
}

[HistoryAction]
internal class SetTextHistoryAction : TextHistoryAction<SetTextResult>, ISetTextHistoryAction
{
    private readonly IInputActionsFactory _inputActionsFactory;
    public string? _text;

    public SetTextHistoryAction(IInputActionsFactory inputActionsFactory, IInputActionContext context) : base(context)
    {
        _inputActionsFactory = inputActionsFactory;
    }

    public ISetTextHistoryAction SetParams(string text)
    {
        _text = text;
        return this;
    }

    public override bool Do()
    {
        Result = _inputActionsFactory.Get<ISetTextInputAction>().Do(_context, _text!);
        _context.CodeTextBox.InvalidateVisual();

        return true;
    }

    public override void Undo()
    {
        _inputActionsFactory.Get<ISetTextInputAction>().Do(_context, Result.DeletedSelectedText);
        SetCursorToStartPosition();
        _context.CodeTextBox.InvalidateVisual();
    }

    public override void Redo()
    {
        _inputActionsFactory.Get<ISetTextInputAction>().Do(_context, _text!);
        _context.CodeTextBox.InvalidateVisual();
    }
}
