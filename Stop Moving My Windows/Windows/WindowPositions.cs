using System.Collections.Generic;
using System.Linq;

namespace StopMovingMyWindows.Windows
{
    public class WindowPositions
    {
        protected IEnumerable<WindowPosition> _WindowPositionList;


        public WindowPositions()
        {
            Store();
        }


        /// <summary>
        /// Store current window positions
        /// </summary>
        public void Store()
        {
            _WindowPositionList = WindowHelper.GetWindowPositions();
        }


        /// <summary>
        /// Restore window positions to their last stored state
        /// </summary>
        /// <returns>List of those that had to be restored</returns>
        public IEnumerable<WindowPosition> Restore()
        {
            var restoredWindows = new List<WindowPosition>();
            foreach(var window in _WindowPositionList.Except(WindowHelper.GetWindowPositions()))
            {
                window.RestoreWindowPosition();
                restoredWindows.Add(window);
            }
            return restoredWindows;
        }


        /// <summary>
        /// Returns true if any of the windows listed has changed position
        /// </summary>
        public bool HasChanged()
        {
            return _WindowPositionList.Except(WindowHelper.GetWindowPositions()).Any();
        }

    }
}
