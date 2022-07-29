using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal class MoveCursorDownInputAction
{
    public static readonly MoveCursorDownInputAction Instance = new();

    public void Do(InputModel inputModel, TextCursor textCursor, Viewport viewport, ICodeTextBox? codeTextBox)
    {
        inputModel.MoveCursorDown();
        viewport.CorrectByCursorPosition(textCursor);
        codeTextBox?.InvalidateVisual();
    }
}
