using CodeHighlighter.CodeProvidering;
using CodeHighlighter.InputActions;
using CodeHighlighter.Model;

namespace CodeHighlighter.Tests.Model;

internal class BaseCodeTextBoxModelIntegration
{
    protected CodeTextBoxModel MakeModel()
    {
        var text = new Text();
        var textCursor = new TextCursor(text);
        var tokens = new Tokens();
        var tokensColors = new TokensColors();
        var textMeasures = new TextMeasures();
        var history = new History();
        var linesDecoration = new LinesDecorationCollection();
        var textSelection = new TextSelection(text);
        var textSelector = new TextSelector(text, textCursor, textSelection);
        var viewport = new Viewport(text, new DummyViewportContext(), textCursor, textMeasures);
        var bracketsHighlighter = new BracketsHighlighter(text, "");
        var textEvents = new TextEvents(text);
        var codeProvider = new SqlCodeProvider();
        var inputActionsFactory = new TestInputActionsFactory();
        var inputActionContext = new InputActionContext(
            codeProvider,
            text,
            textCursor,
            textMeasures,
            textSelection,
            textSelector,
            tokens,
            tokensColors,
            viewport,
            textEvents);
        var historyActionsFactory = new TestHistoryActionsFactory(inputActionsFactory, inputActionContext);

        return new CodeTextBoxModel(
            text,
            textCursor,
            tokens,
            tokensColors,
            textMeasures,
            history,
            linesDecoration,
            textSelection,
            textSelector,
            viewport,
            bracketsHighlighter,
            textEvents,
            codeProvider,
            inputActionsFactory,
            historyActionsFactory,
            inputActionContext,
            new CodeTextBoxModelAdditionalParams());
    }
}
