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
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tvProcess
            // 
            this.tvProcess.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.tvProcess.Location = new System.Drawing.Point(45, 32);
            this.tvProcess.Name = "tvProcess";
            this.tvProcess.Size = new System.Drawing.Size(624, 497);
            this.tvProcess.TabIndex = 0;
            this.tvProcess.BeforeCollapse += new System.Windows.Forms.TreeViewCancelEventHandler(this.tvProcess_BeforeCollapse);
            this.tvProcess.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvProcess_NodeMouseDoubleClick);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(721, 61);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(854, 559);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tvProcess);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView tvProcess;
        private System.Windows.Forms.Button button1;
    }
}

