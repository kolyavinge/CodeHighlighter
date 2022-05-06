using CodeHighlighter.Input;

namespace CodeHighlighter.Commands
{
    internal class MoveCursorEndLineCommand : InputCommand
    {
        public MoveCursorEndLineCommand(InputCommandContext context) : base(context) { }

        public override void Execute(object parameter)
        {
            _context.Model.MoveCursorEndLine();
            _context.Viewport.CorrectByCursorPosition(_context.Model.TextCursor);
            _context.TextBox.InvalidateVisual();
        }
    }
}
