using CodeHighlighter.Model;
using Moq;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Model;

internal class TokenSelectorTest
{
    private Mock<ITokens> _tokens;
    private TokenSelector _selector;

    [SetUp]
    public void Setup()
    {
        _tokens = new Mock<ITokens>();
        _selector = new TokenSelector();
    }

    [Test]
    public void GetSelection_Empty()
    {
        _tokens.Setup(x => x.GetTokens(0)).Returns(new TokenList());
        _tokens.SetupGet(x => x.LinesCount).Returns(0);
        Assert.AreEqual(default(TokenSelector.SelectedRange), _selector.GetSelection(_tokens.Object, new(0, 0)));
        Assert.AreEqual(default(TokenSelector.SelectedRange), _selector.GetSelection(_tokens.Object, new(0, 1)));
        Assert.AreEqual(default(TokenSelector.SelectedRange), _selector.GetSelection(_tokens.Object, new(0, 10)));
        Assert.AreEqual(default(TokenSelector.SelectedRange), _selector.GetSelection(_tokens.Object, new(1, 0)));
        Assert.AreEqual(default(TokenSelector.SelectedRange), _selector.GetSelection(_tokens.Object, new(1, 1)));
        Assert.AreEqual(default(TokenSelector.SelectedRange), _selector.GetSelection(_tokens.Object, new(1, 10)));
    }

    [Test]
    public void GetSelection_EmptyLine()
    {
        _tokens.Setup(x => x.GetTokens(0)).Returns(new TokenList());
        _tokens.SetupGet(x => x.LinesCount).Returns(1);
        Assert.AreEqual(default(TokenSelector.SelectedRange), _selector.GetSelection(_tokens.Object, new(0, 0)));
        Assert.AreEqual(default(TokenSelector.SelectedRange), _selector.GetSelection(_tokens.Object, new(0, 1)));
        Assert.AreEqual(default(TokenSelector.SelectedRange), _selector.GetSelection(_tokens.Object, new(0, 10)));
        Assert.AreEqual(default(TokenSelector.SelectedRange), _selector.GetSelection(_tokens.Object, new(1, 0)));
        Assert.AreEqual(default(TokenSelector.SelectedRange), _selector.GetSelection(_tokens.Object, new(1, 1)));
        Assert.AreEqual(default(TokenSelector.SelectedRange), _selector.GetSelection(_tokens.Object, new(1, 10)));
    }

    [Test]
    public void GetSelection()
    {
        // '  xx  yzz'
        var tokens = new TokenList
        {
            new("xx", 2, 2, 0), // x
            new("y", 6, 1, 1), // y
            new("zz", 7, 2, 2), // z
        };
        _tokens.SetupGet(x => x.LinesCount).Returns(1);
        _tokens.Setup(x => x.GetTokens(0)).Returns(tokens);

        Assert.AreEqual(new TokenSelector.SelectedRange(2, 4), _selector.GetSelection(_tokens.Object, new(0, 0)));
        Assert.AreEqual(new TokenSelector.SelectedRange(2, 4), _selector.GetSelection(_tokens.Object, new(0, 1)));
        Assert.AreEqual(new TokenSelector.SelectedRange(2, 4), _selector.GetSelection(_tokens.Object, new(0, 2)));
        Assert.AreEqual(new TokenSelector.SelectedRange(2, 4), _selector.GetSelection(_tokens.Object, new(0, 3)));
        Assert.AreEqual(new TokenSelector.SelectedRange(2, 4), _selector.GetSelection(_tokens.Object, new(0, 4)));

        Assert.AreEqual(new TokenSelector.SelectedRange(4, 6), _selector.GetSelection(_tokens.Object, new(0, 5)));

        Assert.AreEqual(new TokenSelector.SelectedRange(6, 7), _selector.GetSelection(_tokens.Object, new(0, 6)));

        Assert.AreEqual(new TokenSelector.SelectedRange(7, 9), _selector.GetSelection(_tokens.Object, new(0, 7)));
        Assert.AreEqual(new TokenSelector.SelectedRange(7, 9), _selector.GetSelection(_tokens.Object, new(0, 8)));
        Assert.AreEqual(new TokenSelector.SelectedRange(7, 9), _selector.GetSelection(_tokens.Object, new(0, 9)));
        Assert.AreEqual(new TokenSelector.SelectedRange(7, 9), _selector.GetSelection(_tokens.Object, new(0, 50)));
    }
}
