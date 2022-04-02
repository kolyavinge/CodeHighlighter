using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public SqlCodeProvider()
        {
            _keywords = new HashSet<char[]>(new KeywordsCollection().Select(x => x.ToArray()).ToList());
            _functions = new HashSet<char[]>(new FunctionsCollection().Select(x => x.ToArray()).ToList());
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
                    if (textIterator.IsReturn || textIterator.IsSpace)
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
                        lexems.Add(new(lexemLineIndex, lexemStartColumn, lexemKind));
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
                    if (textIterator.IsSpace || textIterator.IsReturn || IsDelimiter(textIterator.Char))
                    {
                        lexemKind = GetLexemKind(lexemNameArray, lexemNameArrayIndex);
                        lexems.Add(new(lexemLineIndex, lexemStartColumn, lexemKind));
                        lexemNameArrayIndex = 0;
                        goto case State.General;
                    }
                    else if (textIterator.Char == '\'' && lexemNameArray[lexemNameArrayIndex - 1] == 'N')
                    {
                        lexemLineIndex = textIterator.LineIndex;
                        lexemStartColumn = textIterator.ColumnIndex;
                        lexemKind = LexemKind.Other;
                        lexems.Add(new(lexemLineIndex, lexemStartColumn, lexemKind));
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
                    if (textIterator.IsReturn)
                    {
                        lexems.Add(new(lexemLineIndex, lexemStartColumn, lexemKind));
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
                        lexems.Add(new(lexemLineIndex, lexemStartColumn, lexemKind));
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
                    if (textIterator.IsReturn)
                    {
                        lexems.Add(new(lexemLineIndex, lexemStartColumn, lexemKind));
                        lexemNameArrayIndex = 0;
                        textIterator.MoveNext();
                        goto case State.General;
                    }
                    else if (textIterator.Char == ']')
                    {
                        lexemNameArray[lexemNameArrayIndex++] = textIterator.Char;
                        lexems.Add(new(lexemLineIndex, lexemStartColumn, lexemKind));
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
                    lexems.Add(new(lexemLineIndex, lexemStartColumn, lexemKind));
                    break;
                case State.End:
                    lexems.Add(new(lexemLineIndex, lexemStartColumn, lexemKind));
                    break;
            }

            return lexems;
        }

        private LexemKind GetLexemKind(char[] lexemNameArray, int lexemNameArrayIndex)
        {
            if (IsKeyword(lexemNameArray, lexemNameArrayIndex)) return LexemKind.Keyword;
            if (IsFunction(lexemNameArray, lexemNameArrayIndex)) return LexemKind.Function;
            if (IsVariable(lexemNameArray)) return LexemKind.Variable;
            if (IsIdentifier(lexemNameArray)) return LexemKind.Identifier;

            return LexemKind.Other;
        }

        private bool IsKeyword(char[] lexemNameArray, int lexemNameArrayIndex)
        {
            return _keywords.Contains(lexemNameArray, new CharArrayEqualityComparer(lexemNameArrayIndex));
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

        public IEnumerable<LexemColor> GetColors()
        {
            return new[]
            {
                new LexemColor(LexemKind.Keyword, Colors.Blue),
                new LexemColor(LexemKind.Function, Colors.Magenta),
                new LexemColor(LexemKind.Variable, Colors.Brown),
                new LexemColor(LexemKind.String, Colors.Red),
                new LexemColor(LexemKind.Comment, Colors.Green),
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
}
