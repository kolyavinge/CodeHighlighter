using CodeHighlighter.Input;

namespace CodeHighlighter.Commands;

internal class MoveCursorTextBeginCommand : InputCommand
{
    public MoveCursorTextBeginCommand(InputCommandContext context) : base(context) { }

    public override void Execute(object parameter)
    {
        _context.Model.MoveCursorTextBegin();
        _context.Viewport.CorrectByCursorPosition(_context.Model.TextCursor);
        _context.TextBox.InvalidateVisual();
    }
}
