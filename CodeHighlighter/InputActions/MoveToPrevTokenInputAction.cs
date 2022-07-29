using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal class MoveToPrevTokenInputAction
{
    public static readonly MoveToPrevTokenInputAction Instance = new();

    public void Do(InputModel inputModel, TextCursor textCursor, Viewport viewport, ICodeTextBox? codeTextBox)
    {
        inputModel.MoveToPrevToken();
        viewport.CorrectByCursorPosition(textCursor);
        codeTextBox?.InvalidateVisual();
    }
}
