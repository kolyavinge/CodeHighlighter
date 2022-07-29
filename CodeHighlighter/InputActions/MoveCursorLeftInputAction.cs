using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal class MoveCursorLeftInputAction
{
    public static readonly MoveCursorLeftInputAction Instance = new();

    public void Do(InputModel inputModel, TextCursor textCursor, Viewport viewport, ICodeTextBox? codeTextBox)
    {
        inputModel.MoveCursorLeft();
        viewport.CorrectByCursorPosition(textCursor);
        codeTextBox?.InvalidateVisual();
    }
}
