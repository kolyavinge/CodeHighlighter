using CodeHighlighter.Ancillary;
using CodeHighlighter.CodeProvidering;
using CodeHighlighter.Core;
using CodeHighlighter.Infrastructure;
using CodeHighlighter.InputActions;
using CodeHighlighter.Model;
using Moq;
using NUnit.Framework;

namespace CodeHighlighter.Tests.HistoryActions;

internal class BaseHistoryActionIntegration
{
    protected readonly SqlCodeProvider _codeProvider;
    protected readonly Text _text;
    protected readonly TextCursor _textCursor;
    protected readonly TextCursorAbsolutePosition _textCursorAbsolutePosition;
    protected readonly TextMeasures _textMeasures;
    protected readonly TextSelectionLineConverter _textSelectionLineConverter;
    protected readonly TextSelection _textSelection;
    protected readonly TextSelector _textSelector;
    protected readonly Tokens _tokens;
    protected readonly TokensColors _tokensColors;
    protected readonly Viewport _viewport;
    protected readonly ViewportCursorPositionCorrector _cursorPositionCorrector;
    protected readonly PageScroller _pageScroller;
    protected readonly EditTextResultToLinesChangeConverter _editTextResultToLinesChangeConverter;
    protected readonly LineGapCollection _gaps;
    protected readonly LineFolds _folds;
    protected readonly LineFoldsUpdater _lineFoldsUpdater;
    protected readonly TextEvents _textEvents;
    protected Mock<ICodeTextBox> _codeTextBox;
    protected InputActionsFactory _inputActionsFactory;
    protected InputActionContext _context;

    protected BaseHistoryActionIntegration()
    {
        _codeProvider = new();
        _text = new();
        _textMeasures = new();
        _gaps = new();
        _folds = new();
        _editTextResultToLinesChangeConverter = new EditTextResultToLinesChangeConverter(new TextLinesChangingLogic());
        _lineFoldsUpdater = new LineFoldsUpdater(_folds, _editTextResultToLinesChangeConverter);
        _textCursor = new(_text, new TextCursorPositionCorrector(_text, _folds));
        _textSelectionLineConverter = new TextSelectionLineConverter(_text);
        _textSelection = new(_text, _textSelectionLineConverter);
        _textSelector = new(_text, _textCursor, _textSelection);
        _tokens = new();
        _tokensColors = new(_codeProvider);
        _textCursorAbsolutePosition = new TextCursorAbsolutePosition(_textCursor, _textMeasures, new ExtendedLineNumberGenerator(new LineNumberGenerator(), _gaps, _folds));
        _viewport = new(
            _textMeasures,
            new ViewportVerticalOffsetUpdater(),
            new DefaultVerticalScrollBarMaximumValueStrategy(_text, _textMeasures, _gaps, _folds),
            new DefaultHorizontalScrollBarMaximumValueStrategy(_text, _textMeasures));
        _cursorPositionCorrector = new ViewportCursorPositionCorrector(_viewport, _textMeasures, _textCursorAbsolutePosition);
        _pageScroller = new PageScroller(_viewport, _gaps);
        _textEvents = new(_text, new TextChangedEventArgsFactory(_editTextResultToLinesChangeConverter));
        _inputActionsFactory = new();
    }

    protected void MakeContext()
    {
        _context = new(
            _codeProvider,
            _text,
            _textCursor,
            _textMeasures,
            _textSelection,
            _textSelector,
            _tokens,
            _tokensColors,
            _viewport,
            _cursorPositionCorrector,
            _pageScroller,
            _lineFoldsUpdater,
            _textEvents);
        _codeTextBox = new();
        _context.CodeTextBox = _codeTextBox.Object;
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

    protected void ActivateSelection()
    {
        _textSelector.ActivateSelection();
    }

    protected void CompleteSelection()
    {
        _textSelector.CompleteSelection();
    }

    protected void MakeUncompleteSelection()
    {
        MoveCursorTo(new(2, 0));
        ActivateSelection();
    }

    protected void MakeInactiveSelection()
    {
        _textSelection.StartPosition = new(1, 0);
        _textSelection.EndPosition = new(1, 0);
    }

    protected void AssertCursorPosition(CursorPosition position)
    {
        Assert.AreEqual(position, _textCursor.Position);
    }

    protected void InvalidateVisualCallNever()
    {
        _codeTextBox.Verify(x => x.InvalidateVisual(), Times.Never());
    }

    protected void InvalidateVisualCallThreeTimes()
    {
        _codeTextBox.Verify(x => x.InvalidateVisual(), Times.Exactly(3));
    }
}
