using CodeHighlighter.Input;

namespace CodeHighlighter.Commands;

internal class ScrollLineUpCommand : InputCommand
{
    public ScrollLineUpCommand(InputCommandContext context) : base(context) { }

    public override void Execute(object parameter)
    {
        _context.Viewport.ScrollLineUp();
        _context.TextBox.InvalidateVisual();
    }
}
