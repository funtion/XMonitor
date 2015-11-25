namespace XMonitor
{
    partial class ProcessForm
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
            this.listView1 = new System.Windows.Forms.ListView();
            this.time = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.len = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.type = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.srcIP = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.srcPort = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.dstIp = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.dstPort = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.time,
            this.len,
            this.type,
            this.srcIP,
            this.srcPort,
            this.dstIp,
            this.dstPort});
            this.listView1.Location = new System.Drawing.Point(12, 12);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(1042, 427);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
            // 
            // time
            // 
            this.time.Text = "time";
            this.time.Width = 93;
            // 
            // len
            // 
            this.len.Text = "len";
            // 
            // type
            // 
            this.type.Text = "type";
            this.type.Width = 109;
            // 
            // srcIP
            // 
            this.srcIP.Text = "srcIP";
            // 
            // srcPort
            // 
            this.srcPort.Text = "srcPort";
            this.srcPort.Width = 85;
            // 
            // dstIp
            // 
            this.dstIp.Text = "dstIP";
            this.dstIp.Width = 96;
            // 
            // dstPort
            // 
            this.dstPort.Text = "dstPort";
            this.dstPort.Width = 118;
            // 
            // ProcessForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1066, 451);
            this.Controls.Add(this.listView1);
            this.Name = "ProcessForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ProcessForm_FormClosed);
            this.Load += new System.EventHandler(this.ProcessForm_Load);
            this.SizeChanged += new System.EventHandler(this.ProcessForm_SizeChanged);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader time;
        private System.Windows.Forms.ColumnHeader len;
        private System.Windows.Forms.ColumnHeader type;
        private System.Windows.Forms.ColumnHeader srcIP;
        private System.Windows.Forms.ColumnHeader srcPort;
        private System.Windows.Forms.ColumnHeader dstIp;
        private System.Windows.Forms.ColumnHeader dstPort;
    }
}