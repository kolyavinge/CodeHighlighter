using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal class MoveCursorPageDownInputAction
{
    public static readonly MoveCursorPageDownInputAction Instance = new();

    public void Do(InputModel inputModel, TextCursor textCursor, Viewport viewport, ICodeTextBox? codeTextBox)
    {
        inputModel.MoveCursorPageDown(viewport.GetLinesCountInViewport());
        viewport.CorrectByCursorPosition(textCursor);
        codeTextBox?.InvalidateVisual();
    }
}
