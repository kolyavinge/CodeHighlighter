using CodeHighlighter.Model;

namespace CodeHighlighter.Input
{
    internal class InputCommandContext
    {
        public ICodeTextBox TextBox { get; }
        public CodeTextBoxModel Model { get; }
        public Viewport Viewport { get; }

        public InputCommandContext(ICodeTextBox textBox, CodeTextBoxModel model, Viewport viewport)
        {
            TextBox = textBox;
            Model = model;
            Viewport = viewport;
        }
    }
}
