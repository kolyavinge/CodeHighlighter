using CodeHighlighter.Input;

namespace CodeHighlighter.Commands;

internal class LeftDeleteCommand : InputCommand
{
    public LeftDeleteCommand(InputCommandContext context) : base(context) { }

    public override void Execute(object parameter)
    {
        _context.Model.LeftDelete();
        _context.Viewport.CorrectByCursorPosition(_context.Model.TextCursor);
        _context.TextBox.InvalidateVisual();
    }
}
