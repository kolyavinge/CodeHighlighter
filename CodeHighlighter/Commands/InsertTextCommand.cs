using CodeHighlighter.Input;

namespace CodeHighlighter.Commands
{
    public class InsertTextCommandParameter
    {
        public string InsertedText { get; }

        public InsertTextCommandParameter(string insertedText)
        {
            InsertedText = insertedText;
        }
    }

    internal class InsertTextCommand : InputCommand
    {
        public InsertTextCommand(InputCommandContext context) : base(context) { }

        public override void Execute(object parameter)
        {
            var p = parameter as InsertTextCommandParameter;
            _context.Model.InsertText(p.InsertedText);
            _context.Viewport.CorrectByCursorPosition(_context.Model.TextCursor);
            _context.TextBox.InvalidateVisual();
        }
    }
}
