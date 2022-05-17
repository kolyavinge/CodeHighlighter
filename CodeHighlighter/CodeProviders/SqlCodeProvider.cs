using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Media;
using CodeHighlighter.Sql;

namespace CodeHighlighter.CodeProviders;

public class SqlCodeProvider : ICodeProvider
{
    private readonly HashSet<char> _delimiters = new(
        new[] { ' ', ',', '.', ';', '(', ')', '+', '-', '*', '/', '<', '>', '=' });

    private readonly HashSet<char[]> _keywords;
    private readonly HashSet<char[]> _functions;
    private readonly HashSet<char[]> _operators;

    public SqlCodeProvider()
    {
        _keywords = new HashSet<char[]>(new KeywordsCollection().Select(x => x.ToArray()).ToList());
        _functions = new HashSet<char[]>(new FunctionsCollection().Select(x => x.ToArray()).ToList());
        _operators = new HashSet<char[]>(new OperatorsCollection().Select(x => x.ToArray()).ToList());
    }

    public IEnumerable<Token> GetTokens(ITextIterator textIterator)
    {
        var tokens = new List<Token>();
        if (textIterator.Eof) return tokens;
        var tokenNameArray = new char[10 * 1024];
        int tokenNameArrayIndex = 0;
        int tokenLineIndex;
        int tokenStartColumn;
        TokenKind tokenKind;
        switch (State.General)
        {
            case State.General:
                if (textIterator.Eof) break;
                if (IsReturn(textIterator.Char) || IsSpace(textIterator.Char))
                {
                    textIterator.MoveNext();
                    goto case State.General;
                }
                else if (textIterator.Char == '-' && textIterator.NextChar == '-') // comment start
                {
                    tokenNameArray[tokenNameArrayIndex++] = '-';
                    tokenNameArray[tokenNameArrayIndex++] = '-';
                    tokenLineIndex = textIterator.LineIndex;
                    tokenStartColumn = textIterator.ColumnIndex;
                    tokenKind = TokenKind.Comment;
                    textIterator.MoveNext();
                    textIterator.MoveNext();
                    goto case State.Comment;
                }
                else if (textIterator.Char == '\'') // string start
                {
                    //if (tokens.Any() && tokens.Last().Name == "N")
                    //{
                    //    tokens.Last().Kind = TokenKind.Other;
                    //}
                    tokenLineIndex = textIterator.LineIndex;
                    tokenStartColumn = textIterator.ColumnIndex;
                    tokenKind = TokenKind.String;
                    tokenNameArray[tokenNameArrayIndex++] = textIterator.Char;
                    textIterator.MoveNext();
                    goto case State.String;
                }
                else if (textIterator.Char == '[')
                {
                    tokenNameArray[tokenNameArrayIndex++] = '[';
                    tokenLineIndex = textIterator.LineIndex;
                    tokenStartColumn = textIterator.ColumnIndex;
                    tokenKind = TokenKind.Identifier;
                    textIterator.MoveNext();
                    goto case State.Identifier;
                }
                else if (IsDelimiter(textIterator.Char))
                {
                    tokenLineIndex = textIterator.LineIndex;
                    tokenStartColumn = textIterator.ColumnIndex;
                    tokenKind = TokenKind.Delimiter;
                    tokens.Add(new(tokenLineIndex, tokenStartColumn, 1, (byte)tokenKind));
                    textIterator.MoveNext();
                    goto case State.General;
                }
                else
                {
                    tokenLineIndex = textIterator.LineIndex;
                    tokenStartColumn = textIterator.ColumnIndex;
                    tokenNameArray[tokenNameArrayIndex++] = textIterator.Char;
                    textIterator.MoveNext();
                    goto case State.KeywordFunctionOther;
                }
            case State.KeywordFunctionOther: // keyword, function or other name
                if (textIterator.Eof) goto case State.EndUnknownToken;
                if (IsSpace(textIterator.Char) || IsReturn(textIterator.Char) || IsDelimiter(textIterator.Char))
                {
                    tokenKind = GetTokenKind(tokenNameArray, tokenNameArrayIndex);
                    tokens.Add(new(tokenLineIndex, tokenStartColumn, tokenNameArrayIndex, (byte)tokenKind));
                    tokenNameArrayIndex = 0;
                    goto case State.General;
                }
                else if (textIterator.Char == '\'' && tokenNameArray[tokenNameArrayIndex - 1] == 'N')
                {
                    tokenLineIndex = textIterator.LineIndex;
                    tokenStartColumn = textIterator.ColumnIndex;
                    tokenKind = TokenKind.Other;
                    tokens.Add(new(tokenLineIndex, tokenStartColumn, 1, (byte)tokenKind));
                    tokenNameArrayIndex = 0;
                    goto case State.General;
                }
                else
                {
                    tokenNameArray[tokenNameArrayIndex++] = textIterator.Char;
                    textIterator.MoveNext();
                    goto case State.KeywordFunctionOther;
                }
            case State.Comment: // comment
                if (textIterator.Eof) goto case State.End;
                if (IsReturn(textIterator.Char))
                {
                    tokens.Add(new(tokenLineIndex, tokenStartColumn, tokenNameArrayIndex, (byte)tokenKind));
                    tokenNameArrayIndex = 0;
                    textIterator.MoveNext();
                    goto case State.General;
                }
                else
                {
                    tokenNameArray[tokenNameArrayIndex++] = textIterator.Char;
                    textIterator.MoveNext();
                    goto case State.Comment;
                }
            case State.String: // string
                if (textIterator.Eof) goto case State.End;
                if (textIterator.Char == '\'')
                {
                    tokenNameArray[tokenNameArrayIndex++] = textIterator.Char;
                    tokens.Add(new(tokenLineIndex, tokenStartColumn, tokenNameArrayIndex, (byte)tokenKind));
                    tokenNameArrayIndex = 0;
                    textIterator.MoveNext();
                    goto case State.General;
                }
                else
                {
                    tokenNameArray[tokenNameArrayIndex++] = textIterator.Char;
                    textIterator.MoveNext();
                    goto case State.String;
                }
            case State.Identifier:
                if (textIterator.Eof) goto case State.End;
                if (IsReturn(textIterator.Char))
                {
                    tokens.Add(new(tokenLineIndex, tokenStartColumn, tokenNameArrayIndex, (byte)tokenKind));
                    tokenNameArrayIndex = 0;
                    textIterator.MoveNext();
                    goto case State.General;
                }
                else if (textIterator.Char == ']')
                {
                    tokenNameArray[tokenNameArrayIndex++] = textIterator.Char;
                    tokens.Add(new(tokenLineIndex, tokenStartColumn, tokenNameArrayIndex, (byte)tokenKind));
                    tokenNameArrayIndex = 0;
                    textIterator.MoveNext();
                    goto case State.General;
                }
                else
                {
                    tokenNameArray[tokenNameArrayIndex++] = textIterator.Char;
                    textIterator.MoveNext();
                    goto case State.Identifier;
                }
            case State.EndUnknownToken:
                tokenKind = GetTokenKind(tokenNameArray, tokenNameArrayIndex);
                tokens.Add(new(tokenLineIndex, tokenStartColumn, tokenNameArrayIndex, (byte)tokenKind));
                break;
            case State.End:
                tokens.Add(new(tokenLineIndex, tokenStartColumn, tokenNameArrayIndex, (byte)tokenKind));
                break;
        }

        return tokens;
    }

    private TokenKind GetTokenKind(char[] tokenNameArray, int tokenNameArrayIndex)
    {
        if (IsKeyword(tokenNameArray, tokenNameArrayIndex)) return TokenKind.Keyword;
        if (IsOperator(tokenNameArray, tokenNameArrayIndex)) return TokenKind.Operator;
        if (IsFunction(tokenNameArray, tokenNameArrayIndex)) return TokenKind.Function;
        if (IsVariable(tokenNameArray)) return TokenKind.Variable;
        if (IsIdentifier(tokenNameArray)) return TokenKind.Identifier;

        return TokenKind.Other;
    }

    private bool IsKeyword(char[] tokenNameArray, int tokenNameArrayIndex)
    {
        return _keywords.Contains(tokenNameArray, new CharArrayEqualityComparer(tokenNameArrayIndex));
    }

    private bool IsOperator(char[] tokenNameArray, int tokenNameArrayIndex)
    {
        return _operators.Contains(tokenNameArray, new CharArrayEqualityComparer(tokenNameArrayIndex));
    }

    private bool IsFunction(char[] tokenNameArray, int tokenNameArrayIndex)
    {
        return _functions.Contains(tokenNameArray, new CharArrayEqualityComparer(tokenNameArrayIndex));
    }

    private bool IsVariable(char[] tokenNameArray)
    {
        return tokenNameArray[0] == '@';
    }

    private bool IsIdentifier(char[] tokenNameArray)
    {
        return Char.IsLetter(tokenNameArray[0]) || tokenNameArray[0] == '_' || tokenNameArray[0] == '#';
    }

    private bool IsDelimiter(char ch)
    {
        return _delimiters.Contains(ch);
    }

    private bool IsSpace(char ch)
    {
        return ch == ' ' || ch == '\t';
    }

    private bool IsReturn(char ch)
    {
        return ch == '\n';
    }

    public IEnumerable<TokenColor> GetColors()
    {
        return new[]
        {
            new TokenColor((byte)TokenKind.Keyword, Colors.Blue),
            new TokenColor((byte)TokenKind.Operator, Colors.DimGray),
            new TokenColor((byte)TokenKind.Function, Colors.Magenta),
            new TokenColor((byte)TokenKind.Variable, Colors.Brown),
            new TokenColor((byte)TokenKind.String, Colors.Red),
            new TokenColor((byte)TokenKind.Comment, Colors.Green),
        };
    }

    enum State
    {
        General,
        KeywordFunctionOther,
        Identifier,
        Comment,
        String,
        EndUnknownToken,
        End
    }

    class CharArrayEqualityComparer : IEqualityComparer<char[]>
    {
        private readonly int _arrayLength;

        public CharArrayEqualityComparer(int arrayLength)
        {
            _arrayLength = arrayLength;
        }

        public bool Equals([AllowNull] char[] x, [AllowNull] char[] y)
        {
            if (x == null || y == null) return false;
            var length = Math.Min(x.Length, y.Length);
            if (length != _arrayLength) return false;
            for (int i = 0; i < length; i++)
            {
                if (Char.ToUpper(x[i]) != Char.ToUpper(y[i])) return false;
            }

            return true;
        }

        public int GetHashCode([DisallowNull] char[] obj)
        {
            return obj.Select(x => x.GetHashCode()).Sum();
        }
    }
}

internal enum TokenKind : byte
{
    Identifier,
    Keyword,
    Operator,
    Function,
    Method,
    Property,
    Variable,
    Constant,
    String,
    Comment,
    Delimiter,
    Other
}
