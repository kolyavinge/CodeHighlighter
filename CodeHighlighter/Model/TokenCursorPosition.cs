using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeHighlighter.Model;

internal enum TokenCursorPositionKind
{
    InToken,
    BetweenTokens,
    StartLine,
    EndLine,
}

internal class TokenCursorPosition
{
    public static TokenCursorPosition Default => new(TokenCursorPositionKind.StartLine, Token.Default, Token.Default);

    public readonly TokenCursorPositionKind Position;
    public readonly Token Left;
    public readonly Token Right;

    public TokenCursorPosition(TokenCursorPositionKind position, Token left, Token right)
    {
        Position = position;
        Left = left;
        Right = right;
    }

    public static TokenCursorPosition GetPosition(TokenList lineTokens, int columnIndex)
    {
        if (!lineTokens.Any()) return Default;
        if (columnIndex >= lineTokens.LastOrDefault().EndColumnIndex + 1)
        {
            return new(TokenCursorPositionKind.EndLine, lineTokens.LastOrDefault(), Token.Default);
        }
        int index;
        if ((index = lineTokens.FindIndex(x => x.StartColumnIndex < columnIndex && columnIndex < x.EndColumnIndex + 1)) != -1)
        {
            return new(TokenCursorPositionKind.InToken, lineTokens[index], lineTokens[index]);
        }
        else if ((index = lineTokens.FindIndex(x => x.StartColumnIndex == columnIndex)) != -1)
        {
            if (index > 0)
            {
                return new(TokenCursorPositionKind.BetweenTokens, lineTokens[index - 1], lineTokens[index]);
            }
            else
            {
                return new(TokenCursorPositionKind.StartLine, Token.Default, lineTokens[index]);
            }
        }
        else if ((index = lineTokens.FindIndex(x => x.EndColumnIndex + 1 == columnIndex)) != -1)
        {
            if (index < lineTokens.Count)
            {
                return new(TokenCursorPositionKind.BetweenTokens, lineTokens[index], lineTokens[index + 1]);
            }
            else
            {
                return new(TokenCursorPositionKind.InToken, lineTokens[index], lineTokens[index]);
            }
        }
        else
        {
            index = lineTokens.FindIndex(x => x.StartColumnIndex > columnIndex);
            if (index == 0)
            {
                return new(TokenCursorPositionKind.StartLine, Token.Default, lineTokens.First());
            }
            else
            {
                return new(TokenCursorPositionKind.BetweenTokens, lineTokens[index - 1], lineTokens[index]);
            }
        }
    }

    public override bool Equals(object? obj)
    {
        return obj is TokenCursorPosition position &&
               Position == position.Position &&
               EqualityComparer<Token>.Default.Equals(Left, position.Left) &&
               EqualityComparer<Token>.Default.Equals(Right, position.Right);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Position, Left, Right);
    }
}
