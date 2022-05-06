using CodeHighlighter.Input;

namespace CodeHighlighter.Commands
{
    internal class ScrollLineDownCommand : InputCommand
    {
        public ScrollLineDownCommand(InputCommandContext context) : base(context) { }

        public override void Execute(object parameter)
        {
            _context.Viewport.ScrollLineDown();
            _context.TextBox.InvalidateVisual();
        }
    }
}
