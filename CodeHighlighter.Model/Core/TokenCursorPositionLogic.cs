using System.Collections.Generic;
using System.Linq;

namespace CodeHighlighter.Core;

public class TokenCursorPosition
{
    public readonly Token TokenOnPosition;
    public readonly Token? NeighbourToken;

    public TokenCursorPosition(Token tokenOnPosition, Token? neighbourToken)
    {
        TokenOnPosition = tokenOnPosition;
        NeighbourToken = neighbourToken;
    }

    public override bool Equals(object? obj) => obj is TokenCursorPosition position &&
        EqualityComparer<Token>.Default.Equals(TokenOnPosition, position.TokenOnPosition) &&
        EqualityComparer<Token>.Default.Equals(NeighbourToken ?? Token.Default, position.NeighbourToken ?? Token.Default);

    public override int GetHashCode() => HashCode.Combine(TokenOnPosition, NeighbourToken);
}

internal enum TokenCursorPositionKind
{
    InToken,
    BetweenTokens,
    StartLine,
    EndLine,
}

internal class TokenCursorPositionExt
{
    public static TokenCursorPositionExt Default => new(TokenCursorPositionKind.StartLine, Token.Default, Token.Default);

    public readonly TokenCursorPositionKind Position;
    public readonly Token Left;
    public readonly Token Right;

    public TokenCursorPositionExt(TokenCursorPositionKind position, Token left, Token right)
    {
        Position = position;
        Left = left;
        Right = right;
    }

    public override bool Equals(object? obj) => obj is TokenCursorPositionExt position &&
        Position == position.Position &&
        EqualityComparer<Token>.Default.Equals(Left, position.Left) &&
        EqualityComparer<Token>.Default.Equals(Right, position.Right);

    public override int GetHashCode() => HashCode.Combine(Position, Left, Right);
}

internal class TokenCursorPositionLogic
{
    public TokenCursorPosition? GetPosition(TokenList lineTokens, int columnIndex)
    {
        if (!lineTokens.Any()) return null;
        Token? tokenOnPosition = null, neighbourToken = null;
        for (int i = 0; i < lineTokens.Count; i++)
        {
            var token = lineTokens[i];
            if (token.StartColumnIndex <= columnIndex && columnIndex <= token.EndColumnIndex + 1)
            {
                tokenOnPosition = token;
                if (i + 1 < lineTokens.Count && lineTokens[i + 1].StartColumnIndex == columnIndex)
                {
                    neighbourToken = lineTokens[i + 1];
                }
                break;
            }
        }

        return tokenOnPosition != null ? new(tokenOnPosition, neighbourToken) : null;
    }

    public TokenCursorPositionExt GetPositionExt(TokenList lineTokens, int columnIndex)
    {
        if (!lineTokens.Any()) return TokenCursorPositionExt.Default;
        if (columnIndex >= lineTokens.Last().EndColumnIndex + 1)
        {
            return new(TokenCursorPositionKind.EndLine, lineTokens.Last(), Token.Default);
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
            return new(TokenCursorPositionKind.BetweenTokens, lineTokens[index], lineTokens[index + 1]);
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
}
