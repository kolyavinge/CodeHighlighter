using System.Windows;
using System.Windows.Input;
using CodeHighlighter.Model;

namespace CodeHighlighter.Controllers;

internal class MouseController
{
    public void OnMouseDown(ICodeTextBox codeTextBox, Viewport viewport, ITextSelectionActivator selectionActivator, ICursorHandler cursorHandler, Point positionInControl, bool shiftPressed)
    {
        codeTextBox.Focus();
        var lineIndex = viewport.GetCursorLineIndex(positionInControl);
        var columnIndex = viewport.GetCursorColumnIndex(positionInControl);
        if (shiftPressed) selectionActivator.ActivateSelection();
        else selectionActivator.CompleteSelection();
        cursorHandler.MoveCursorTo(lineIndex, columnIndex);
        codeTextBox.InvalidateVisual();
    }

    public void OnMouseMove(ICodeTextBox codeTextBox, Viewport viewport, ITextSelectionActivator selectionActivator, ICursorHandler cursorHandler, Point positionInControl, MouseButtonState leftButton)
    {
        if (leftButton == MouseButtonState.Pressed)
        {
            selectionActivator.ActivateSelection();
            var lineIndex = viewport.GetCursorLineIndex(positionInControl);
            var columnIndex = viewport.GetCursorColumnIndex(positionInControl);
            cursorHandler.MoveCursorTo(lineIndex, columnIndex);
            codeTextBox.InvalidateVisual();
        }
    }

    public void OnMouseWheel(ICodeTextBox codeTextBox, IViewportContext viewportContext, int delta)
    {
        viewportContext.VerticalScrollBarValue -= delta;
        codeTextBox.InvalidateVisual();
    }

    public void OnMouseDoubleClick(ICodeTextBox codeTextBox, Viewport viewport, ITokenSelector tokenSelector, Point positionInControl)
    {
        var lineIndex = viewport.GetCursorLineIndex(positionInControl);
        var columnIndex = viewport.GetCursorColumnIndex(positionInControl);
        tokenSelector.SelectToken(lineIndex, columnIndex);
        codeTextBox.InvalidateVisual();
    }
}
