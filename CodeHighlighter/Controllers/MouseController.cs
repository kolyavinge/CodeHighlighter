using System.Windows;
using System.Windows.Input;
using CodeHighlighter.Model;

namespace CodeHighlighter.Controllers;

internal class MouseController
{
    private readonly ICodeTextBox _codeTextBox;
    private readonly ITokenSelector _tokenSelector;
    private readonly ICursorHandler _cursorHandler;
    private readonly ITextSelectionActivator _selectionActivator;
    private readonly Viewport _viewport;
    private readonly IViewportContext _viewportContext;

    public MouseController(
        ICodeTextBox codeTextBox, ITokenSelector tokenSelector, ICursorHandler cursorHandler, ITextSelectionActivator selectionActivator, Viewport viewport, IViewportContext viewportContext)
    {
        _codeTextBox = codeTextBox;
        _tokenSelector = tokenSelector;
        _cursorHandler = cursorHandler;
        _selectionActivator = selectionActivator;
        _viewport = viewport;
        _viewportContext = viewportContext;
    }

    public void OnMouseDown(Point positionInControl, bool shiftPressed)
    {
        _codeTextBox.Focus();
        var lineIndex = _viewport.GetCursorLineIndex(positionInControl);
        var columnIndex = _viewport.GetCursorColumnIndex(positionInControl);
        if (shiftPressed) _selectionActivator.ActivateSelection();
        else _selectionActivator.CompleteSelection();
        _cursorHandler.MoveCursorTo(lineIndex, columnIndex);
        _codeTextBox.InvalidateVisual();
    }

    public void OnMouseMove(Point positionInControl, MouseButtonState leftButton)
    {
        if (leftButton == MouseButtonState.Pressed)
        {
            _selectionActivator.ActivateSelection();
            var lineIndex = _viewport.GetCursorLineIndex(positionInControl);
            var columnIndex = _viewport.GetCursorColumnIndex(positionInControl);
            _cursorHandler.MoveCursorTo(lineIndex, columnIndex);
            _codeTextBox.InvalidateVisual();
        }
    }

    public void OnMouseWheel(int delta)
    {
        _viewportContext.VerticalScrollBarValue -= delta;
        _codeTextBox.InvalidateVisual();
    }

    public void OnMouseDoubleClick(Point positionInControl)
    {
        var lineIndex = _viewport.GetCursorLineIndex(positionInControl);
        var columnIndex = _viewport.GetCursorColumnIndex(positionInControl);
        _tokenSelector.SelectToken(lineIndex, columnIndex);
        _codeTextBox.InvalidateVisual();
    }
}
