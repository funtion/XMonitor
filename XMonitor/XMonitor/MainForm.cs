using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace XMonitor
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<Proc> tree = Proc.getProcessTree();
            tvProcess.Nodes.Clear();
            foreach (Proc proc in tree)
            {
                tvProcess.Nodes.Add(buildTreeNode(proc));
            }
            tvProcess.ExpandAll();
        }

        private TreeNode buildTreeNode(Proc proc)
        {
            var res = new TreeNode(string.Format("{0} ({1}) - {2}",proc.processName,proc.processId,proc.connections.Count));
            res.Name = string.Format("{0}", proc.processId);
            foreach(Proc child in proc.children)
            {
                res.Nodes.Add(buildTreeNode(child));
            }
            return res;
        }


        private void tvProcess_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            int pid = Int32.Parse(e.Node.Name);
            var pf = new ProcessForm(pid);
            pf.Show();
        }

    }
}
 