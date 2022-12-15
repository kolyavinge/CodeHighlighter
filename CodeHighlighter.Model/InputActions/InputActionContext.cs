using CodeHighlighter.CodeProvidering;
using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

public interface IInputActionContext
{
    ICodeProvider CodeProvider { get; }
    ICodeTextBox CodeTextBox { get; set; }
    IText Text { get; }
    ITextCursor TextCursor { get; }
    ITextEvents TextEvents { get; }
    ITextMeasures TextMeasures { get; }
    ITextSelection TextSelection { get; }
    ITextSelector TextSelector { get; }
    ITokensColors TokenColors { get; }
    ITokens Tokens { get; }
    IViewport Viewport { get; set; }
}

internal class InputActionContext : IInputActionContext
{
    public ICodeProvider CodeProvider { get; }
    public IText Text { get; }
    public ITextCursor TextCursor { get; }
    public ITextMeasures TextMeasures { get; }
    public ITextSelection TextSelection { get; }
    public ITextSelector TextSelector { get; }
    public ITokens Tokens { get; }
    public ITokensColors TokenColors { get; }
    public IViewport Viewport { get; set; }
    public ITextEvents TextEvents { get; }
    public ICodeTextBox CodeTextBox { get; set; }

    public InputActionContext(
        ICodeProvider codeProvider,
        IText text,
        ITextCursor textCursor,
        ITextMeasures textMeasures,
        ITextSelection textSelection,
        ITextSelector textSelector,
        ITokens tokens,
        ITokensColors tokenColors,
        IViewport viewport,
        ITextEvents textEvents)
    {
        CodeProvider = codeProvider;
        Text = text;
        TextCursor = textCursor;
        TextMeasures = textMeasures;
        TextSelection = textSelection;
        TextSelector = textSelector;
        Tokens = tokens;
        TokenColors = tokenColors;
        Viewport = viewport;
        TextEvents = textEvents;
        CodeTextBox = DummyCodeTextBox.Instance;
    }
}
