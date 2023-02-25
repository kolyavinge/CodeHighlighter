using CodeHighlighter.CodeProvidering;
using CodeHighlighter.Infrastructure;
using CodeHighlighter.InputActions;
using CodeHighlighter.Model;

namespace CodeHighlighter.Tests.InputActions;

internal class BaseInputActionIntegration
{
    protected Text _text;
    protected TextCursor _textCursor;
    protected TextCursorAbsolutePosition _textCursorAbsolutePosition;
    protected TextMeasures _textMeasures;
    protected TextSelection _textSelection;
    protected TextSelector _textSelector;
    protected Tokens _tokens;
    protected LineGapCollection _gaps;
    protected Viewport _viewport;
    protected ViewportCursorPositionCorrector _cursorPositionCorrector;
    protected PageScroller _pageScroller;
    protected TextEvents _textEvents;
    protected InputActionsFactory _inputActionFactory;
    protected InputActionContext _context;

    protected void Init()
    {
        _text = new();
        _textCursor = new(_text);
        _textMeasures = new();
        _textSelection = new(_text);
        _textSelector = new(_text, _textCursor, _textSelection);
        _tokens = new();
        _gaps = new();
        _textCursorAbsolutePosition = new(_textCursor, _textMeasures, new ExtendedLineNumberGenerator(new LineNumberGenerator(), _gaps));
        _viewport = new(
            _textMeasures,
            new ViewportVerticalOffsetUpdater(),
            new DefaultVerticalScrollBarMaximumValueStrategy(_text, _textMeasures, _gaps),
            new DefaultHorizontalScrollBarMaximumValueStrategy(_text, _textMeasures));
        _cursorPositionCorrector = new ViewportCursorPositionCorrector(_viewport, _textMeasures, _textCursorAbsolutePosition);
        _pageScroller = new PageScroller(_viewport, _gaps);
        _textEvents = new(_text, new TextChangedEventArgsFactory(new TextLinesChangingLogic()));
        _inputActionFactory = new();
        _context = new(
            new SqlCodeProvider(),
            _text,
            _textCursor,
            _textMeasures,
            _textSelection,
            _textSelector,
            _tokens,
            new TokensColors(),
            _viewport,
            _cursorPositionCorrector,
            _pageScroller,
            _textEvents);
        SetText("");
    }

    protected void SetText(string text)
    {
        new SetTextInputAction().Do(_context, text);
    }

    protected void MoveCursorTo(CursorPosition position)
    {
        new MoveCursorToInputAction().Do(_context, position);
    }

    protected void MoveCursorStartLine()
    {
        new MoveCursorStartLineInputAction().Do(_context);
    }

    protected void MoveCursorEndLine()
    {
        new MoveCursorEndLineInputAction().Do(_context);
    }

    protected void MoveCursorTextEnd()
    {
        new MoveCursorTextEndInputAction().Do(_context);
    }

    protected void SelectAll()
    {
        new SelectAllInputAction().Do(_context);
    }

    protected void ActivateSelection()
    {
        _textSelector.ActivateSelection();
    }

    protected void CompleteSelection()
    {
        _textSelector.CompleteSelection();
    }

    protected void AppendNewLine()
    {
        new AppendNewLineInputAction().Do(_context);
    }
}
