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
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // tvProcess
            // 
            this.tvProcess.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.tvProcess.Dock = System.Windows.Forms.DockStyle.Left;
            this.tvProcess.Location = new System.Drawing.Point(0, 0);
            this.tvProcess.Name = "tvProcess";
            this.tvProcess.Size = new System.Drawing.Size(717, 559);
            this.tvProcess.TabIndex = 0;
            this.tvProcess.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvProcess_BeforeCollapse);
            this.tvProcess.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvProcess_NodeMouseDoubleClick);
            // 
            // lvStatistic
            // 
            this.lvStatistic.AccessibleName = "";
            this.lvStatistic.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lvStatistic.Dock = System.Windows.Forms.DockStyle.Right;
            this.lvStatistic.FullRowSelect = true;
            this.lvStatistic.Location = new System.Drawing.Point(735, 0);
            this.lvStatistic.Name = "lvStatistic";
            this.lvStatistic.Size = new System.Drawing.Size(441, 559);
            this.lvStatistic.TabIndex = 1;
            this.lvStatistic.UseCompatibleStateImageBehavior = false;
            this.lvStatistic.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Width = 203;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Width = 277;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1176, 559);
            this.Controls.Add(this.lvStatistic);
            this.Controls.Add(this.tvProcess);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView tvProcess;
        private System.Windows.Forms.ListView lvStatistic;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
    }
}

