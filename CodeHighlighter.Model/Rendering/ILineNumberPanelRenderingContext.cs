namespace CodeHighlighter.Rendering;

public enum TextAlign { Left, Right }

public interface ILineNumberPanelRenderingContext
{
    void RenderNumber(double offsetY, string lineNumber, TextAlign align);
}
