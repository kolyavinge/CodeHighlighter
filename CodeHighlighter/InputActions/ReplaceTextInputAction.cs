using System;
using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal class ReplaceTextInputAction
{
    public static readonly ReplaceTextInputAction Instance = new();

    public void Do(
        int cursorStartLineIndex,
        int cursorStartColumnIndex,
        int cursorEndLineIndex,
        int cursorEndColumnIndex,
        string insertedText,
        InputModel inputModel,
        Text text,
        TextCursor textCursor,
        Viewport viewport,
        ICodeTextBox? codeTextBox,
        Action raiseTextChanged)
    {
        inputModel.MoveCursorTo(cursorStartLineIndex, cursorStartColumnIndex);
        inputModel.ActivateSelection();
        inputModel.MoveCursorTo(cursorEndLineIndex, cursorEndColumnIndex);
        inputModel.CompleteSelection();
        inputModel.InsertText(insertedText);
        viewport.CorrectByCursorPosition(textCursor);
        viewport.UpdateScrollbarsMaximumValues(text);
        raiseTextChanged();
        codeTextBox?.InvalidateVisual();
    }
}
