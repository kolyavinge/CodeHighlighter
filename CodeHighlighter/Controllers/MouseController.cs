using System.Windows;
using System.Windows.Input;
using CodeHighlighter.Model;

namespace CodeHighlighter.Controllers;

internal class MouseController
{
    public void OnMouseDown(ICodeTextBox codeTextBox, CodeTextBoxModel model, Point positionInControl, bool shiftPressed)
    {
        codeTextBox.Focus();
        var pos = model.Viewport.GetCursorPosition(positionInControl);
        if (shiftPressed) model.InputModel.ActivateSelection();
        else model.InputModel.CompleteSelection();
        model.InputModel.MoveCursorTo(pos);
        codeTextBox.InvalidateVisual();
    }

    public void OnMouseMove(ICodeTextBox codeTextBox, CodeTextBoxModel model, Point positionInControl, MouseButtonState leftButton)
    {
        if (leftButton == MouseButtonState.Pressed)
        {
            model.InputModel.ActivateSelection();
            var pos = model.Viewport.GetCursorPosition(positionInControl);
            model.InputModel.MoveCursorTo(pos);
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
        var pos = model.Viewport.GetCursorPosition(positionInControl);
        model.InputModel.SelectToken(pos);
        codeTextBox.InvalidateVisual();
    }
}
