namespace CodeHighlighter.Model;

internal class TokenNavigator
{
    private readonly TokenCursorPositionLogic _tokenCursorPositionLogic = new();

    public CursorPosition MoveRight(IText text, ITokens tokens, int lineIndex, int columnIndex)
    {
        var line = text.GetLine(lineIndex);
        if (columnIndex == line.Length)
        {
            if (lineIndex + 1 < text.LinesCount) return new(lineIndex + 1, 0);
            else return new(lineIndex, columnIndex);
        }
        var lineTokens = tokens.GetTokens(lineIndex);
        var cursor = _tokenCursorPositionLogic.GetPositionExt(lineTokens, columnIndex);
        if (cursor.Position == TokenCursorPositionKind.StartLine && columnIndex == cursor.Right.StartColumnIndex)
        {
            return GetNextCursorPosition(text, lineIndex, lineTokens, cursor.Right);
        }
        else if (cursor.Position == TokenCursorPositionKind.StartLine)
        {
            return ToNewCursorPosition(lineIndex, cursor.Right);
        }
        else if (cursor.Position == TokenCursorPositionKind.BetweenTokens && columnIndex == cursor.Right.StartColumnIndex)
        {
            return GetNextCursorPosition(text, lineIndex, lineTokens, cursor.Right);
        }
        else if (cursor.Position == TokenCursorPositionKind.BetweenTokens)
        {
            return ToNewCursorPosition(lineIndex, cursor.Right);
        }
        else if (cursor.Position == TokenCursorPositionKind.InToken)
        {
            return GetNextCursorPosition(text, lineIndex, lineTokens, cursor.Right);
        }
        else // TokenCursorPositionKind.EndLine
        {
            return new(lineIndex, text.GetLine(lineIndex).Length);
        }
    }

    public CursorPosition MoveLeft(IText text, ITokens tokens, int lineIndex, int columnIndex)
    {
        if (columnIndex == 0)
        {
            if (lineIndex > 0) return new(lineIndex - 1, text.GetLine(lineIndex - 1).Length);
            else return new(lineIndex, columnIndex);
        }
        var lineTokens = tokens.GetTokens(lineIndex);
        var cursor = _tokenCursorPositionLogic.GetPositionExt(lineTokens, columnIndex);
        if (cursor.Position == TokenCursorPositionKind.StartLine)
        {
            return new(lineIndex, 0);
        }
        else if (cursor.Position == TokenCursorPositionKind.BetweenTokens)
        {
            return ToNewCursorPosition(lineIndex, cursor.Left);
        }
        else if (cursor.Position == TokenCursorPositionKind.InToken)
        {
            return ToNewCursorPosition(lineIndex, cursor.Left);
        }
        else // TokenCursorPositionKind.EndLine
        {
            return ToNewCursorPosition(lineIndex, cursor.Left);
        }
    }

    private CursorPosition GetNextCursorPosition(IText text, int lineIndex, TokenList lineTokens, Token token)
    {
        var index = lineTokens.FindIndex(x => x.Equals(token));
        if (index == -1 || index == lineTokens.Count - 1) return new(lineIndex, text.GetLine(lineIndex).Length);
        var next = lineTokens[index + 1];
        return new(lineIndex, next.StartColumnIndex);
    }

    private CursorPosition ToNewCursorPosition(int lineIndex, Token token) => new(lineIndex, token.StartColumnIndex);
}
