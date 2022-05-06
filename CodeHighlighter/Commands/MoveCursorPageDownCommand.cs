using CodeHighlighter.Input;

namespace CodeHighlighter.Commands
{
    internal class MoveCursorPageDownCommand : InputCommand
    {
        public MoveCursorPageDownCommand(InputCommandContext context) : base(context) { }

        public override void Execute(object parameter)
        {
            _context.Model.MoveCursorPageDown(_context.Viewport.GetLinesCountInViewport());
            _context.Viewport.CorrectByCursorPosition(_context.Model.TextCursor);
            _context.TextBox.InvalidateVisual();
        }
    }
}
