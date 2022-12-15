using CodeHighlighter.Infrastructure;
using CodeHighlighter.InputActions;
using CodeHighlighter.Model;

namespace CodeHighlighter.HistoryActions;

internal class SetTextHistoryAction : TextHistoryAction<SetTextResult>
{
    private readonly IInputActionsFactory _inputActionsFactory;
    public readonly string _text;

    public SetTextHistoryAction(IInputActionsFactory inputActionsFactory, IInputActionContext context, string text) : base(context)
    {
        _inputActionsFactory = inputActionsFactory;
        _text = text;
    }

    public override bool Do()
    {
        Result = _inputActionsFactory.Get<ISetTextInputAction>().Do(_context, _text);
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
        _inputActionsFactory.Get<ISetTextInputAction>().Do(_context, _text);
        _context.CodeTextBox.InvalidateVisual();
    }
}
