using System.Linq;
using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal class AppendNewLineInputAction : InputAction
{
    public static readonly AppendNewLineInputAction Instance = new();

    public AppendNewLineResult Do(InputActionContext context)
    {
        var result = AppendNewLine(context);
        context.Viewport.CorrectByCursorPosition(context.TextCursor);
        context.Viewport.UpdateScrollbarsMaximumValues(context.Text);
        context.RaiseTextChanged();

        return result;
    }

    private AppendNewLineResult AppendNewLine(InputActionContext context)
    {
        var oldCursorPosition = context.TextCursor.Position;
        var (selectionStart, selectionEnd) = context.TextSelection.GetSortedPositions();
        var deletedSelectedText = GetSelectedText(context);
        if (context.TextSelection.IsExist)
        {
            if (selectionStart.Kind == CursorPositionKind.Virtual)
            {
                context.TextSelection.StartPosition = new(context.TextSelection.StartPosition.LineIndex, 0);
            }
            DeleteSelection(context);
        }
        context.Text.AppendNewLine(context.TextCursor.Position);
        context.Tokens.InsertEmptyLine(context.TextCursor.LineIndex + 1);
        context.TextCursor.MoveDown();
        context.TextCursor.MoveEndLine();
        if (context.Text.GetLine(context.TextCursor.LineIndex).Any()) context.TextCursor.MoveStartLine();
        var newCursorPosition = context.TextCursor.Position;
        UpdateTokensForLines(context, context.TextCursor.LineIndex - 1, 2);

        return new(oldCursorPosition, newCursorPosition, selectionStart, selectionEnd, deletedSelectedText);
    }
}
