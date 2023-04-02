namespace CodeHighlighter.Core;

internal class TokenSelector
{
    public SelectedRange GetSelection(ITokens tokens, CursorPosition position)
    {
        if (position.LineIndex >= tokens.LinesCount) return default;
        var lineTokens = tokens.GetTokens(position.LineIndex);
        var logic = new TokenCursorPositionLogic();
        var cursor = logic.GetPositionExt(lineTokens, position.ColumnIndex);
        if (cursor.Position == TokenCursorPositionKind.StartLine)
        {
            return ToSelectedRange(cursor.Right);
        }
        else if (cursor.Position == TokenCursorPositionKind.BetweenTokens && position.ColumnIndex == cursor.Right.StartColumnIndex)
        {
            return ToSelectedRange(cursor.Right);
        }
        else if (cursor.Position == TokenCursorPositionKind.BetweenTokens && position.ColumnIndex == cursor.Left.EndColumnIndex + 1)
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

    private SelectedRange ToSelectedRange(Token token) => new(token.StartColumnIndex, token.EndColumnIndex + 1);

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
