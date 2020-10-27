namespace StopMovingMyWindows
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.Tray = new System.Windows.Forms.NotifyIcon(this.components);
            this.TrayContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.MenuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.TrayContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // Tray
            // 
            this.Tray.ContextMenuStrip = this.TrayContextMenuStrip;
            this.Tray.Icon = ((System.Drawing.Icon)(resources.GetObject("Tray.Icon")));
            this.Tray.Text = "Stop Moving My Windows";
            this.Tray.Visible = true;
            // 
            // TrayContextMenuStrip
            // 
            this.TrayContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MenuItemExit});
            this.TrayContextMenuStrip.Name = "TrayContextMenuStrip";
            this.TrayContextMenuStrip.Size = new System.Drawing.Size(94, 26);
            // 
            // MenuItemExit
            // 
            this.MenuItemExit.Name = "MenuItemExit";
            this.MenuItemExit.Size = new System.Drawing.Size(93, 22);
            this.MenuItemExit.Text = "Exit";
            this.MenuItemExit.Click += new System.EventHandler(this.MenuItemExit_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(283, 150);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.TrayContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon Tray;
        private System.Windows.Forms.ContextMenuStrip TrayContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem MenuItemExit;
    }
}

