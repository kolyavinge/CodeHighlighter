using System;
using CodeHighlighter.HistoryActions;
using CodeHighlighter.Model;
using Moq;
using NUnit.Framework;

namespace CodeHighlighter.Tests.HistoryActions;

internal class BaseHistoryActionIntegration
{
    protected readonly InputModel _inputModel;
    protected readonly Text _text;
    protected readonly TextCursor _textCursor;
    protected readonly TextMeasures _textMeasures;
    protected readonly TextSelection _textSelection;
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
        _viewportContext = new();
        _viewport = new(_viewportContext.Object, _textMeasures);
        _raiseTextChanged = () => { };
        _raiseTextSet = () => { };
        _inputModel = new(_text, _textCursor, _textSelection, new());
    }

    protected void MakeContext()
    {
        _context = new(_inputModel, _text, _textCursor, _textMeasures, _textSelection, _viewport, _viewportContext.Object, _raiseTextChanged, _raiseTextSet);
        _codeTextBox = new();
        _context.CodeTextBox = _codeTextBox.Object;
    }

    protected void MakeUncompleteSelection()
    {
        _context.InputModel.MoveCursorTo(new(2, 0));
        _context.InputModel.ActivateSelection();
    }

    protected void MakeInactiveSelection()
    {
        _context.TextSelection.StartPosition = new(1, 0);
        _context.TextSelection.EndPosition = new(1, 0);
    }

    protected void AssertCursorPosition(CursorPosition position)
    {
        Assert.AreEqual(position, _context.TextCursor.Position);
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
