using CodeHighlighter.Input;

namespace CodeHighlighter.Commands;

internal class MoveSelectedLinesUpCommand : InputCommand
{
    public MoveSelectedLinesUpCommand(InputCommandContext context) : base(context) { }

    public override void Execute(object parameter)
    {
        _context.Model.MoveSelectedLinesUp();
        _context.Viewport.CorrectByCursorPosition(_context.Model.TextCursor);
        _context.TextBox.InvalidateVisual();
    }
}
