using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal class MoveCursorPageUpInputAction
{
    public static readonly MoveCursorPageUpInputAction Instance = new();

    public void Do(InputModel inputModel, TextCursor textCursor, Viewport viewport, ICodeTextBox? codeTextBox)
    {
        inputModel.MoveCursorPageUp(viewport.GetLinesCountInViewport());
        viewport.CorrectByCursorPosition(textCursor);
        codeTextBox?.InvalidateVisual();
    }
}
