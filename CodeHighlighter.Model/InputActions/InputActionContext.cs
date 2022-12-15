using CodeHighlighter.CodeProvidering;
using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal class InputActionContext
{
    public readonly ICodeProvider CodeProvider;
    public readonly IText Text;
    public readonly ITextCursor TextCursor;
    public readonly ITextMeasures TextMeasures;
    public readonly ITextSelection TextSelection;
    public readonly ITextSelector TextSelector;
    public readonly ITokens Tokens;
    public readonly ITokensColors TokenColors;
    public IViewport Viewport;
    public readonly Action RaiseTextChanged;
    public readonly Action RaiseTextSet;
    public ICodeTextBox CodeTextBox;

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
        Action raiseTextChanged,
        Action raiseTextSet)
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
        RaiseTextChanged = raiseTextChanged;
        RaiseTextSet = raiseTextSet;
        CodeTextBox = DummyCodeTextBox.Instance;
    }
}
