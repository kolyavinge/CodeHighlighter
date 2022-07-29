using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal class MoveCursorEndLineInputAction
{
    public static readonly MoveCursorEndLineInputAction Instance = new();

    public void Do(InputModel inputModel, TextCursor textCursor, Viewport viewport, ICodeTextBox? codeTextBox)
    {
        inputModel.MoveCursorEndLine();
        viewport.CorrectByCursorPosition(textCursor);
        codeTextBox?.InvalidateVisual();
    }
}
