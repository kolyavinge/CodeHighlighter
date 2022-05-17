using CodeHighlighter.Input;

namespace CodeHighlighter.Commands;

internal class NewLineCommand : InputCommand
{
    public NewLineCommand(InputCommandContext context) : base(context) { }

    public override void Execute(object parameter)
    {
        _context.Model.NewLine();
        _context.Viewport.CorrectByCursorPosition(_context.Model.TextCursor);
        _context.TextBox.InvalidateVisual();
    }
}
