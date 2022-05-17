using CodeHighlighter.Input;

namespace CodeHighlighter.Commands;

internal class MoveCursorDownCommand : InputCommand
{
    public MoveCursorDownCommand(InputCommandContext context) : base(context) { }

    public override void Execute(object parameter)
    {
        _context.Model.MoveCursorDown();
        _context.Viewport.CorrectByCursorPosition(_context.Model.TextCursor);
        _context.TextBox.InvalidateVisual();
    }
}
