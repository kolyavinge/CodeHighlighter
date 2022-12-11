using System.Windows.Media;
using CodeHighlighter.Model;

namespace CodeHighlighter.Rendering;

internal class LineRenderLogic
{
    public void DrawLines(CodeTextBoxModel model, DrawingContext context, double actualWidth)
    {
        var linesDecorationCollection = model.LinesDecoration;
        if (!linesDecorationCollection.AnyItems) return;

        var textMeasures = model.TextMeasures;
        var viewport = model.Viewport;

        foreach (var line in LineNumber.GetLineNumbers(viewport.ActualHeight, viewport.VerticalScrollBarValue, textMeasures.LineHeight, model.Text.LinesCount))
        {
            var lineDecoration = linesDecorationCollection[line.Index];
            if (lineDecoration != null)
            {
                var x = 0.0;
                var y = line.Index * textMeasures.LineHeight - viewport.VerticalScrollBarValue;
                var width = actualWidth;
                var height = textMeasures.LineHeight;
                context.DrawRectangle(lineDecoration.Background, null, new(x, y, width, height));
            }
        }
    }
}
