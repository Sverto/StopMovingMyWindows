using System;
using System.Drawing;

namespace StopMovingMyWindows.Windows
{
    public class Window
    {
        public IntPtr Handle { get; }
        public Point Position { get; }
        public string Name { get; }

        public Window(IntPtr handle, Point position, string name)
        {
            Handle = handle;
            Position = position;
            Name = name;
        }
    }
}
