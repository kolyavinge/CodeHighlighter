using System.Collections.Generic;

namespace CodeHighlighter.Rendering;

public interface ILineFoldingPanelRenderingContext
{
    void DrawPolygon(object platformColor, IEnumerable<Common.Point> points);
}
