using CodeHighlighter.Common;

namespace CodeHighlighter.Rendering;

public interface IRenderingContext
{
    void DrawText(string text, Point position, Color? foreground);

    void DrawRectangle(object platformColor, Rect rect);

    void DrawRectangle(Color color, Rect rect);
}
