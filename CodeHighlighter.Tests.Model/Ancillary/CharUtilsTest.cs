using CodeHighlighter.Ancillary;
using NUnit.Framework;

namespace CodeHighlighter.Tests.Ancillary;

internal class CharUtilsTest
{
    [Test]
    public void IsCharEqualIgnoreCase_Ascii()
    {
        Assert.True(CharUtils.IsCharEqualIgnoreCase('X', 'X'));
        Assert.True(CharUtils.IsCharEqualIgnoreCase('x', 'x'));
        Assert.True(CharUtils.IsCharEqualIgnoreCase('x', 'X'));
    }

    [Test]
    public void IsCharEqualIgnoreCase_Unicode()
    {
        Assert.True(CharUtils.IsCharEqualIgnoreCase('Ж', 'Ж'));
        Assert.True(CharUtils.IsCharEqualIgnoreCase('ж', 'ж'));
        Assert.True(CharUtils.IsCharEqualIgnoreCase('ж', 'Ж'));
    }

    [Test]
    public void IsCharEqualIgnoreCase_Mix()
    {
        Assert.False(CharUtils.IsCharEqualIgnoreCase('Ж', 'X'));
        Assert.False(CharUtils.IsCharEqualIgnoreCase('ж', 'x'));
        Assert.False(CharUtils.IsCharEqualIgnoreCase('ж', 'x'));
    }

    [Test]
    public void IsCharEqualIgnoreCase_NonAlphabet()
    {
        Assert.False(CharUtils.IsCharEqualIgnoreCase(' ', '@'));
    }
}
