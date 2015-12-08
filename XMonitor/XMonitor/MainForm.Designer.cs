using System;
namespace XMonitor
{
    partial class MainForm
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
            foreach (var dev in winPcapDeviceList)
            {
                try
                {

                    dev.StopCapture();
                    dev.Close();
                }
                catch (Exception e)
                {

                    //pass
                }
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
            this.tvProcess = new System.Windows.Forms.TreeView();
            this.lvStatistic = new System.Windows.Forms.ListView();
            this.device = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.package = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pps = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.data = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.bps = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // tvProcess
            // 
            this.tvProcess.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.tvProcess.Location = new System.Drawing.Point(12, 12);
            this.tvProcess.Name = "tvProcess";
            this.tvProcess.Size = new System.Drawing.Size(519, 473);
            this.tvProcess.TabIndex = 0;
            this.tvProcess.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvProcess_BeforeCollapse);
            this.tvProcess.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvProcess_NodeMouseDoubleClick);
            // 
            // lvStatistic
            // 
            this.lvStatistic.AccessibleName = "";
            this.lvStatistic.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.device,
            this.package,
            this.pps,
            this.data,
            this.bps});
            this.lvStatistic.FullRowSelect = true;
            this.lvStatistic.Location = new System.Drawing.Point(537, 12);
            this.lvStatistic.Name = "lvStatistic";
            this.lvStatistic.Size = new System.Drawing.Size(617, 473);
            this.lvStatistic.TabIndex = 1;
            this.lvStatistic.UseCompatibleStateImageBehavior = false;
            this.lvStatistic.View = System.Windows.Forms.View.Details;
            // 
            // device
            // 
            this.device.Text = "device";
            this.device.Width = 77;
            // 
            // package
            // 
            this.package.Text = "package";
            this.package.Width = 71;
            // 
            // pps
            // 
            this.pps.Text = "pps";
            this.pps.Width = 98;
            // 
            // data
            // 
            this.data.Text = "data (Byte)";
            this.data.Width = 78;
            // 
            // bps
            // 
            this.bps.Text = "bps";
            this.bps.Width = 91;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1166, 497);
            this.Controls.Add(this.lvStatistic);
            this.Controls.Add(this.tvProcess);
            this.Name = "MainForm";
            this.Text = "XMonitor-SEU";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView tvProcess;
        private System.Windows.Forms.ListView lvStatistic;
        private System.Windows.Forms.ColumnHeader device;
        private System.Windows.Forms.ColumnHeader package;
        private System.Windows.Forms.ColumnHeader pps;
        private System.Windows.Forms.ColumnHeader data;
        private System.Windows.Forms.ColumnHeader bps;
    }
}

