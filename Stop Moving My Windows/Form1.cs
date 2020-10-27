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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StopMovingMyWindows
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            _Dpseh = new DisplayPowerStateEventHandler(this.Handle);
            _Dpseh.OnDisplayOff += OnDisplayOff;
            _Dpseh.OnDisplayOn += OnDisplayOn;
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _Dpseh?.Dispose();
            _Dpseh = null;
        }


        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            _Dpseh?.WndProcHook(ref m); // Hook into display powerstate event
        }


        protected override void SetVisibleCore(bool value)
        {
            base.SetVisibleCore(false); // Hide Form, use tray
        }


        private void MenuItemExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        #region Display PowerState Event Handling
        private IEnumerable<Window> _Windows;
        private DisplayPowerStateEventHandler _Dpseh;

        private void OnDisplayOff(object sender, EventArgs e)
        {
            // Store last window positions on display poweroff
            _Windows = WindowHelper.GetWindows();
        }

        private void OnDisplayOn(object sender, EventArgs e)
        {
            if (_Windows == null) return;
            
            // Restore window positions on display poweron
            foreach (var window in _Windows)
            {
                if (WindowHelper.GetPosition(window.Handle) != window.Position)
                {
                    Debug.Print($"Restoring window position '{window.Name}'");
                    WindowHelper.SetPosition(window.Handle, window.Position);
                }
            }
        }
        #endregion

    }
}
