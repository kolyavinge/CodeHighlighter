using CodeHighlighter.Ancillary;
using CodeHighlighter.CodeProvidering;
using CodeHighlighter.Core;
using CodeHighlighter.Infrastructure;
using CodeHighlighter.InputActions;
using CodeHighlighter.Model;

namespace CodeHighlighter.Tests.Model;

internal class BaseCodeTextBoxIntegration
{
    protected CodeTextBoxModel MakeModel()
    {
        var codeProvider = new SqlCodeProvider();
        var text = new Text();
        var textLines = new TextLines(text);
        var textSelectionLineConverter = new TextSelectionLineConverter(text);
        var textSelection = new TextSelection(textSelectionLineConverter);
        var gaps = new LineGapCollection();
        var folds = new LineFolds();
        var editTextResultToLinesChangeConverter = new EditTextResultToLinesChangeConverter(new TextLinesChangingLogic());
        var lineFoldsUpdater = new LineFoldsUpdater(folds, editTextResultToLinesChangeConverter);
        var textCursor = new TextCursor(text, new TextCursorPositionCorrector(text, folds));
        var textSelector = new TextSelector(text, textCursor, textSelection);
        var textMeasures = new TextMeasures();
        var textEvents = new TextEvents(text, new TextChangedEventArgsFactory(editTextResultToLinesChangeConverter));
        var textMeasuresEvents = new TextMeasuresEvents(textMeasures);
        var tokens = new Tokens();
        var tokensColors = new TokensColors(codeProvider);
        var history = new History();
        var linesDecoration = new LinesDecorationCollection();
        var textCursorAbsolutePosition = new TextCursorAbsolutePosition(textCursor, textMeasures, new ExtendedLineNumberGenerator(new LineNumberGenerator(), gaps, folds));
        var viewport = new Viewport(
            textMeasures,
            new ViewportVerticalOffsetUpdater(),
            new DefaultVerticalScrollBarMaximumValueStrategy(text, textMeasures, gaps, folds),
            new DefaultHorizontalScrollBarMaximumValueStrategy(text, textMeasures));
        var cursorPositionCorrector = new ViewportCursorPositionCorrector(viewport, textMeasures, textCursorAbsolutePosition);
        var pageScroller = new PageScroller(viewport, gaps);
        var bracketsHighlighter = new BracketsHighlighter(text, "");
        var textHighlighter = new TextHighlighter(textSelectionLineConverter);
        var inputActionsFactory = new InputActionsFactory();
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
            cursorPositionCorrector,
            pageScroller,
            lineFoldsUpdater,
            textEvents);
        var historyActionsFactory = new HistoryActionsFactory(inputActionsFactory, inputActionContext);

        return new CodeTextBoxModel(
            codeProvider,
            text,
            textLines,
            textCursor,
            textCursorAbsolutePosition,
            textSelection,
            textSelector,
            textMeasures,
            textEvents,
            textMeasuresEvents,
            tokens,
            tokensColors,
            history,
            linesDecoration,
            gaps,
            folds,
            viewport,
            bracketsHighlighter,
            textHighlighter,
            inputActionContext,
            inputActionsFactory,
            historyActionsFactory,
            new CodeTextBoxModelAdditionalInfo(text),
            new());
    }
}
