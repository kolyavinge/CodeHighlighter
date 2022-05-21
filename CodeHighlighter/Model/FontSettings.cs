using System.Windows;
using System.Windows.Media;

namespace CodeHighlighter.Model;

internal class FontSettings
{
    public double FontSize { get; set; }
    public FontFamily FontFamily { get; set; }
    public FontStyle FontStyle { get; set; }
    public FontWeight FontWeight { get; set; }
    public FontStretch FontStretch { get; set; }

    public FontSettings()
    {
        FontFamily = new FontFamily("Consolas");
        FontSize = 12;
    }
}
