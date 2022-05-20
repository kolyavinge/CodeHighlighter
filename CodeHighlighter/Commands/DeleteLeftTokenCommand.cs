using CodeHighlighter.Input;

namespace CodeHighlighter.Commands;

internal class DeleteLeftTokenCommand : InputCommand
{
    public DeleteLeftTokenCommand(InputCommandContext context) : base(context) { }

    public override void Execute(object parameter)
    {
        _context.Model.DeleteLeftToken();
        _context.Viewport.CorrectByCursorPosition(_context.Model.TextCursor);
        _context.TextBox.InvalidateVisual();
    }
}
