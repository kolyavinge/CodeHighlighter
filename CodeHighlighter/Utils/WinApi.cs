using System.Runtime.InteropServices;

namespace CodeHighlighter.Utils;

internal static class WinApi
{
    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool SystemParametersInfo(uint uiAction, uint uiParam, out IntPtr pvParam, uint fWinIni);

    public const uint SPI_GETWHEELSCROLLLINES = 0x0068;
}
