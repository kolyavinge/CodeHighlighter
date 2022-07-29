using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal class MoveToNextTokenInputAction
{
    public static readonly MoveToNextTokenInputAction Instance = new();

    public void Do(InputModel inputModel, TextCursor textCursor, Viewport viewport, ICodeTextBox? codeTextBox)
    {
        inputModel.MoveToNextToken();
        viewport.CorrectByCursorPosition(textCursor);
        codeTextBox?.InvalidateVisual();
    }
}
