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
            _context.DrawNumber(line.OffsetY, line.LineIndex + 1, TextAlign.Right);
        }
    }
}
