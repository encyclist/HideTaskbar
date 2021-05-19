using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Win32Interop.WinHandles.Internal
{
    internal delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

    /// <summary> Win32 methods. </summary>
    internal static class NativeMethods
    {
        public const bool EnumWindows_ContinueEnumerating = true;
        public const bool EnumWindows_StopEnumerating = false;

        /// <summary>  
        /// 获取所有窗口
        /// </summary>      
        [DllImport("user32.dll")]
        public static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);

        /// <summary>    
        /// </summary>    
        [DllImport("user32.dll")]
        internal static extern IntPtr FindWindow(string sClassName, string sAppName);

        /// <summary>    
        /// </summary>    
        [DllImport("user32.dll")]
        internal static extern bool IsWindowVisible(IntPtr hWnd);

        /// <summary>    
        /// 获取正在使用的窗口
        /// </summary>    
        [DllImport("user32.dll")]
        internal static extern IntPtr GetForegroundWindow();

        /// <summary>    
        /// 该函数将指定窗口的标题条文本（如果存在）
        /// </summary>    
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern int GetWindowText(IntPtr hWnd, StringBuilder strText, int maxCount);

        /// <summary>   
        /// </summary>     
        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        internal static extern int GetWindowTextLength(IntPtr hWnd);

        /// <summary>    
        /// 该函数获得指定窗口所属的类的类名。
        /// </summary>    
        [DllImport("user32.dll")]
        internal static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        /// <summary>    
        /// 该函数设置指定窗口的显示状态。
        /// ShowWindow函数：https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-showwindow
        /// </summary>    
        [DllImport("user32.dll")]
        internal static extern int ShowWindow(IntPtr hWnd, int State);

        /// <summary>    
        /// 附加显示状态
        /// </summary>    
        [DllImport("user32.dll")]
        internal static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        /// <summary>    
        /// 该函数改变指定窗口的属性 
        /// 扩展窗口样式（SetWindowLong）：https://docs.microsoft.com/en-us/windows/win32/winmsg/extended-window-styles
        /// </summary>    
        [DllImport("user32.dll")]
        internal static extern int SetWindowLong(IntPtr hWnd, int nIndex, long dwNewLong);

        /// <summary>    
        /// 该函数改变指定窗口的属性 
        /// 扩展窗口样式（SetWindowLongPrt）：https://docs.microsoft.com/en-us/windows/win32/winmsg/extended-window-styles
        /// </summary>    
        [DllImport("user32.dll")]
        internal static extern int SetWindowLongPtr(IntPtr hWnd, int nIndex, long dwNewLong);

        /// <summary>    
        /// 获取线程ID
        /// </summary>    
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        internal static extern int GetWindowThreadProcessId(IntPtr hwnd, out int ID);

        /// <summary>    
        /// 该函数将创建指定窗口的线程设置到前台，并且激活该窗口
        /// </summary>    
        [DllImport("user32.dll")]
        internal static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <summary>    
        /// 该函数改变指定窗口的位置和尺寸。对于顶层窗口，位置和尺寸是相对于屏幕的左上角的：对于子窗口，位置和尺寸是相对于父窗口客户区的左上角坐标的。 
        /// </summary>    
        [DllImport("user32.dll")]
        internal static extern bool MoveWindow(IntPtr hWnd, int x, int y, int width, int height, bool repaint);

        /// <summary>    
        /// 该函数获得指定窗口所属的类的类名。
        /// </summary>  
        [DllImport("user32.dll")]
        internal static extern int GetClassName(IntPtr hWnd, out StringBuilder ClassName, int nMaxCount);

        /// <summary>    
        /// 该函数改变指定窗口的标题栏的文本内容 
        /// </summary>    
        [DllImport("user32.dll")]
        internal static extern int SetWindowText(IntPtr hWnd, string text);

        /// <summary>    
        /// 抽取程序图标
        /// </summary>    
        [DllImport("shell32.dll")]
        internal static extern int ExtractIconEx(string lpszFile, int niconIndex, IntPtr[] phiconLarge, IntPtr[] phiconSmall, int nIcons);

        /// <summary>    
        /// 获取父窗口句柄
        /// </summary>  
        [DllImport("user32.dll")]
        internal static extern IntPtr GetParent(IntPtr hWnd);
    }
}