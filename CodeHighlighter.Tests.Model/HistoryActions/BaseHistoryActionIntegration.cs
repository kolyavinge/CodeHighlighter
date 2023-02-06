using CodeHighlighter.CodeProvidering;
using CodeHighlighter.Infrastructure;
using CodeHighlighter.InputActions;
using CodeHighlighter.Model;
using Moq;
using NUnit.Framework;

namespace CodeHighlighter.Tests.HistoryActions;

internal class BaseHistoryActionIntegration
{
    protected readonly Text _text;
    protected readonly TextCursor _textCursor;
    protected readonly TextCursorAbsolutePosition _textCursorAbsolutePosition;
    protected readonly TextMeasures _textMeasures;
    protected readonly TextSelection _textSelection;
    protected readonly TextSelector _textSelector;
    protected readonly Tokens _tokens;
    protected readonly Viewport _viewport;
    protected readonly Mock<IViewportContext> _viewportContext;
    protected readonly LineGapCollection _gaps;
    protected readonly TextEvents _textEvents;
    protected Mock<ICodeTextBox> _codeTextBox;
    protected InputActionsFactory _inputActionsFactory;
    protected InputActionContext _context;

    protected BaseHistoryActionIntegration()
    {
        _text = new();
        _textCursor = new(_text);
        _textMeasures = new();
        _textSelection = new(_text);
        _textSelector = new(_text, _textCursor, _textSelection);
        _tokens = new();
        _viewportContext = new();
        _gaps = new();
        _textCursorAbsolutePosition = new TextCursorAbsolutePosition(_textCursor, _textMeasures, new ExtendedLineNumberGenerator(new LineNumberGenerator(), _gaps));
        _viewport = new(
            _viewportContext.Object,
            _textCursorAbsolutePosition,
            _textMeasures,
            new ViewportVerticalOffsetUpdater(),
            new DefaultVerticalScrollBarMaximumValueStrategy(_text, _textMeasures, _gaps),
            new DefaultHorizontalScrollBarMaximumValueStrategy(_text, _textMeasures));
        _textEvents = new(_text);
        _inputActionsFactory = new();
    }

    protected void MakeContext()
    {
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
