using CodeHighlighter.Input;

namespace CodeHighlighter.Commands;

internal class MoveCursorTextEndCommand : InputCommand
{
    public MoveCursorTextEndCommand(InputCommandContext context) : base(context) { }

    public override void Execute(object parameter)
    {
        _context.Model.MoveCursorTextEnd();
        _context.Viewport.CorrectByCursorPosition(_context.Model.TextCursor);
        _context.TextBox.InvalidateVisual();
    }
}
