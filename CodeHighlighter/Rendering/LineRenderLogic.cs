using System.Windows.Media;
using CodeHighlighter.Model;

namespace CodeHighlighter.Rendering;

internal class LineRenderLogic
{
    public void DrawLines(ICodeTextBoxModel model, DrawingContext context, double actualWidth)
    {
        var linesDecorationCollection = model.LinesDecoration;
        if (!linesDecorationCollection.AnyItems) return;

        var textMeasures = model.TextMeasures;
        var viewport = model.Viewport;

        foreach (var line in LineNumber.GetLineNumbers(viewport.ActualHeight, viewport.VerticalScrollBarValue, textMeasures.LineHeight, model.TextLinesCount))
        {
            var lineDecoration = linesDecorationCollection[line.Index];
            if (lineDecoration != null)
            {
                var x = 0.0;
                var y = line.Index * textMeasures.LineHeight - viewport.VerticalScrollBarValue;
                var width = actualWidth;
                var height = textMeasures.LineHeight;
                var background = lineDecoration.Background;
                var brush = new SolidColorBrush(new() { R = background.R, G = background.G, B = background.B, A = background.A });
                context.DrawRectangle(brush, null, new(x, y, width, height));
            }
        }
    }
}
