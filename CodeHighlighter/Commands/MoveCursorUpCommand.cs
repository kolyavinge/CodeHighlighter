using CodeHighlighter.Input;

namespace CodeHighlighter.Commands;

internal class MoveCursorUpCommand : InputCommand
{
    public MoveCursorUpCommand(InputCommandContext context) : base(context) { }

    public override void Execute(object parameter)
    {
        _context.Model.MoveCursorUp();
        _context.Viewport.CorrectByCursorPosition(_context.Model.TextCursor);
        _context.TextBox.InvalidateVisual();
    }
}
