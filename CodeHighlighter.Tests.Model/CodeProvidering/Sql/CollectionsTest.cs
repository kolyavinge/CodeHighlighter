using System.Collections.Generic;
using System.Linq;
using CodeHighlighter.CodeProvidering.Sql;
using NUnit.Framework;

namespace CodeHighlighter.Tests.CodeProvidering.Sql;

internal class CollectionsTest
{
    [Test]
    public void CollectionsAreUnique()
    {
        var keywords = new HashSet<char[]>(new KeywordsCollection().Select(x => x.ToArray()).ToList());
        var functions = new HashSet<char[]>(new FunctionsCollection().Select(x => x.ToArray()).ToList());
        var operators = new HashSet<char[]>(new OperatorsCollection().Select(x => x.ToArray()).ToList());

        foreach (var keyword in keywords)
        {
            Assert.False(functions.Contains(keyword));
            Assert.False(operators.Contains(keyword));
        }

        foreach (var function in functions)
        {
            Assert.False(operators.Contains(function));
        }
    }
}
