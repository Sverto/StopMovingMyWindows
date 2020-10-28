using StopMovingMyWindows.Power;
using StopMovingMyWindows.Windows;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StopMovingMyWindows
{
    public partial class MainForm : Form
    {
        private IEnumerable<Window> _Windows;
        private DisplayPowerStateEventHandler _Dpseh;


        public MainForm()
        {
            InitializeComponent();
            // Hook into Display Power State changes
            _Dpseh = new DisplayPowerStateEventHandler(this.Handle);
            _Dpseh.OnDisplayOff += OnDisplayOff;
            _Dpseh.OnDisplayOn += OnDisplayOn;
        }


        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            // Catch Display Power State changes 
            _Dpseh?.WndProcHook(ref m);
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Dispose Display Power State object
            _Dpseh?.Dispose();
            _Dpseh = null;
        }


        protected override void SetVisibleCore(bool value)
        {
            // Hide Form, use tray
            base.SetVisibleCore(false);
        }


        private void MenuItemExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        #region Display PowerState Event Handling
        private void OnDisplayOff(object sender, EventArgs e)
        {
            // Store last window positions on display power off
            _Windows = WindowHelper.GetWindows();
            Debug.Print($"Stored window positions");
        }

        private void OnDisplayOn(object sender, EventArgs e)
        {
            if (_Windows == null) return;

            // Restore window positions on display power on
            foreach (var window in _Windows)
            {
                if (WindowHelper.GetPositionAndSize(window.Handle).Location != window.Position)
                {
                    Debug.Print($"Restoring window position '{window.Name}'");
                    WindowHelper.SetPosition(window.Handle, window.Position);
                }
            }
            Debug.Print($"Restored window positions");
        }
        #endregion


#if DEBUG
        // Test
        private async void MenuItemSimulatePowerOffOn_Click(object sender, EventArgs e)
        {
            MenuItemSimulatePowerOffOn.Enabled = false;
            OnDisplayOff(this, EventArgs.Empty);
            await Task.Delay(1000);
            OnDisplayOn(this, EventArgs.Empty);
            MenuItemSimulatePowerOffOn.Enabled = true;
        }
#endif

    }
}
