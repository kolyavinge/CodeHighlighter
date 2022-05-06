using CodeHighlighter.Input;

namespace CodeHighlighter.Commands
{
    internal class MoveToNextTokenCommand : InputCommand
    {
        public MoveToNextTokenCommand(InputCommandContext context) : base(context) { }

        public override void Execute(object parameter)
        {
            _context.Model.MoveToNextToken();
            _context.Viewport.CorrectByCursorPosition(_context.Model.TextCursor);
            _context.TextBox.InvalidateVisual();
        }
    }
}
