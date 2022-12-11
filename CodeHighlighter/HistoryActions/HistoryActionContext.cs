using CodeHighlighter.CodeProvidering;
using CodeHighlighter.InputActions;
using CodeHighlighter.Model;

namespace CodeHighlighter.HistoryActions;

internal class HistoryActionContext : InputActionContext
{
    public ICodeTextBox CodeTextBox;

    public HistoryActionContext(
        ICodeProvider codeProvider,
        Text text,
        TextCursor textCursor,
        TextMeasures textMeasures,
        TextSelection textSelection,
        TextSelector textSelector,
        Tokens tokens,
        TokensColors tokenColors,
        Viewport viewport,
        Action raiseTextChanged,
        Action raiseTextSet)
        : base(codeProvider, text, textCursor, textMeasures, textSelection, textSelector, tokens, tokenColors, viewport, raiseTextChanged, raiseTextSet)
    {
        CodeTextBox = DummyCodeTextBox.Instance;
    }
}
