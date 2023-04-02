using System.Collections;
using System.Collections.Generic;

namespace CodeHighlighter.Model;

public class TokenList : IEnumerable<Token>, IReadOnlyCollection<Token>, IReadOnlyList<Token>
{
    public static TokenList FromEnumerable(IEnumerable<Token> tokens) => new(tokens);

    private readonly List<Token> _list = new();

    public int Count => _list.Count;

    public Token this[int index] => _list[index];

    internal TokenList() { }

    private TokenList(IEnumerable<Token> tokens)
    {
        foreach (var token in tokens)
        {
            Add(token);
        }
    }

    internal void Add(Token token) => _list.Add(token);

    internal int FindIndex(Predicate<Token> match) => _list.FindIndex(match);

    internal void Clear() => _list.Clear();

    public IEnumerator<Token> GetEnumerator() => _list.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
