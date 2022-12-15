using System.Linq;
using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal class DeleteSelectedLinesInputAction
{
    public static readonly DeleteSelectedLinesInputAction Instance = new();

    public DeleteSelectedLinesResult Do(InputActionContext context)
    {
        var result = DeleteSelectedLines(context);
        context.Viewport.CorrectByCursorPosition();
        context.Viewport.UpdateScrollbarsMaximumValues();
        context.RaiseTextChanged();

        return result;
    }

    private DeleteSelectedLinesResult DeleteSelectedLines(InputActionContext context)
    {
        var oldCursorPosition = context.TextCursor.Position;
        var (selectionStart, selectionEnd) = context.TextSelection.GetSortedPositions();
        int startLineIndex, endLineIndex;
        string deletedSelectedText;
        if (context.TextSelection.IsExist)
        {
            startLineIndex = selectionStart.LineIndex;
            endLineIndex = selectionEnd.LineIndex;
            deletedSelectedText = String.Join(
                Environment.NewLine,
                Enumerable.Range(selectionStart.LineIndex, selectionEnd.LineIndex - selectionStart.LineIndex + 1).Select(context.Text.GetLine));
            context.TextSelection.Reset();
        }
        else
        {
            startLineIndex = endLineIndex = context.TextCursor.LineIndex;
            deletedSelectedText = context.Text.GetLine(context.TextCursor.LineIndex).ToString();
        }
        var oldTextLinesCount = context.Text.LinesCount;
        for (int i = startLineIndex; i <= endLineIndex; i++)
        {
            if (startLineIndex < context.Text.LinesCount - 1)
            {
                context.Text.DeleteLine(startLineIndex);
                context.Tokens.DeleteLine(startLineIndex);
            }
            else
            {
                context.Text.GetLine(startLineIndex).Clear();
                context.Tokens.GetTokens(startLineIndex).Clear();
            }
        }
        context.TextCursor.MoveTo(new(startLineIndex, 0));
        var newCursorPosition = context.TextCursor.Position;
        if (endLineIndex < oldTextLinesCount - 1)
        {
            deletedSelectedText += Environment.NewLine;
        }

        return new(oldCursorPosition, newCursorPosition, selectionStart, selectionEnd, deletedSelectedText);
    }
}
