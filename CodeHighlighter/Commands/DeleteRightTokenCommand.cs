using CodeHighlighter.Input;

namespace CodeHighlighter.Commands;

internal class DeleteRightTokenCommand : InputCommand
{
    public DeleteRightTokenCommand(InputCommandContext context) : base(context) { }

    public override void Execute(object parameter)
    {
        _context.Model.DeleteRightToken();
        _context.Viewport.CorrectByCursorPosition(_context.Model.TextCursor);
        _context.TextBox.InvalidateVisual();
    }
}
