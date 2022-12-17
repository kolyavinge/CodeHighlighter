using System;
using CodeHighlighter.Common;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Common;

internal class ColorTest
{
    [Test]
    public void FromHex_Sharp()
    {
        var color = Color.FromHex("#ff32a852");
        Assert.That(color.A, Is.EqualTo(255));
        Assert.That(color.R, Is.EqualTo(50));
        Assert.That(color.G, Is.EqualTo(168));
        Assert.That(color.B, Is.EqualTo(82));
    }

    [Test]
    public void FromHex_NoSharp()
    {
        var color = Color.FromHex("ff32a852");
        Assert.That(color.A, Is.EqualTo(255));
        Assert.That(color.R, Is.EqualTo(50));
        Assert.That(color.G, Is.EqualTo(168));
        Assert.That(color.B, Is.EqualTo(82));
    }

    [Test]
    public void FromHex_SharpNoAlpha()
    {
        var color = Color.FromHex("#32a852");
        Assert.That(color.A, Is.EqualTo(255));
        Assert.That(color.R, Is.EqualTo(50));
        Assert.That(color.G, Is.EqualTo(168));
        Assert.That(color.B, Is.EqualTo(82));
    }

    [Test]
    public void FromHex_NoSharpNoAlpha()
    {
        var color = Color.FromHex("32a852");
        Assert.That(color.A, Is.EqualTo(255));
        Assert.That(color.R, Is.EqualTo(50));
        Assert.That(color.G, Is.EqualTo(168));
        Assert.That(color.B, Is.EqualTo(82));
    }

    [Test]
    public void FromHex_Empty_Error()
    {
        try
        {
            Color.FromHex("");
        }
        catch (ArgumentException e)
        {
            Assert.That(e.Message, Is.EqualTo("Incorrect hex value."));
        }
    }

    [Test]
    public void FromHex_Incorrect()
    {
        try
        {
            Color.FromHex("32a85");
        }
        catch (ArgumentException e)
        {
            Assert.That(e.Message, Is.EqualTo("Incorrect hex value."));
        }
    }

    [Test]
    public void FromHex_Incorrect_2()
    {
        try
        {
            Color.FromHex("32a8");
        }
        catch (ArgumentException e)
        {
            Assert.That(e.Message, Is.EqualTo("Incorrect hex value."));
        }
    }
}
