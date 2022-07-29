using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal class MoveCursorTextBeginInputAction
{
    public static readonly MoveCursorTextBeginInputAction Instance = new();

    public void Do(InputModel inputModel, TextCursor textCursor, Viewport viewport, ICodeTextBox? codeTextBox)
    {
        inputModel.MoveCursorTextBegin();
        viewport.CorrectByCursorPosition(textCursor);
        codeTextBox?.InvalidateVisual();
    }
}
