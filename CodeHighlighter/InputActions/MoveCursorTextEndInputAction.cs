using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal class MoveCursorTextEndInputAction
{
    public static readonly MoveCursorTextEndInputAction Instance = new();

    public void Do(InputModel inputModel, TextCursor textCursor, Viewport viewport, ICodeTextBox? codeTextBox)
    {
        inputModel.MoveCursorTextEnd();
        viewport.CorrectByCursorPosition(textCursor);
        codeTextBox?.InvalidateVisual();
    }
}
