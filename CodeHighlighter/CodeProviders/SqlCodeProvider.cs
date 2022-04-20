﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Media;
using CodeHighlighter.Sql;

namespace CodeHighlighter.CodeProviders
{
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

        public IEnumerable<Lexem> GetLexems(ITextIterator textIterator)
        {
            var lexems = new List<Lexem>();
            if (textIterator.Eof) return lexems;
            var lexemNameArray = new char[10 * 1024];
            int lexemNameArrayIndex = 0;
            int lexemLineIndex = 0;
            int lexemStartColumn = 0;
            var lexemKind = LexemKind.Other;
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
                        lexemNameArray[lexemNameArrayIndex++] = '-';
                        lexemNameArray[lexemNameArrayIndex++] = '-';
                        lexemLineIndex = textIterator.LineIndex;
                        lexemStartColumn = textIterator.ColumnIndex;
                        lexemKind = LexemKind.Comment;
                        textIterator.MoveNext();
                        textIterator.MoveNext();
                        goto case State.Comment;
                    }
                    else if (textIterator.Char == '\'') // string start
                    {
                        //if (lexems.Any() && lexems.Last().Name == "N")
                        //{
                        //    lexems.Last().Kind = LexemKind.Other;
                        //}
                        lexemLineIndex = textIterator.LineIndex;
                        lexemStartColumn = textIterator.ColumnIndex;
                        lexemKind = LexemKind.String;
                        lexemNameArray[lexemNameArrayIndex++] = textIterator.Char;
                        textIterator.MoveNext();
                        goto case State.String;
                    }
                    else if (textIterator.Char == '[')
                    {
                        lexemNameArray[lexemNameArrayIndex++] = '[';
                        lexemLineIndex = textIterator.LineIndex;
                        lexemStartColumn = textIterator.ColumnIndex;
                        lexemKind = LexemKind.Identifier;
                        textIterator.MoveNext();
                        goto case State.Identifier;
                    }
                    else if (IsDelimiter(textIterator.Char))
                    {
                        lexemLineIndex = textIterator.LineIndex;
                        lexemStartColumn = textIterator.ColumnIndex;
                        lexemKind = LexemKind.Delimiter;
                        lexems.Add(new(lexemLineIndex, lexemStartColumn, (byte)lexemKind));
                        textIterator.MoveNext();
                        goto case State.General;
                    }
                    else
                    {
                        lexemLineIndex = textIterator.LineIndex;
                        lexemStartColumn = textIterator.ColumnIndex;
                        lexemNameArray[lexemNameArrayIndex++] = textIterator.Char;
                        textIterator.MoveNext();
                        goto case State.KeywordFunctionOther;
                    }
                case State.KeywordFunctionOther: // keyword, function or other name
                    if (textIterator.Eof) goto case State.EndUnknownLexem;
                    if (IsSpace(textIterator.Char) || IsReturn(textIterator.Char) || IsDelimiter(textIterator.Char))
                    {
                        lexemKind = GetLexemKind(lexemNameArray, lexemNameArrayIndex);
                        lexems.Add(new(lexemLineIndex, lexemStartColumn, (byte)lexemKind));
                        lexemNameArrayIndex = 0;
                        goto case State.General;
                    }
                    else if (textIterator.Char == '\'' && lexemNameArray[lexemNameArrayIndex - 1] == 'N')
                    {
                        lexemLineIndex = textIterator.LineIndex;
                        lexemStartColumn = textIterator.ColumnIndex;
                        lexemKind = LexemKind.Other;
                        lexems.Add(new(lexemLineIndex, lexemStartColumn, (byte)lexemKind));
                        lexemNameArrayIndex = 0;
                        goto case State.General;
                    }
                    else
                    {
                        lexemNameArray[lexemNameArrayIndex++] = textIterator.Char;
                        textIterator.MoveNext();
                        goto case State.KeywordFunctionOther;
                    }
                case State.Comment: // comment
                    if (textIterator.Eof) goto case State.End;
                    if (IsReturn(textIterator.Char))
                    {
                        lexems.Add(new(lexemLineIndex, lexemStartColumn, (byte)lexemKind));
                        lexemNameArrayIndex = 0;
                        textIterator.MoveNext();
                        goto case State.General;
                    }
                    else
                    {
                        lexemNameArray[lexemNameArrayIndex++] = textIterator.Char;
                        textIterator.MoveNext();
                        goto case State.Comment;
                    }
                case State.String: // string
                    if (textIterator.Eof) goto case State.End;
                    if (textIterator.Char == '\'')
                    {
                        lexemNameArray[lexemNameArrayIndex++] = textIterator.Char;
                        lexems.Add(new(lexemLineIndex, lexemStartColumn, (byte)lexemKind));
                        lexemNameArrayIndex = 0;
                        textIterator.MoveNext();
                        goto case State.General;
                    }
                    else
                    {
                        lexemNameArray[lexemNameArrayIndex++] = textIterator.Char;
                        textIterator.MoveNext();
                        goto case State.String;
                    }
                case State.Identifier:
                    if (textIterator.Eof) goto case State.End;
                    if (IsReturn(textIterator.Char))
                    {
                        lexems.Add(new(lexemLineIndex, lexemStartColumn, (byte)lexemKind));
                        lexemNameArrayIndex = 0;
                        textIterator.MoveNext();
                        goto case State.General;
                    }
                    else if (textIterator.Char == ']')
                    {
                        lexemNameArray[lexemNameArrayIndex++] = textIterator.Char;
                        lexems.Add(new(lexemLineIndex, lexemStartColumn, (byte)lexemKind));
                        lexemNameArrayIndex = 0;
                        textIterator.MoveNext();
                        goto case State.General;
                    }
                    else
                    {
                        lexemNameArray[lexemNameArrayIndex++] = textIterator.Char;
                        textIterator.MoveNext();
                        goto case State.Identifier;
                    }
                case State.EndUnknownLexem:
                    lexemKind = GetLexemKind(lexemNameArray, lexemNameArrayIndex);
                    lexems.Add(new(lexemLineIndex, lexemStartColumn, (byte)lexemKind));
                    break;
                case State.End:
                    lexems.Add(new(lexemLineIndex, lexemStartColumn, (byte)lexemKind));
                    break;
            }

            return lexems;
        }

        private LexemKind GetLexemKind(char[] lexemNameArray, int lexemNameArrayIndex)
        {
            if (IsKeyword(lexemNameArray, lexemNameArrayIndex)) return LexemKind.Keyword;
            if (IsOperator(lexemNameArray, lexemNameArrayIndex)) return LexemKind.Operator;
            if (IsFunction(lexemNameArray, lexemNameArrayIndex)) return LexemKind.Function;
            if (IsVariable(lexemNameArray)) return LexemKind.Variable;
            if (IsIdentifier(lexemNameArray)) return LexemKind.Identifier;

            return LexemKind.Other;
        }

        private bool IsKeyword(char[] lexemNameArray, int lexemNameArrayIndex)
        {
            return _keywords.Contains(lexemNameArray, new CharArrayEqualityComparer(lexemNameArrayIndex));
        }

        private bool IsOperator(char[] lexemNameArray, int lexemNameArrayIndex)
        {
            return _operators.Contains(lexemNameArray, new CharArrayEqualityComparer(lexemNameArrayIndex));
        }

        private bool IsFunction(char[] lexemNameArray, int lexemNameArrayIndex)
        {
            return _functions.Contains(lexemNameArray, new CharArrayEqualityComparer(lexemNameArrayIndex));
        }

        private bool IsVariable(char[] lexemNameArray)
        {
            return lexemNameArray[0] == '@';
        }

        private bool IsIdentifier(char[] lexemNameArray)
        {
            return Char.IsLetter(lexemNameArray[0]) || lexemNameArray[0] == '_' || lexemNameArray[0] == '#';
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

        public IEnumerable<LexemColor> GetColors()
        {
            return new[]
            {
                new LexemColor((byte)LexemKind.Keyword, Colors.Blue),
                new LexemColor((byte)LexemKind.Operator, Colors.DimGray),
                new LexemColor((byte)LexemKind.Function, Colors.Magenta),
                new LexemColor((byte)LexemKind.Variable, Colors.Brown),
                new LexemColor((byte)LexemKind.String, Colors.Red),
                new LexemColor((byte)LexemKind.Comment, Colors.Green),
            };
        }

        enum State
        {
            General,
            KeywordFunctionOther,
            Identifier,
            Comment,
            String,
            EndUnknownLexem,
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

    internal enum LexemKind : byte
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
}
