using System.Windows.Input;
using CodeHighlighter.Common;
using CodeHighlighter.Model;

namespace CodeHighlighter.Controllers;

internal class MouseController
{
    public void OnMouseDown(ICodeTextBox codeTextBox, ICodeTextBoxModel model, Point positionInControl)
    {
        codeTextBox.Focus();
        var pos = model.Viewport.GetCursorPosition(positionInControl);
        model.MoveCursorTo(pos);
        codeTextBox.InvalidateVisual();
    }

    public void OnMouseMove(ICodeTextBox codeTextBox, ICodeTextBoxModel model, Point positionInControl, MouseButtonState leftButton)
    {
        if (leftButton == MouseButtonState.Pressed)
        {
            model.ActivateSelection();
            var pos = model.Viewport.GetCursorPosition(positionInControl);
            model.MoveCursorTo(pos);
            codeTextBox.InvalidateVisual();
        }
    }

    public void OnMouseUp(ICodeTextBoxModel model)
    {
        model.CompleteSelection();
    }

    public void OnMouseWheel(ICodeTextBox codeTextBox, IViewportContext viewportContext, ITextMeasures textMeasures, int pages, bool up)
    {
        viewportContext.VerticalScrollBarValue += (up ? -1 : 1) * pages * textMeasures.LineHeight;
        codeTextBox.InvalidateVisual();
    }

    public void OnMouseDoubleClick(ICodeTextBox codeTextBox, ICodeTextBoxModel model, Point positionInControl)
    {
        var pos = model.Viewport.GetCursorPosition(positionInControl);
        model.SelectToken(pos);
        codeTextBox.InvalidateVisual();
    }
}
