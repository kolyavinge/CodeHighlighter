using CodeHighlighter.Input;

namespace CodeHighlighter.Commands;

internal class DeleteSelectedLinesCommand : InputCommand
{
    public DeleteSelectedLinesCommand(InputCommandContext context) : base(context) { }

    public override void Execute(object parameter)
    {
        _context.Model.DeleteSelectedLines();
        _context.Viewport.CorrectByCursorPosition(_context.Model.TextCursor);
        _context.TextBox.InvalidateVisual();
    }
}
