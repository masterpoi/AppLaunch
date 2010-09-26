using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Media;
using System.Windows;
using System.Windows.Interop;

namespace MtAppLaunch
{
    public class GlassHelper
    {
        public static void ExtendGlassFrame(Window window, Thickness thikness)
        {
            try
            {
                int isGlassEnabled = 0;
                DwmIsCompositionEnabled(ref isGlassEnabled);
                if (Environment.OSVersion.Version.Major > 5 && isGlassEnabled > 0)
                {
                    // Get the window handle
                    WindowInteropHelper helper = new WindowInteropHelper(window);
                    HwndSource mainWindowSrc = (HwndSource)HwndSource.
                        FromHwnd(helper.Handle);
                    mainWindowSrc.CompositionTarget.BackgroundColor =
                        Colors.Transparent;

                    // Get the dpi of the screen
                    System.Drawing.Graphics desktop =
                       System.Drawing.Graphics.FromHwnd(mainWindowSrc.Handle);
                    float dpiX = desktop.DpiX / 96;
                    float dpiY = desktop.DpiY / 96;

                    // Set Margins
                    MARGINS margins = new MARGINS();
                    margins.cxLeftWidth = (int)(thikness.Left * dpiX);
                    margins.cxRightWidth = (int)(thikness.Right * dpiX);
                    margins.cyBottomHeight = (int)(thikness.Bottom * dpiY);
                    margins.cyTopHeight = (int)(thikness.Top * dpiY);

                    window.Background = Brushes.Transparent;

                    int hr = DwmExtendFrameIntoClientArea(mainWindowSrc.Handle,
                                ref margins);
                }
                else
                {
                    window.Background = SystemColors.WindowBrush;
                }
            }
            catch (DllNotFoundException)
            {

            }
        }

        [DllImport("dwmapi.dll")]
        static extern int
           DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMarInset);

        [DllImport("dwmapi.dll")]
        extern static int DwmIsCompositionEnabled(ref int en);


    }


    [StructLayout(LayoutKind.Sequential)]
    struct MARGINS
    {
        public int cxLeftWidth;
        public int cxRightWidth;
        public int cyTopHeight;
        public int cyBottomHeight;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SHFILEINFO
    {
        public IntPtr hIcon;
        public IntPtr iIcon;
        public uint dwAttributes;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string szDisplayName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
        public string szTypeName;
    };


    class Win32
    {
        public const uint SHGFI_ICON = 0x100;
        public const uint SHGFI_LARGEICON = 0x0;    // 'Large icon
        public const uint SHGFI_SMALLICON = 0x1;    // 'Small icon

        [DllImport("shell32.dll")]
        public static extern IntPtr SHGetFileInfo(string pszPath,
                                    uint dwFileAttributes,
                                    ref SHFILEINFO psfi,
                                    uint cbSizeFileInfo,
                                    uint uFlags);
    }

}
