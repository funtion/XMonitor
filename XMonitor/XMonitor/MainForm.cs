using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace XMonitor
{
    public partial class MainForm : Form
    {
        [DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        public static extern int GetScrollPos(int hWnd, int nBar);

        [DllImport("user32.dll")]
        static extern int SetScrollPos(IntPtr hWnd, int nBar, int nPos, bool bRedraw);

        private const int SB_HORZ = 0x0;
        private const int SB_VERT = 0x1;

        private Point GetTreeViewScrollPos(TreeView treeView)
        {
            return new Point(
                GetScrollPos((int)treeView.Handle, SB_HORZ),
                GetScrollPos((int)treeView.Handle, SB_VERT)
                );
        }

        private void SetTreeViewScrollPos(TreeView treeView, Point scrollPosition)
        {
            SetScrollPos((IntPtr)treeView.Handle, SB_HORZ, scrollPosition.X, true);
            SetScrollPos((IntPtr)treeView.Handle, SB_VERT, scrollPosition.Y, true);
        }

        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<Proc> tree = Proc.getProcessTree();
            var pos = new Point(
                GetScrollPos((int)tvProcess.Handle, SB_HORZ),
                GetScrollPos((int)tvProcess.Handle, SB_VERT)
                );
            tvProcess.BeginUpdate();
            tvProcess.Nodes.Clear();
            foreach (Proc proc in tree)
            {
                tvProcess.Nodes.Add(buildTreeNode(proc));
            }
            tvProcess.ExpandAll();
            
            tvProcess.EndUpdate();
            
            SetScrollPos((IntPtr)tvProcess.Handle, SB_VERT, pos.Y, true);

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
            pf.TopLevel = true;
            pf.Show();

            
        }

        private void tvProcess_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            e.Cancel = true;
        }

    }
}
 