using CodeHighlighter.Model;

namespace CodeHighlighter.Input;

internal class InputCommandContext
{
    public ICodeTextBox TextBox { get; }
    public CodeTextBoxModel Model { get; }
    public Viewport Viewport { get; }
    public IViewportContext ViewportContext { get; }
    public TextMeasures TextMeasures { get; }

    public InputCommandContext(
        ICodeTextBox textBox,
        CodeTextBoxModel model,
        Viewport viewport,
        IViewportContext viewportContext,
        TextMeasures textMeasures)
    {
        TextBox = textBox;
        Model = model;
        Viewport = viewport;
        ViewportContext = viewportContext;
        TextMeasures = textMeasures;
    }
}
