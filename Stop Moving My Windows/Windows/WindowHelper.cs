using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace StopMovingMyWindows.Windows
{
    public static class WindowHelper
    {
        #region System Library Imports
        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc enumFunc, int lParam);
        private delegate bool EnumWindowsProc(IntPtr handle, int lParam); // callback method

        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(IntPtr handle);

        [DllImport("user32.dll")]
        private static extern int GetWindowTextLength(IntPtr handle);

        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr handle, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        private static extern bool GetWindowRect(IntPtr handle, ref Rectangle rectangle);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)] static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, WindowPositionFlags uFlags);

        [Flags]
        private enum WindowPositionFlags : uint
        {
            SynchronousWindowPosition = 0x4000,
            DeferErase = 0x2000,
            DrawFrame = 0x0020,
            FrameChanged = 0x0020,
            HideWindow = 0x0080,
            DoNotActivate = 0x0010,
            DoNotCopyBits = 0x0100,
            IgnoreMove = 0x0002,
            DoNotChangeOwnerZOrder = 0x0200,
            DoNotRedraw = 0x0008,
            DoNotReposition = 0x0200,
            DoNotSendChangingEvent = 0x0400,
            IgnoreResize = 0x0001,
            IgnoreZOrder = 0x0004,
            ShowWindow = 0x0040,
        }
        #endregion


        /// <summary>
        /// Set window position by handle
        /// </summary>
        public static void SetWindowPosition(IntPtr handle, Point position)
        {
            SetWindowPos(handle, IntPtr.Zero, position.X, position.Y, 0, 0, 
                         WindowPositionFlags.IgnoreResize | WindowPositionFlags.IgnoreZOrder | WindowPositionFlags.DoNotSendChangingEvent);
        }


        /// <summary>
        /// Get window title by handle
        /// </summary>
        /// <returns>Title or null when window has none</returns>
        public static string GetWindowTitle(IntPtr handle)
        {
            string title = null;
            int titleLength = GetWindowTextLength(handle);
            if (titleLength > 0)
            {
                StringBuilder titleBuilder = new StringBuilder(titleLength);
                GetWindowText(handle, titleBuilder, titleLength + 1);
                title = titleBuilder.ToString();
            }
            return title;
        }


        /// <summary>
        /// Get a list of visible windows
        /// </summary>
        public static IEnumerable<WindowPosition> GetWindowPositions()
        {
            var windows = new List<WindowPosition>();
            EnumWindows(delegate (IntPtr handle, int lParam) { return ValidateAndEnlistWindow(handle, windows); }, 0);
            return windows;
        }

        private static bool ValidateAndEnlistWindow(IntPtr handle, List<WindowPosition> windows)
        {
            // Ignore hidden Windows
            if (!IsWindowVisible(handle)) return true;

            // Get window title, ignore otherwise
            string windowTitle = GetWindowTitle(handle);
            if (windowTitle == null) return true;

            // Get window position and size, ignore if position is negative (minimized) or size <= 0
            var windowRect = new Rectangle();
            GetWindowRect(handle, ref windowRect);
            if (windowRect.X <= -32000 || windowRect.Y <= -32000 ||
                windowRect.Width <= 0 || windowRect.Height <= 0) return true;

            // Add to list
            windows.Add(new WindowPosition(handle, windowRect.Location, windowTitle));

            return true;
        }

    }
}
