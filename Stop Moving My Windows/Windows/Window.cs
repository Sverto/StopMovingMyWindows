using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

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
