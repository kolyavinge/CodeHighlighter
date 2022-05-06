using CodeHighlighter.Input;

namespace CodeHighlighter.Commands
{
    internal class MoveToPrevTokenCommand : InputCommand
    {
        public MoveToPrevTokenCommand(InputCommandContext context) : base(context) { }

        public override void Execute(object parameter)
        {
            _context.Model.MoveToPrevToken();
            _context.Viewport.CorrectByCursorPosition(_context.Model.TextCursor);
            _context.TextBox.InvalidateVisual();
        }
    }
}
