using CodeHighlighter.Input;

namespace CodeHighlighter.Commands
{
    internal class MoveCursorRightCommand : InputCommand
    {
        public MoveCursorRightCommand(InputCommandContext context) : base(context) { }

        public override void Execute(object parameter)
        {
            _context.Model.MoveCursorRight();
            _context.Viewport.CorrectByCursorPosition(_context.Model.TextCursor);
            _context.TextBox.InvalidateVisual();
        }
    }
}
