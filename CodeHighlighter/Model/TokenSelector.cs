using System.Linq;

namespace CodeHighlighter.Model;

internal class TokenSelector
{
    public Token? GetTokenOnPosition(ITokens tokens, int lineIndex, int columnIndex)
    {
        if (lineIndex >= tokens.LinesCount) return default;
        var lineTokens = tokens.GetTokens(lineIndex);
        return lineTokens.LastOrDefault(x => x.StartColumnIndex <= columnIndex && columnIndex <= x.EndColumnIndex + 1);
    }

    public SelectedRange GetSelection(ITokens tokens, int lineIndex, int columnIndex)
    {
        if (lineIndex >= tokens.LinesCount) return default;
        var lineTokens = tokens.GetTokens(lineIndex);
        var cursor = TokenCursorPosition.GetPosition(lineTokens, columnIndex);
        if (cursor.Position == TokenCursorPositionKind.StartLine)
        {
            return ToSelectedRange(cursor.Right);
        }
        else if (cursor.Position == TokenCursorPositionKind.BetweenTokens && columnIndex == cursor.Right.StartColumnIndex)
        {
            return ToSelectedRange(cursor.Right);
        }
        else if (cursor.Position == TokenCursorPositionKind.BetweenTokens && columnIndex == cursor.Left.EndColumnIndex + 1)
        {
            return ToSelectedRange(cursor.Left);
        }
        else if (cursor.Position == TokenCursorPositionKind.BetweenTokens)
        {
            return new SelectedRange(cursor.Left.EndColumnIndex + 1, cursor.Right.StartColumnIndex);
        }
        else if (cursor.Position == TokenCursorPositionKind.InToken)
        {
            return ToSelectedRange(cursor.Left);
        }
        else // TokenCursorPositionKind.EndLine
        {
            return ToSelectedRange(cursor.Left);
        }
    }

    private SelectedRange ToSelectedRange(Token token)
    {
        return new SelectedRange(token.StartColumnIndex, token.EndColumnIndex + 1);
    }

    public readonly struct SelectedRange
    {
        public readonly int StartCursorColumnIndex;
        public readonly int EndCursorColumnIndex;

        public SelectedRange(int startCursorColumnIndex, int endCursorColumnIndex)
        {
            StartCursorColumnIndex = startCursorColumnIndex;
            EndCursorColumnIndex = endCursorColumnIndex;
        }
    }
}
