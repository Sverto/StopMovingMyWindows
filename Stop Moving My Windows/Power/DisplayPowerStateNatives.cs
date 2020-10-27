using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace StopMovingMyWindows.Power
{
    public static class DisplayPowerStateNatives
    {
        public static Guid GUID_CONSOLE_DISPLAY_STATE = new Guid(0x6fe69556, 0x704a, 0x47a0, 0x8f, 0x24, 0xc2, 0x8d, 0x93, 0x6f, 0xda, 0x47);
        public const int DEVICE_NOTIFY_WINDOW_HANDLE = 0x00000000;
        public const int WM_POWERBROADCAST = 0x0218;
        public const int PBT_POWERSETTINGCHANGE = 0x8013;

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct POWERBROADCAST_SETTING
        {
            public Guid PowerSetting;
            public uint DataLength;
            public byte Data;
        }
    }
}
