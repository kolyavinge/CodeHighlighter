using System.Linq;
using CodeHighlighter.Model;

namespace CodeHighlighter.InputActions;

internal class InputActionAttribute : Attribute { }

internal class InputAction
{
    protected void SetSelection(IInputActionContext context)
    {
        if (context.TextSelection.InProgress)
        {
            context.TextSelection.EndPosition = context.TextCursor.Position;
        }
        else
        {
            context.TextSelection.StartPosition = context.TextCursor.Position;
            context.TextSelection.EndPosition = context.TextCursor.Position;
        }
    }

    protected void DeleteSelection(IInputActionContext context)
    {
        var deleteResult = context.Text.DeleteSelection(context.TextSelection.GetSelectedLines());
        context.Tokens.DeleteLines(deleteResult.FirstDeletedLineIndex, deleteResult.DeletedLinesCount);
        var (startCursorPosition, _) = context.TextSelection.GetSortedPositions();
        context.TextCursor.MoveTo(startCursorPosition);
        context.TextSelection.Reset();
    }

    protected void SetTokens(IInputActionContext context)
    {
        var codeProviderTokens = context.CodeProvider.GetTokens(new ForwardTextIterator(context.Text, 0, context.Text.LinesCount - 1)).ToList();
        context.Tokens.SetTokens(codeProviderTokens, 0, context.Text.LinesCount);
        context.TokenColors.SetColors(context.CodeProvider.GetColors());
    }

    protected void UpdateTokensForLines(IInputActionContext context, int startLineIndex, int count)
    {
        var codeProviderTokens = context.CodeProvider.GetTokens(new ForwardTextIterator(context.Text, startLineIndex, startLineIndex + count - 1)).ToList();
        context.Tokens.SetTokens(codeProviderTokens, startLineIndex, count);
    }
}
