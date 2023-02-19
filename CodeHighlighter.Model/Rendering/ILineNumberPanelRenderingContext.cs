using CodeHighlighter.Common;

namespace CodeHighlighter.Rendering;

public enum TextAlign { Left, Right }

public interface ILineNumberPanelRenderingContext
{
    void Render(double lineOffsetY, string lineNumber, double controlWidth, Point value, TextAlign align);
}
