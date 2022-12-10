using CodeHighlighter.CodeProvidering;
using CodeHighlighter.InputActions;
using CodeHighlighter.Model;

namespace CodeHighlighter.HistoryActions;

internal class HistoryActionContext : InputActionContext
{
    public ICodeTextBox CodeTextBox;

    public HistoryActionContext(
        ICodeProvider codeProvider,
        InputModel inputModel,
        Text text,
        TextCursor textCursor,
        TextMeasures textMeasures,
        TextSelection textSelection,
        Tokens tokens,
        TokensColors tokenColors,
        Viewport viewport,
        IViewportContext viewportContext,
        Action raiseTextChanged,
        Action raiseTextSet)
        : base(codeProvider, inputModel, text, textCursor, textMeasures, textSelection, tokens, tokenColors, viewport, viewportContext, raiseTextChanged, raiseTextSet)
    {
        CodeTextBox = DummyCodeTextBox.Instance;
    }
}
