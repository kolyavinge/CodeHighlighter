using System;
using CodeHighlighter.CodeProvidering;
using CodeHighlighter.HistoryActions;
using CodeHighlighter.InputActions;
using CodeHighlighter.Model;
using Moq;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace CodeHighlighter.Tests.HistoryActions;

internal class BaseHistoryActionIntegration
{
    protected readonly Text _text;
    protected readonly TextCursor _textCursor;
    protected readonly TextMeasures _textMeasures;
    protected readonly TextSelection _textSelection;
    protected readonly TextSelector _textSelector;
    protected readonly Tokens _tokens;
    protected readonly Viewport _viewport;
    protected readonly Mock<IViewportContext> _viewportContext;
    protected readonly Action _raiseTextChanged;
    protected readonly Action _raiseTextSet;
    protected Mock<ICodeTextBox> _codeTextBox;
    protected HistoryActionContext _context;

    protected BaseHistoryActionIntegration()
    {
        _text = new();
        _textCursor = new(_text);
        _textMeasures = new(new FontSettings());
        _textSelection = new();
        _textSelector = new(_text, _textCursor, _textSelection);
        _tokens = new();
        _viewportContext = new();
        _viewport = new(_text, _viewportContext.Object, _textMeasures);
        _raiseTextChanged = () => { };
        _raiseTextSet = () => { };
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
            new(),
            _viewport,
            _raiseTextChanged,
            _raiseTextSet);
        _codeTextBox = new();
        _context.CodeTextBox = _codeTextBox.Object;
    }

    protected void SetText(string text)
    {
        SetTextInputAction.Instance.Do(_context, text);
    }

    protected void MoveCursorTo(CursorPosition position)
    {
        MoveCursorToInputAction.Instance.Do(_context, position);
    }

    protected void MoveCursorStartLine()
    {
        MoveCursorStartLineInputAction.Instance.Do(_context);
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
