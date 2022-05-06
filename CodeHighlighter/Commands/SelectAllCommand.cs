using CodeHighlighter.Input;

namespace CodeHighlighter.Commands
{
    internal class SelectAllCommand : InputCommand
    {
        public SelectAllCommand(InputCommandContext context) : base(context) { }

        public override void Execute(object parameter)
        {
            _context.Model.SelectAll();
            _context.Viewport.CorrectByCursorPosition(_context.Model.TextCursor);
            _context.TextBox.InvalidateVisual();
        }
    }
}
