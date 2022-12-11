using CodeHighlighter.CodeProvidering;
using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal class InputActionContext
{
    public readonly ICodeProvider CodeProvider;
    public readonly Text Text;
    public readonly TextCursor TextCursor;
    public readonly TextMeasures TextMeasures;
    public readonly TextSelection TextSelection;
    public readonly TextSelector TextSelector;
    public readonly Tokens Tokens;
    public readonly TokensColors TokenColors;
    public Viewport Viewport;
    public IViewportContext ViewportContext;
    public readonly Action RaiseTextChanged;
    public readonly Action RaiseTextSet;

    public InputActionContext(
        ICodeProvider codeProvider,
        Text text,
        TextCursor textCursor,
        TextMeasures textMeasures,
        TextSelection textSelection,
        TextSelector textSelector,
        Tokens tokens,
        TokensColors tokenColors,
        Viewport viewport,
        IViewportContext viewportContext,
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
        ViewportContext = viewportContext;
        RaiseTextChanged = raiseTextChanged;
        RaiseTextSet = raiseTextSet;
    }
}
