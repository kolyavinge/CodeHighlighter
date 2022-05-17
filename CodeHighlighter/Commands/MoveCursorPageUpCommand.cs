using CodeHighlighter.Input;

namespace CodeHighlighter.Commands;

internal class MoveCursorPageUpCommand : InputCommand
{
    public MoveCursorPageUpCommand(InputCommandContext context) : base(context) { }

    public override void Execute(object parameter)
    {
        _context.Model.MoveCursorPageUp(_context.Viewport.GetLinesCountInViewport());
        _context.Viewport.CorrectByCursorPosition(_context.Model.TextCursor);
        _context.TextBox.InvalidateVisual();
    }
}
