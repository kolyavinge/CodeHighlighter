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
    protected Mock<ICodeTextBox> _codeTextBox;
    protected HistoryActionContext _context;

    protected BaseHistoryActionIntegration()
    {
        _text = new();
        _textCursor = new(_text);
        _textMeasures = new(new FontSettings());
        _textSelection = new(0, 0, 0, 0);
        _viewportContext = new();
        _viewport = new(_viewportContext.Object, _textMeasures);
        _raiseTextChanged = new Action(() => { });
        _inputModel = new(_text, _textCursor, _textSelection, new());
    }

    protected void MakeContext()
    {
        _context = new(_inputModel, _text, _textCursor, _textMeasures, _textSelection, _viewport, _viewportContext.Object, _raiseTextChanged);
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
        _context.TextSelection.StartCursorLineIndex = 1;
        _context.TextSelection.EndCursorLineIndex = 1;
    }

    protected void AssertCursorPosition(CursorPosition position)
    {
        Assert.AreEqual(position.LineIndex, _context.TextCursor.LineIndex);
        Assert.AreEqual(position.ColumnIndex, _context.TextCursor.ColumnIndex);
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
