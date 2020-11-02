using StopMovingMyWindows.Power;
using StopMovingMyWindows.Windows;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StopMovingMyWindows
{
    public partial class MainForm : Form
    {
        private WindowPositions _WindowPositions;
        private DisplayPowerStateEventHandler _Dpseh;


        public MainForm()
        {
            InitializeComponent();
            // Hook into Display Power State changes
            _Dpseh = new DisplayPowerStateEventHandler(this.Handle);
            _Dpseh.OnDisplayOff += OnDisplayOff;
            _Dpseh.OnDisplayOn += OnDisplayOn;
            // Init WindowPositions
            _WindowPositions = new WindowPositions();
        }


        protected override void WndProc(ref Message m)
        {
            // Catch Display Power State changes 
            _Dpseh?.WndProcHook(ref m);
            base.WndProc(ref m);
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
        /// <summary>
        /// Triggered when any display is turned off
        /// </summary>
        private void OnDisplayOff(object sender, EventArgs e)
        {
            // Get window positions on display power off
            _WindowPositions.Store();
            Debug.Print($"Stored window positions");
        }

        /// <summary>
        /// Triggered when any display is turned on
        /// </summary>
        private async void OnDisplayOn(object sender, EventArgs e)
        {
            // Wait for 5 seconds or until window movement is detected before restoring window positions
            var interval = 100; // ms
            for (int i = 0; i < 5000; i += interval)
            {
                if (_WindowPositions.HasChanged()) break;
                await Task.Delay(interval);
            }
            await Task.Delay(100); // Leave some time inbetween detection and restore

            // Restore window positions
            var restoredWindows = _WindowPositions.Restore();
            Debug.Print("Restored window positions " + String.Join(", ", restoredWindows.Select(w => $"'{w.Handle} {w.Name}'")));
        }
        #endregion


        #region DEBUG
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
        #endregion

    }
}
