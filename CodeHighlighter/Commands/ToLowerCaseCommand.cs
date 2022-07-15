using CodeHighlighter.Input;

namespace CodeHighlighter.Commands;

internal class ToLowerCaseCommand : InputCommand
{
    public ToLowerCaseCommand(InputCommandContext context) : base(context) { }

    public override void Execute(object parameter)
    {
        _context.Model.SetSelectedTextCase(TextCase.Lower);
        _context.TextBox.InvalidateVisual();
    }
}
