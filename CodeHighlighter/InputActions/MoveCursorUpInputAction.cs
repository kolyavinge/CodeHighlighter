using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal class MoveCursorUpInputAction
{
    public static readonly MoveCursorUpInputAction Instance = new();

    public void Do(InputModel inputModel, TextCursor textCursor, Viewport viewport, ICodeTextBox? codeTextBox)
    {
        inputModel.MoveCursorUp();
        viewport.CorrectByCursorPosition(textCursor);
        codeTextBox?.InvalidateVisual();
    }
}
