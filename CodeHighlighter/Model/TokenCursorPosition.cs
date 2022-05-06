using System.Collections.Generic;
using System.Linq;

namespace CodeHighlighter.Model
{
    internal enum TokenCursorPositionKind
    {
        InToken,
        BetweenTokens,
        StartLine,
        EndLine,
    }

    internal struct TokenCursorPosition
    {
        public readonly TokenCursorPositionKind Position;
        public readonly Token Left;
        public readonly Token Right;

        public TokenCursorPosition(TokenCursorPositionKind position, Token left, Token right)
        {
            Position = position;
            Left = left;
            Right = right;
        }

        public static TokenCursorPosition GetPosition(List<Token> lineTokens, int lineIndex, int columnIndex)
        {
            if (!lineTokens.Any()) return default;
            if (columnIndex >= lineTokens.LastOrDefault().EndColumnIndex + 1)
            {
                return new(TokenCursorPositionKind.EndLine, lineTokens.LastOrDefault(), default);
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
                    return new(TokenCursorPositionKind.StartLine, default, lineTokens[index]);
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
                    return new(TokenCursorPositionKind.StartLine, default, lineTokens.First());
                }
                else
                {
                    return new(TokenCursorPositionKind.BetweenTokens, lineTokens[index - 1], lineTokens[index]);
                }
            }
        }
    }
}
