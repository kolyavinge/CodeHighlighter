using System;
using System.Collections.Generic;
using System.Text;

namespace CodeHighlighter.Model
{
    internal class TokenNavigator
    {
        internal struct NewCursorPosition
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
                return GetNextCursorPosition(text, lineTokens, cursor.Right);
            }
            else if (cursor.Position == TokenCursorPositionKind.StartLine)
            {
                return ToNewCursorPosition(cursor.Right);
            }
            else if (cursor.Position == TokenCursorPositionKind.BetweenTokens && columnIndex == cursor.Right.StartColumnIndex)
            {
                return GetNextCursorPosition(text, lineTokens, cursor.Right);
            }
            else if (cursor.Position == TokenCursorPositionKind.BetweenTokens)
            {
                return ToNewCursorPosition(cursor.Right);
            }
            else if (cursor.Position == TokenCursorPositionKind.InToken)
            {
                return GetNextCursorPosition(text, lineTokens, cursor.Right);
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
                return ToNewCursorPosition(cursor.Left);
            }
            else if (cursor.Position == TokenCursorPositionKind.InToken)
            {
                return ToNewCursorPosition(cursor.Left);
            }
            else // TokenCursorPositionKind.EndLine
            {
                return ToNewCursorPosition(cursor.Left);
            }
        }

        private NewCursorPosition GetNextCursorPosition(IText text, List<Token> tokens, Token token)
        {
            var index = tokens.FindIndex(x => x.Equals(token));
            if (index == -1 || index == tokens.Count - 1) return new(token.LineIndex, text.GetLine(token.LineIndex).Length);
            var next = tokens[index + 1];
            return new(next.LineIndex, next.StartColumnIndex);
        }

        private NewCursorPosition ToNewCursorPosition(Token token)
        {
            return new(token.LineIndex, token.StartColumnIndex);
        }
    }
}
