using System;
using CodeHighlighter.CodeProvidering;
using CodeHighlighter.InputActions;
using CodeHighlighter.Model;
using Moq;

namespace CodeHighlighter.Tests.InputActions;

internal class BaseInputActionIntegration
{
    protected Text _text;
    protected TextCursor _textCursor;
    protected TextMeasures _textMeasures;
    protected TextSelection _textSelection;
    protected TextSelector _textSelector;
    protected Tokens _tokens;
    protected Viewport _viewport;
    protected Mock<IViewportContext> _viewportContext;
    protected Action _raiseTextChanged;
    protected Action _raiseTextSet;
    protected InputActionContext _context;

    protected void Init()
    {
        _text = new();
        _textCursor = new(_text);
        _textMeasures = new();
        _textSelection = new(_text);
        _textSelector = new(_text, _textCursor, _textSelection);
        _tokens = new();
        _viewportContext = new();
        _viewport = new(_text, _viewportContext.Object, _textCursor, _textMeasures);
        _raiseTextChanged = () => { };
        _raiseTextSet = () => { };
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
            _raiseTextChanged,
            _raiseTextSet);
        SetText("");
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

    protected void MoveCursorEndLine()
    {
        MoveCursorEndLineInputAction.Instance.Do(_context);
    }

    protected void MoveCursorTextEnd()
    {
        MoveCursorTextEndInputAction.Instance.Do(_context);
    }

    protected void SelectAll()
    {
        SelectAllInputAction.Instance.Do(_context);
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
        AppendNewLineInputAction.Instance.Do(_context);
    }
}
