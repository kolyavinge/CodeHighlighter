using CodeHighlighter.Input;

namespace CodeHighlighter.Commands;

internal class RightDeleteCommand : InputCommand
{
    public RightDeleteCommand(InputCommandContext context) : base(context) { }

    public override void Execute(object parameter)
    {
        _context.Model.RightDelete();
        _context.Viewport.CorrectByCursorPosition(_context.Model.TextCursor);
        _context.TextBox.InvalidateVisual();
    }
}
