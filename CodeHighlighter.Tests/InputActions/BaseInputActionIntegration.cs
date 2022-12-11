using System;
using CodeHighlighter.CodeProvidering;
using CodeHighlighter.InputActions;
using CodeHighlighter.Model;
using Moq;

namespace CodeHighlighter.Tests.InputActions;

internal class BaseInputActionIntegration
{
    protected InputModel _model;
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
        _textMeasures = new(new FontSettings());
        _textSelection = new();
        _textSelector = new(_text, _textCursor, _textSelection);
        _tokens = new();
        _viewportContext = new();
        _viewport = new(_viewportContext.Object, _textMeasures);
        _raiseTextChanged = () => { };
        _raiseTextSet = () => { };
        _model = new InputModel(_text, _textCursor, _textSelection, _tokens);
        _model.SetCodeProvider(new SqlCodeProvider());
        _context = new(
            new SqlCodeProvider(),
            _model,
            _text,
            _textCursor,
            _textMeasures,
            _textSelection,
            _textSelector,
            _model.Tokens,
            _model.TokenColors,
            _viewport,
            _viewportContext.Object,
            _raiseTextChanged,
            _raiseTextSet);
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
