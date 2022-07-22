using System.Windows;
using System.Windows.Input;
using CodeHighlighter.Model;

namespace CodeHighlighter.Controllers;

internal class MouseController
{
    public void OnMouseDown(ICodeTextBox codeTextBox, CodeTextBoxModel model, Point positionInControl, bool shiftPressed)
    {
        codeTextBox.Focus();
        var lineIndex = model.Viewport.GetCursorLineIndex(positionInControl);
        var columnIndex = model.Viewport.GetCursorColumnIndex(positionInControl);
        if (shiftPressed) model.InputModel.ActivateSelection();
        else model.InputModel.CompleteSelection();
        model.InputModel.MoveCursorTo(lineIndex, columnIndex);
        codeTextBox.InvalidateVisual();
    }

    public void OnMouseMove(ICodeTextBox codeTextBox, CodeTextBoxModel model, Point positionInControl, MouseButtonState leftButton)
    {
        if (leftButton == MouseButtonState.Pressed)
        {
            model.InputModel.ActivateSelection();
            var lineIndex = model.Viewport.GetCursorLineIndex(positionInControl);
            var columnIndex = model.Viewport.GetCursorColumnIndex(positionInControl);
            model.InputModel.MoveCursorTo(lineIndex, columnIndex);
            codeTextBox.InvalidateVisual();
        }
    }

    public void OnMouseWheel(ICodeTextBox codeTextBox, IViewportContext viewportContext, int delta)
    {
        viewportContext.VerticalScrollBarValue -= delta;
        codeTextBox.InvalidateVisual();
    }

    public void OnMouseDoubleClick(ICodeTextBox codeTextBox, CodeTextBoxModel model, Point positionInControl)
    {
        var lineIndex = model.Viewport.GetCursorLineIndex(positionInControl);
        var columnIndex = model.Viewport.GetCursorColumnIndex(positionInControl);
        model.InputModel.SelectToken(lineIndex, columnIndex);
        codeTextBox.InvalidateVisual();
    }
}
