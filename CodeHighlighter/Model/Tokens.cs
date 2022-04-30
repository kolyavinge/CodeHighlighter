using System.Collections.Generic;
using System.Linq;

namespace CodeHighlighter.Model
{
    internal interface ITokens
    {
        int LinesCount { get; }
        List<Token> GetTokens(int lineIndex);
        List<MergedToken> GetMergedTokens(int lineIndex);
    }

    internal class Tokens : ITokens
    {
        private readonly List<List<Token>> _tokens = new();
        private readonly List<List<MergedToken>> _mergedTokens = new();

        public int LinesCount => _tokens.Count;

        public void SetTokens(IEnumerable<Token> tokens, int startLineIndex, int linesCount)
        {
            var groupedTokens = tokens
                .GroupBy(x => x.LineIndex)
                .ToDictionary(group => group.Key, group => group.ToList());

            var length = startLineIndex + linesCount;
            for (int lineIndex = startLineIndex; lineIndex < length; lineIndex++)
            {
                var lineTokens = groupedTokens.ContainsKey(lineIndex) ? groupedTokens[lineIndex] : new List<Token>();
                if (lineIndex < _tokens.Count)
                {
                    _tokens[lineIndex] = lineTokens;
                    _mergedTokens[lineIndex] = MergeTokens(lineTokens);
                }
                else
                {
                    _tokens.Add(lineTokens);
                    _mergedTokens.Add(MergeTokens(lineTokens));
                }
            }
        }

        public List<MergedToken> MergeTokens(IEnumerable<Token> tokens)
        {
            var result = new List<MergedToken>();
            var tokensArray = tokens.ToArray();
            int columnIndex = 0;
            int length;
            for (int i = 0; i < tokensArray.Length;)
            {
                var kind = tokensArray[i].Kind;
                i++;
                while (i < tokensArray.Length && kind == tokensArray[i].Kind) i++;
                if (i < tokensArray.Length)
                {
                    length = tokensArray[i].StartColumnIndex - columnIndex;
                    result.Add(new(columnIndex, length, kind));
                    columnIndex = tokensArray[i].StartColumnIndex;
                }
                else
                {
                    var lineLength = tokens.Last().StartColumnIndex + tokens.Last().Length;
                    result.Add(new(columnIndex, lineLength - columnIndex, kind));
                }
            }

            return result;
        }

        public void InsertEmptyLine(int lineIndex)
        {
            _tokens.Insert(lineIndex, new List<Token>());
            _mergedTokens.Insert(lineIndex, new List<MergedToken>());
        }

        public void DeleteLine(int lineIndex)
        {
            if (lineIndex == 0 && !_tokens.Any()) return;
            _tokens.RemoveAt(lineIndex);
            _mergedTokens.RemoveAt(lineIndex);
        }

        public void DeleteLines(int lineIndex, int count)
        {
            _tokens.RemoveRange(lineIndex, count);
            _mergedTokens.RemoveRange(lineIndex, count);
        }

        public List<Token> GetTokens(int lineIndex)
        {
            return _tokens[lineIndex];
        }

        public List<MergedToken> GetMergedTokens(int lineIndex)
        {
            return _mergedTokens[lineIndex];
        }
    }

    internal struct MergedToken
    {
        public readonly int ColumnIndex;
        public readonly int Length;
        public readonly byte Kind;

        public MergedToken(int columnIndex, int length, byte kind)
        {
            ColumnIndex = columnIndex;
            Length = length;
            Kind = kind;
        }
    }
}
