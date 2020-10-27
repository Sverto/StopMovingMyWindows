using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StopMovingMyWindows.Power
{
    public class DisplayPowerStateEventHandler : IDisposable
    {
        #region System Library Imports
        [DllImport("user32.dll", SetLastError = true, EntryPoint = "RegisterPowerSettingNotification", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr RegisterPowerSettingNotification(IntPtr hRecipient, ref Guid PowerSettingGuid, Int32 Flags);

        [DllImport("user32.dll", SetLastError = true, EntryPoint = "UnregisterPowerSettingNotification", CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnregisterPowerSettingNotification(IntPtr handle);
        #endregion


        #region Events
        public delegate void DisplayOffEventHandler(object sender, EventArgs e);
        public event DisplayOffEventHandler OnDisplayOff;

        public delegate void DisplayOnEventHandler(object sender, EventArgs e);
        public event DisplayOnEventHandler OnDisplayOn;

        public delegate void DisplayDimmedEventHandler(object sender, EventArgs e);
        public event DisplayDimmedEventHandler OnDisplayDimmed;
        #endregion


        protected bool _Disposed = false;
        private readonly IntPtr _Handle;
        private readonly IntPtr _PowerSettingNotification;


        public DisplayPowerStateEventHandler(IntPtr handle)
        {
            _Handle = handle;
            _PowerSettingNotification = RegisterPowerSettingNotification(handle, ref DisplayPowerStateNatives.GUID_CONSOLE_DISPLAY_STATE, DisplayPowerStateNatives.DEVICE_NOTIFY_WINDOW_HANDLE);
        }


        /// <summary>
        /// Override your Form WndProc method and add a call to this method.
        /// This is required if you want to detect display powerstate changes.
        /// </summary>
        public void WndProcHook(ref Message m)
        {
            // Hook into display powerstate event
            switch (m.Msg)
            {
                case DisplayPowerStateNatives.WM_POWERBROADCAST:
                    if (m.WParam.ToInt32() == DisplayPowerStateNatives.PBT_POWERSETTINGCHANGE)
                    {
                        var s = (DisplayPowerStateNatives.POWERBROADCAST_SETTING)Marshal.PtrToStructure(m.LParam, typeof(DisplayPowerStateNatives.POWERBROADCAST_SETTING));
                        if (s.PowerSetting == DisplayPowerStateNatives.GUID_CONSOLE_DISPLAY_STATE)
                        {
                            Debug.Print($"Display powerstate change detected: {(DisplayPowerState)s.Data}");
                            switch ((DisplayPowerState)s.Data)
                            {
                                case DisplayPowerState.Off:
                                    OnDisplayOff(this, EventArgs.Empty);
                                    break;
                                case DisplayPowerState.On:
                                    OnDisplayOn(this, EventArgs.Empty);
                                    break;
                                case DisplayPowerState.Dimmed:
                                    OnDisplayDimmed(this, EventArgs.Empty);
                                    break;
                            }
                        }
                    }
                    break;
            }
        }


        public void Dispose()
        {
            if (!_Disposed)
            {
                _Disposed = true;
                UnregisterPowerSettingNotification(_PowerSettingNotification);
            }
        }

    }
}
