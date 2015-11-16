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
            treeView1.Nodes.Clear();
            foreach (Proc proc in tree)
            {
                treeView1.Nodes.Add(buildTreeNode(proc));
            }
            treeView1.ExpandAll();
        }

        private TreeNode buildTreeNode(Proc proc)
        {
            var res = new TreeNode(string.Format("{0} ({1})",proc.processName,proc.processId));
           
            foreach(Proc child in proc.children)
            {
                res.Nodes.Add(buildTreeNode(child));
            }
            return res;
        }

    }
}
