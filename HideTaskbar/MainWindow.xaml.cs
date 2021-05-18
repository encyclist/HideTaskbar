using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Management;
using Win32Interop.WinHandles;
using Win32Interop.WinHandles.Internal;
using System.Drawing;

namespace HideTaskbar
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        private static int GWL_EXSTYLE = -20; // GWL_EXSTYLE 得到扩展的窗口风格
        private static long WS_EX_TOOLWINDOW = 0x00000080L; // 128
        private static int SW_SHOW = 5; // 激活窗口，并将其显示为当前大小和位置。
        private static int SW_HIDE = 0; // 隐藏窗口并激活另一个窗口。
        private static SortDescription lastSortDescription = new SortDescription("windowName", ListSortDirection.Descending);

        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            refresh();
        }

        // 刷新列表
        private void refresh()
        {
            listView.Items.Clear();

            var allWindows = TopLevelWindowUtils.FindWindows(w => 
                w.IsValid &&
                w.GetWindowText() != "" &&
                w.GetWindowText() != "Default IME" && 
                w.GetWindowText() != "MSCTFIME UI"
            );
            foreach (var windowHandle in allWindows)
            {
                try
                {
                    listView.Items.Add(new WindowStatus(windowHandle));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            listView.Items.SortDescriptions.Clear();
            listView.Items.SortDescriptions.Add(lastSortDescription);
        }

        private void MenuItem_Click_HideTaskbar(object sender, RoutedEventArgs e)
        {
            int index = listView.SelectedIndex;
            WindowStatus item = (WindowStatus)listView.Items[index];
            NativeMethods.SetWindowLongPtr(item.rawPtr, GWL_EXSTYLE, WS_EX_TOOLWINDOW);
            refresh();
        }

        private void MenuItem_Click_Show(object sender, RoutedEventArgs e)
        {
            int index = listView.SelectedIndex;
            WindowStatus item = (WindowStatus)listView.Items[index];
            NativeMethods.ShowWindow(item.rawPtr, SW_SHOW);
        }

        private void MenuItem_Click_Hide(object sender, RoutedEventArgs e)
        {
            int index = listView.SelectedIndex;
            WindowStatus item = (WindowStatus)listView.Items[index];
            NativeMethods.ShowWindow(item.rawPtr, SW_HIDE);
        }

        private void MenuItem_Click_Copy(object sender, RoutedEventArgs e)
        {
            int index = listView.SelectedIndex;
            WindowStatus item = (WindowStatus)listView.Items[index];
            string text = "进程名:" + item.processName + "；窗口名:" + item.windowName + "；显示状态;" + item.status;
            Clipboard.SetDataObject(text, true);
        }

        private void Button_Click_AboutStatus(object sender, RoutedEventArgs e)
        {
            Process.Start("https://docs.microsoft.com/en-us/windows/win32/winmsg/extended-window-styles");
        }

        private void Button_Click_Resresh(object sender, RoutedEventArgs e)
        {
            refresh();
        }

        private void GridViewColumnHeaderClickedHandler(object sender, RoutedEventArgs e)
        {
            var headerClicked = e.OriginalSource as GridViewColumnHeader;
            if (headerClicked.Content.Equals("进程"))
            {
                lastSortDescription = new SortDescription("processName", ListSortDirection.Ascending);
            }
            else if (headerClicked.Content.Equals("窗口名称"))
            {
                lastSortDescription = new SortDescription("windowName", ListSortDirection.Descending);
            }
            else if (headerClicked.Content.Equals("显示状态"))
            {
                lastSortDescription = new SortDescription("status", ListSortDirection.Descending);
            }
            else if (headerClicked.Content.Equals("是否可见"))
            {
                lastSortDescription = new SortDescription("visible", ListSortDirection.Descending);
            }
            listView.Items.SortDescriptions.Clear();
            listView.Items.SortDescriptions.Add(lastSortDescription);
        }

        class WindowStatus
        {
            public string windowName { set; get; }
            public string status { set; get; }
            public bool visible { set; get; }
            public string processName { set; get; }
            public IntPtr rawPtr { set; get; }
            public WindowHandle windowHandle { get; }
            public WindowStatus(WindowHandle windowHandle)
            {
                IntPtr rawPtr = windowHandle.RawPtr; // 窗口句柄
                long dwExStyle = NativeMethods.GetWindowLong(rawPtr, GWL_EXSTYLE); // 窗口风格
                int calcID; // 进程ID
                NativeMethods.GetWindowThreadProcessId(rawPtr, out calcID);
                Process myProcess = Process.GetProcessById(calcID);
                //var appPath = myProcess.MainModule.FileName;
                //Icon icon = System.Drawing.Icon.ExtractAssociatedIcon(appPath);

                this.windowHandle = windowHandle;
                this.windowName = windowHandle.GetWindowText();
                this.status = "0x"+dwExStyle.ToString("x")+"L";
                this.visible = windowHandle.IsVisible();
                this.processName = myProcess.ProcessName;
                this.rawPtr = rawPtr;
            }
        }
















        


        public void ListAllWindows()
        {
            var allWindows = TopLevelWindowUtils.FindWindows(w => w.GetWindowText() != "");
            Console.WriteLine("All the windows that are currently present:");
            foreach (var windowHandle in allWindows)
            {
                Console.WriteLine(windowHandle.GetWindowText());
            }
        }

        public void ShowForeground()
        {
            var windowHandle = TopLevelWindowUtils.GetForegroundWindow();
            Console.WriteLine(windowHandle.GetWindowText());
        }

        public void FindNotepad()
        {
            var window = TopLevelWindowUtils.FindWindow(w => w.GetWindowText().Contains("Notepad"));
        }
    }
}
