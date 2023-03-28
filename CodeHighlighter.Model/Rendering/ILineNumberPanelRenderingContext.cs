using CodeHighlighter.Common;

namespace CodeHighlighter.Rendering;

public enum TextAlign { Left, Right }

public interface ILineNumberPanelRenderingContext
{
    void DrawNumber(double offsetY, int number, TextAlign align);
    void DrawRectangle(object platformColor, Rect rect);
}
