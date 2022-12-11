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
        var viewportContext = model.ViewportContext;

        foreach (var line in LineNumber.GetLineNumbers(viewportContext.ActualHeight, viewportContext.VerticalScrollBarValue, textMeasures.LineHeight, model.Text.LinesCount))
        {
            var lineDecoration = linesDecorationCollection[line.Index];
            if (lineDecoration != null)
            {
                var x = 0.0;
                var y = line.Index * textMeasures.LineHeight - viewportContext.VerticalScrollBarValue;
                var width = actualWidth;
                var height = textMeasures.LineHeight;
                context.DrawRectangle(lineDecoration.Background, null, new(x, y, width, height));
            }
        }
    }
}
