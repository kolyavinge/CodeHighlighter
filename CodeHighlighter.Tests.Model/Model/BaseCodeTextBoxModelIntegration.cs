﻿using CodeHighlighter.CodeProvidering;
using CodeHighlighter.InputActions;
using CodeHighlighter.Model;

namespace CodeHighlighter.Tests.Model;

internal class BaseCodeTextBoxModelIntegration
{
    protected CodeTextBoxModel MakeModel()
    {
        var codeProvider = new SqlCodeProvider();
        var text = new Text();
        var textCursor = new TextCursor(text);
        var textSelection = new TextSelection(text);
        var textSelector = new TextSelector(text, textCursor, textSelection);
        var textMeasures = new TextMeasures();
        var textEvents = new TextEvents(text);
        var textMeasuresEvents = new TextMeasuresEvents(textMeasures);
        var tokens = new Tokens();
        var tokensColors = new TokensColors();
        var history = new History();
        var linesDecoration = new LinesDecorationCollection();
        var viewport = new Viewport(text, new DummyViewportContext(), textCursor, textMeasures);
        var bracketsHighlighter = new BracketsHighlighter(text, "");
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
            codeProvider,
            text,
            textCursor,
            textSelection,
            textSelector,
            textMeasures,
            textEvents,
            textMeasuresEvents,
            tokens,
            tokensColors,
            history,
            linesDecoration,
            viewport,
            bracketsHighlighter,
            inputActionContext,
            inputActionsFactory,
            historyActionsFactory,
            new());
    }
}
