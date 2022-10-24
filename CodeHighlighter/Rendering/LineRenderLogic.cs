using System.Windows.Media;
using CodeHighlighter.Model;

namespace CodeHighlighter.Rendering;

internal class LineRenderLogic
{
    public void DrawLines(
        CodeTextBoxModel model,
        DrawingContext context,
        double actualWidth)
    {
        var linesDecorationCollection = model.LinesDecoration;
        if (!linesDecorationCollection.AnyLines)
        {
            return;
        }

        var inputModel = model.InputModel;
        var textMeasures = model.TextMeasures;
        var viewport = model.Viewport;
        var viewportContext = model.ViewportContext;

        var startLine = (int)(viewportContext.VerticalScrollBarValue / textMeasures.LineHeight);
        var linesCount = viewport.GetLinesCountInViewport();
        var endLine = Math.Min(startLine + linesCount, inputModel.Text.LinesCount);

        for (int lineIndex = startLine; lineIndex < endLine; lineIndex++)
        {
            var lineDecoration = linesDecorationCollection[lineIndex];
            if (lineDecoration != null)
            {
                var x = 0.0;
                var y = lineIndex * textMeasures.LineHeight - viewportContext.VerticalScrollBarValue;
                var width = actualWidth;
                var height = textMeasures.LineHeight;
                context.DrawRectangle(lineDecoration.Background, null, new(x, y, width, height));
            }
        }
    }
}
