using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Win32Interop.WinHandles;
using Win32Interop.WinHandles.Internal;

namespace HideTaskbar.bean
{
    public class WindowStatus
    {
        public static readonly int GWL_EXSTYLE = -20; // GWL_EXSTYLE 得到扩展的窗口风格

        public string windowName { set; get; }
        public string status { set; get; }
        public bool visible { set; get; }
        public string processName { set; get; }
        public string appPath { set; get; }
        public IntPtr rawPtr { set; get; }
        public WindowHandle windowHandle { get; }
        public ImageSource icon { set; get; }

        public WindowStatus(WindowHandle windowHandle)
        {
            IntPtr rawPtr = windowHandle.RawPtr; // 窗口句柄
            long dwExStyle = NativeMethods.GetWindowLong(rawPtr, GWL_EXSTYLE); // 窗口风格
            int calcID; // 进程ID
            NativeMethods.GetWindowThreadProcessId(rawPtr, out calcID);
            Process myProcess = Process.GetProcessById(calcID);
            try
            {
                this.appPath = myProcess.MainModule.FileName;
                this.icon = new FileToImageIconConverter(appPath).Icon;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            this.windowHandle = windowHandle;
            this.windowName = windowHandle.GetWindowText();
            this.status = "0x" + dwExStyle.ToString("x") + "L";
            this.visible = windowHandle.IsVisible();
            this.processName = myProcess.ProcessName;
            this.rawPtr = rawPtr;
        }
    }
}
