using System.Linq;

namespace CodeHighlighter.Model
{
    internal class TokenSelector
    {
        public SelectedRange GetToken(ITokens tokens, int lineIndex, int columnIndex)
        {
            if (lineIndex >= tokens.LinesCount) return default;
            var lineTokens = tokens.GetTokens(lineIndex);
            if (columnIndex >= lineTokens.LastOrDefault().EndColumnIndex + 1) return ToSelectedRange(lineTokens.LastOrDefault());
            var index = lineTokens.FindIndex(x => x.StartColumnIndex <= columnIndex && columnIndex <= x.EndColumnIndex + 1);
            if (index != -1)
            {
                if (index + 1 < lineTokens.Count && columnIndex == lineTokens[index + 1].StartColumnIndex) return ToSelectedRange(lineTokens[index + 1]);
                else return ToSelectedRange(lineTokens[index]);
            }
            else
            {
                index = lineTokens.FindIndex(x => x.StartColumnIndex > columnIndex);
                if (index == 0) return ToSelectedRange(lineTokens.First());
                else return new SelectedRange(lineTokens[index - 1].EndColumnIndex + 1, lineTokens[index].StartColumnIndex);
            }
        }

        private SelectedRange ToSelectedRange(Token token)
        {
            return new SelectedRange(token.StartColumnIndex, token.EndColumnIndex + 1);
        }

        public struct SelectedRange
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
}
