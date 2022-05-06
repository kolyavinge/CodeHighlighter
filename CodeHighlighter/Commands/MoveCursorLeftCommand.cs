using CodeHighlighter.Input;

namespace CodeHighlighter.Commands
{
    internal class MoveCursorLeftCommand : InputCommand
    {
        public MoveCursorLeftCommand(InputCommandContext context) : base(context) { }

        public override void Execute(object parameter)
        {
            _context.Model.MoveCursorLeft();
            _context.Viewport.CorrectByCursorPosition(_context.Model.TextCursor);
            _context.TextBox.InvalidateVisual();
        }
    }
}
