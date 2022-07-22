using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Media;
using CodeHighlighter.Sql;

namespace CodeHighlighter.CodeProvidering;

public class SqlCodeProvider : ICodeProvider
{
    private readonly HashSet<char> _delimiters = new(
        new[] { ' ', ',', '.', ';', '(', ')', '+', '-', '*', '/', '<', '>', '=' });

    private readonly HashSet<string> _keywords;
    private readonly HashSet<string> _functions;
    private readonly HashSet<string> _operators;

    public SqlCodeProvider()
    {
        _keywords = new HashSet<string>(new KeywordsCollection().ToList(), StringComparer.OrdinalIgnoreCase);
        _functions = new HashSet<string>(new FunctionsCollection().ToList(), StringComparer.OrdinalIgnoreCase);
        _operators = new HashSet<string>(new OperatorsCollection().ToList(), StringComparer.OrdinalIgnoreCase);
    }

    public IEnumerable<ICodeProvider.Token> GetTokens(ITextIterator textIterator)
    {
        var tokens = new List<ICodeProvider.Token>();
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
                    tokens.Add(new(textIterator.Char.ToString(), tokenLineIndex, tokenStartColumn, 1, (byte)tokenKind));
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
                    tokens.Add(new(new string(tokenNameArray, 0, tokenNameArrayIndex), tokenLineIndex, tokenStartColumn, tokenNameArrayIndex, (byte)tokenKind));
                    tokenNameArrayIndex = 0;
                    goto case State.General;
                }
                else if (textIterator.Char == '\'' && tokenNameArray[tokenNameArrayIndex - 1] == 'N')
                {
                    tokenLineIndex = textIterator.LineIndex;
                    tokenStartColumn = textIterator.ColumnIndex;
                    tokenKind = TokenKind.Other;
                    tokens.Add(new(new string(tokenNameArray, 0, 1), tokenLineIndex, tokenStartColumn, 1, (byte)tokenKind));
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
                    tokens.Add(new(new string(tokenNameArray, 0, tokenNameArrayIndex), tokenLineIndex, tokenStartColumn, tokenNameArrayIndex, (byte)tokenKind));
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
                    tokens.Add(new(new string(tokenNameArray, 0, tokenNameArrayIndex), tokenLineIndex, tokenStartColumn, tokenNameArrayIndex, (byte)tokenKind));
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
                    tokens.Add(new(new string(tokenNameArray, 0, tokenNameArrayIndex), tokenLineIndex, tokenStartColumn, tokenNameArrayIndex, (byte)tokenKind));
                    tokenNameArrayIndex = 0;
                    textIterator.MoveNext();
                    goto case State.General;
                }
                else if (textIterator.Char == ']')
                {
                    tokenNameArray[tokenNameArrayIndex++] = textIterator.Char;
                    tokens.Add(new(new string(tokenNameArray, 0, tokenNameArrayIndex), tokenLineIndex, tokenStartColumn, tokenNameArrayIndex, (byte)tokenKind));
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
                tokens.Add(new(new string(tokenNameArray, 0, tokenNameArrayIndex), tokenLineIndex, tokenStartColumn, tokenNameArrayIndex, (byte)tokenKind));
                break;
            case State.End:
                tokens.Add(new(new string(tokenNameArray, 0, tokenNameArrayIndex), tokenLineIndex, tokenStartColumn, tokenNameArrayIndex, (byte)tokenKind));
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
        var result = new string(tokenNameArray, 0, tokenNameArrayIndex);
        return _keywords.Contains(result);
    }

    private bool IsOperator(char[] tokenNameArray, int tokenNameArrayIndex)
    {
        var result = new string(tokenNameArray, 0, tokenNameArrayIndex);
        return _operators.Contains(result);
    }

    private bool IsFunction(char[] tokenNameArray, int tokenNameArrayIndex)
    {
        var result = new string(tokenNameArray, 0, tokenNameArrayIndex);
        return _functions.Contains(result);
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

    public IEnumerable<ICodeProvider.TokenColor> GetColors()
    {
        return new[]
        {
            new ICodeProvider.TokenColor((byte)TokenKind.Keyword, Colors.Blue),
            new ICodeProvider.TokenColor((byte)TokenKind.Operator, Colors.DimGray),
            new ICodeProvider.TokenColor((byte)TokenKind.Function, Colors.Magenta),
            new ICodeProvider.TokenColor((byte)TokenKind.Variable, Colors.Brown),
            new ICodeProvider.TokenColor((byte)TokenKind.String, Colors.Red),
            new ICodeProvider.TokenColor((byte)TokenKind.Comment, Colors.Green),
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

        public int GetHashCode(char[] obj)
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
