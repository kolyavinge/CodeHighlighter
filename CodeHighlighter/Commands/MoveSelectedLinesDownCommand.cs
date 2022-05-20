using CodeHighlighter.Input;

namespace CodeHighlighter.Commands;

internal class MoveSelectedLinesDownCommand : InputCommand
{
    public MoveSelectedLinesDownCommand(InputCommandContext context) : base(context) { }

    public override void Execute(object parameter)
    {
        _context.Model.MoveSelectedLinesDown();
        _context.Viewport.CorrectByCursorPosition(_context.Model.TextCursor);
        _context.TextBox.InvalidateVisual();
    }
}
