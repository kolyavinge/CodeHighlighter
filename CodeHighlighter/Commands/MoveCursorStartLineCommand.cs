using CodeHighlighter.Input;

namespace CodeHighlighter.Commands
{
    internal class MoveCursorStartLineCommand : InputCommand
    {
        public MoveCursorStartLineCommand(InputCommandContext context) : base(context) { }

        public override void Execute(object parameter)
        {
            _context.Model.MoveCursorStartLine();
            _context.Viewport.CorrectByCursorPosition(_context.Model.TextCursor);
            _context.TextBox.InvalidateVisual();
        }
    }
}
