using CodeHighlighter.Utils;

namespace CodeHighlighter.Controllers;

internal class MouseSettings
{
    public int VerticalScrollLinesCount
    {
        get
        {
            WinApi.SystemParametersInfo(WinApi.SPI_GETWHEELSCROLLLINES, 0, out IntPtr ptr, 0);
            return ptr.ToInt32();
        }
    }
}
