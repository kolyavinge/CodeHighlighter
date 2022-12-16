using CodeHighlighter.CodeProvidering;
using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal interface IInputActionContext
{
    ICodeProvider CodeProvider { get; }
    ICodeTextBox CodeTextBox { get; set; }
    IText Text { get; }
    ITextCursor TextCursor { get; }
    ITextEvents TextEvents { get; }
    ITextMeasuresInternal TextMeasures { get; }
    ITextSelectionInternal TextSelection { get; }
    ITextSelector TextSelector { get; }
    ITokensColors TokenColors { get; }
    ITokens Tokens { get; }
    IViewportInternal Viewport { get; set; }
}

internal class InputActionContext : IInputActionContext
{
    public ICodeProvider CodeProvider { get; }
    public IText Text { get; }
    public ITextCursor TextCursor { get; }
    public ITextMeasuresInternal TextMeasures { get; }
    public ITextSelectionInternal TextSelection { get; }
    public ITextSelector TextSelector { get; }
    public ITokens Tokens { get; }
    public ITokensColors TokenColors { get; }
    public IViewportInternal Viewport { get; set; }
    public ITextEvents TextEvents { get; }
    public ICodeTextBox CodeTextBox { get; set; }

    public InputActionContext(
        ICodeProvider codeProvider,
        IText text,
        ITextCursor textCursor,
        ITextMeasuresInternal textMeasures,
        ITextSelectionInternal textSelection,
        ITextSelector textSelector,
        ITokens tokens,
        ITokensColors tokenColors,
        IViewportInternal viewport,
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
