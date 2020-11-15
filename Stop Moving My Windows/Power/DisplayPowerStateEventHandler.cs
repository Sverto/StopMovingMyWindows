using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
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
        /// <summary>Triggered when display is powered off</summary>
        public event DisplayOffEventHandler OnDisplayOff;
        public delegate void DisplayOffEventHandler(object sender, EventArgs e);

        /// <summary>Triggered when display is powered on</summary>
        public event DisplayOnEventHandler OnDisplayOn;
        public delegate void DisplayOnEventHandler(object sender, EventArgs e);

        /// <summary>Triggered when display is dimmed</summary>
        public event DisplayDimmedEventHandler OnDisplayDimmed;
        public delegate void DisplayDimmedEventHandler(object sender, EventArgs e);
        #endregion


        protected bool _Disposed = false;
        private bool skippedFirstEvent = false;
        private readonly IntPtr _PowerSettingNotification;


        /// <summary>
        /// Register for DisplayPowerState system notifications.
        /// A call to <see cref="WndProcHook"/> is required in the <see cref="Form.WndProc"/> method for the <see cref="OnDisplayOff"/>, <see cref="OnDisplayOn"/> and <see cref="OnDisplayDimmed"/> events to work.
        /// </summary>
        /// <param name="handle">The handle of the current process (example: myForm.Handle)</param>
        public DisplayPowerStateEventHandler(IntPtr handle)
        {
            // Register for system DisplayPowerState notifications
            _PowerSettingNotification = RegisterPowerSettingNotification(handle, ref DisplayPowerStateNatives.GUID_CONSOLE_DISPLAY_STATE, DisplayPowerStateNatives.DEVICE_NOTIFY_WINDOW_HANDLE);
        }


        /// <summary>
        /// Override the <see cref="Form.WndProc"/> method and add a call to this method for the <see cref="OnDisplayOff"/>, <see cref="OnDisplayOn"/> and <see cref="OnDisplayDimmed"/> events to work.
        /// </summary>
        public void WndProcHook(ref Message m)
        {
            if (_Disposed) return;

            // Catch PBT_POWERSETTINGCHANGE event
            switch (m.Msg)
            {
                case DisplayPowerStateNatives.WM_POWERBROADCAST:
                    if (m.WParam.ToInt32() == DisplayPowerStateNatives.PBT_POWERSETTINGCHANGE)
                    {
                        var s = (DisplayPowerStateNatives.POWERBROADCAST_SETTING)Marshal.PtrToStructure(m.LParam, typeof(DisplayPowerStateNatives.POWERBROADCAST_SETTING));
                        if (s.PowerSetting == DisplayPowerStateNatives.GUID_CONSOLE_DISPLAY_STATE)
                        {
                            // Skip first event which is triggered on start but useless
                            if (skippedFirstEvent == false)
                            {
                                skippedFirstEvent = true;
                                return;
                            }

                            Debug.Print($"Display powerstate change detected: {(DisplayPowerState)s.Data}");
                            // Trigger event based on DisplayPowerState
                            switch ((DisplayPowerState)s.Data)
                            {
                                case DisplayPowerState.Off:
                                    OnDisplayOff?.Invoke(this, EventArgs.Empty);
                                    break;
                                case DisplayPowerState.On:
                                    OnDisplayOn?.Invoke(this, EventArgs.Empty);
                                    break;
                                case DisplayPowerState.Dimmed:
                                    OnDisplayDimmed?.Invoke(this, EventArgs.Empty);
                                    break;
                            }
                        }
                    }
                    break;
            }
        }


        /// <summary>
        /// Unregister for system DisplayPowerState notifications and stop processing WndProc Messages
        /// </summary>
        public void Dispose()
        {
            if (!_Disposed)
            {
                _Disposed = true;
                // Unregister for system DisplayPowerState notifications
                UnregisterPowerSettingNotification(_PowerSettingNotification);
            }
        }

    }
}
