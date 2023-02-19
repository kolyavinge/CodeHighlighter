using System.Collections.Generic;
using CodeHighlighter.Model;

namespace CodeHighlighter.Rendering;

public interface INumberRendering
{
    void Render(IEnumerable<LineNumber> lines, double controlWidth);
}

internal class NumberRendering : INumberRendering
{
    private readonly ILineNumberPanelRenderingContext _context;

    public NumberRendering(ILineNumberPanelRenderingContext context)
    {
        _context = context;
    }

    public void Render(IEnumerable<LineNumber> lines, double controlWidth)
    {
        foreach (var line in lines)
        {
            var lineNumber = (line.LineIndex + 1).ToString();
            _context.Render(line.OffsetY, lineNumber, controlWidth, new(0, line.OffsetY), TextAlign.Right);
        }
    }
}
