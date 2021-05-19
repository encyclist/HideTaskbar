using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;

namespace HideTaskbar
{
    class WindowHelper
    {
        private const int MAX_PATH = 260;
        public const int PROCESS_ALL_ACCESS = 0x000F0000 | 0x00100000 | 0xFFF;
        [DllImport("coredll.dll")]
        public extern static int GetWindowThreadProcessId(IntPtr hWnd, ref int lpdwProcessId);
        [DllImport("coredll.dll")]
        public extern static IntPtr OpenProcess(int fdwAccess, int fInherit, int IDProcess);
        [DllImport("coredll.dll")]
        public extern static bool TerminateProcess(IntPtr hProcess, int uExitCode);
        [DllImport("coredll.dll")]
        public extern static bool CloseHandle(IntPtr hObject);
        [DllImport("Coredll.dll", EntryPoint = "GetModuleFileName")]
        private static extern uint GetModuleFileName(IntPtr hModule, [Out] StringBuilder lpszFileName, int nSize);
        [DllImport("coredll.dll", SetLastError = true)]
        private static extern IntPtr ExtractIconEx(string fileName, int index, ref IntPtr hIconLarge, ref IntPtr hIconSmall, uint nIcons);

        //通过句柄获取运行程序路径

        public static String GetAppRunPathFromHandle(IntPtr hwnd)
        {
            int pId = 0;
            IntPtr pHandle = IntPtr.Zero;
            GetWindowThreadProcessId(hwnd, ref pId);
            pHandle = OpenProcess(PROCESS_ALL_ACCESS, 0, pId);
            StringBuilder sb = new StringBuilder(MAX_PATH);
            GetModuleFileName(pHandle, sb, sb.Capacity);
            CloseHandle(pHandle);
            return sb.ToString();
        }

        //根据句柄获取运行程序小图标 //当然也可以获取大图标

        public static Icon GetSmallIconFromHandle(IntPtr hwnd)
        {
            IntPtr hLargeIcon = IntPtr.Zero;
            IntPtr hSmallIcon = IntPtr.Zero;
            String filePath = GetAppRunPathFromHandle(hwnd);

            ExtractIconEx(filePath, 0, ref hLargeIcon, ref hSmallIcon, 1);
            Icon icon = null;
            try
            {
                icon = (Icon)Icon.FromHandle(hSmallIcon);
            }
            catch (Exception e)
            {
                Console.Out.WriteLine(e.Message);
            }
            return icon;
        }
    }
}
