using System.Collections.Generic;

namespace CodeHighlighter.Model;

internal class TokenNavigator
{
    internal readonly struct NewCursorPosition
    {
        public readonly int LineIndex;
        public readonly int ColumnIndex;

        public NewCursorPosition(int lineIndex, int columnIndex)
        {
            LineIndex = lineIndex;
            ColumnIndex = columnIndex;
        }
    }

    public NewCursorPosition MoveRight(IText text, ITokens tokens, int lineIndex, int columnIndex)
    {
        var line = text.GetLine(lineIndex);
        if (columnIndex == line.Length)
        {
            if (lineIndex + 1 < text.LinesCount) return new(lineIndex + 1, 0);
            else return new(lineIndex, columnIndex);
        }
        var lineTokens = tokens.GetTokens(lineIndex);
        var cursor = TokenCursorPosition.GetPosition(lineTokens, lineIndex, columnIndex);
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

    public NewCursorPosition MoveLeft(IText text, ITokens tokens, int lineIndex, int columnIndex)
    {
        if (columnIndex == 0)
        {
            if (lineIndex > 0) return new(lineIndex - 1, text.GetLine(lineIndex - 1).Length);
            else return new(lineIndex, columnIndex);
        }
        var lineTokens = tokens.GetTokens(lineIndex);
        var cursor = TokenCursorPosition.GetPosition(lineTokens, lineIndex, columnIndex);
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

    private NewCursorPosition GetNextCursorPosition(IText text, int lineIndex, List<LineToken> lineTokens, LineToken token)
    {
        var index = lineTokens.FindIndex(x => x.Equals(token));
        if (index == -1 || index == lineTokens.Count - 1) return new(lineIndex, text.GetLine(lineIndex).Length);
        var next = lineTokens[index + 1];
        return new(lineIndex, next.StartColumnIndex);
    }

    private NewCursorPosition ToNewCursorPosition(int lineIndex, LineToken token)
    {
        return new(lineIndex, token.StartColumnIndex);
    }
}
