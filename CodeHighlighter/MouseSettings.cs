using CodeHighlighter.Utils;

namespace CodeHighlighter;

internal class MouseSettings
{
    public int VerticalScrollLinesCount { get; }

    public MouseSettings()
    {
        WinApi.SystemParametersInfo(WinApi.SPI_GETWHEELSCROLLLINES, 0, out IntPtr ptr, 0);
        VerticalScrollLinesCount = ptr.ToInt32();
    }
}
