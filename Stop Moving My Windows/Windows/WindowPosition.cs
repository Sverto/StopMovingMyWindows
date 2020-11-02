using System;
using System.Drawing;

namespace StopMovingMyWindows.Windows
{
    public class WindowPosition
    {
        public IntPtr Handle { get; }
        public Point Position { get; }
        public string Name { get; }

        public WindowPosition(IntPtr handle, Point position, string name)
        {
            Handle = handle;
            Position = position;
            Name = name;
        }


        /// <summary>
        /// Restore window position
        /// </summary>
        public void RestoreWindowPosition()
        {
            WindowHelper.SetWindowPosition(Handle, Position);
        }


        #region Value Equality
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            var wp = obj as WindowPosition;
            return wp.Handle == Handle && wp.Position == Position;
        }

        public override int GetHashCode()
        {
            return (Handle, Position).GetHashCode();
        }
        #endregion


        #region Object Equality
        public static bool operator ==(WindowPosition a, WindowPosition b)
        {
            return (a.Handle == b.Handle);
        }

        public static bool operator !=(WindowPosition a, WindowPosition b)
        {
            return (a.Handle != b.Handle);
        }
        #endregion

    }
}
