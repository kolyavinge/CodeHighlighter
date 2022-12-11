using System.Windows.Media;
using CodeHighlighter.Model;
using CodeHighlighter.Utils;

namespace CodeHighlighter.Rendering;

internal class TextSelectionRenderLogic
{
    private readonly TextSelectionRect _textSelectionRect = new();

    public void DrawSelectedLines(
        CodeTextBoxModel model, DrawingContext context, Brush brush)
    {
        var selectedLines = model.TextSelection.GetSelectedLines(model.Text);
        _textSelectionRect
            .GetCalculatedRects(selectedLines, model.TextMeasures, model.Viewport.HorizontalScrollBarValue, model.Viewport.VerticalScrollBarValue)
            .Each(rect => context.DrawRectangle(brush, null, rect));
    }
}
