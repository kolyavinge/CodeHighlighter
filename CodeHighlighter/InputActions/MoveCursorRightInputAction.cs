using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal class MoveCursorRightInputAction
{
    public static readonly MoveCursorRightInputAction Instance = new();

    public void Do(InputModel inputModel, TextCursor textCursor, Viewport viewport, ICodeTextBox? codeTextBox)
    {
        inputModel.MoveCursorRight();
        viewport.CorrectByCursorPosition(textCursor);
        codeTextBox?.InvalidateVisual();
    }
}
