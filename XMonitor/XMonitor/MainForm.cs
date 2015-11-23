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
using System.Timers;
using SharpPcap;
using SharpPcap.WinPcap;
using PacketDotNet;


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
        private const int readTimeoutMilliseconds = 1000;

        private PacketStatistic statistic = new PacketStatistic();


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

        private void updateProcessTree(object sender, ElapsedEventArgs e)
        {
            if (IsDisposed)
                return;
            this.Invoke(new Action(
                    () =>
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
                ));
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

        private void device_OnPcapStatistics(object sender, StatisticsModeEventArgs e)
        {
            statistic.update(e);
            if (IsDisposed)
                return;
            this.Invoke(new Action(
                    () =>
                    {
                        lvStatistic.BeginUpdate();
                        var items = lvStatistic.Items;
                        items.Clear();
                        
                        items.Add(new ListViewItem(new [] {"packet received", statistic.packetReceivedNum.ToString()}));
                        items.Add(new ListViewItem(new [] { "pps", statistic.pps.ToString()}));
                        items.Add(new ListViewItem(new[] { "data received (byte)", statistic.packetReceiveSize.ToString() }));
                        items.Add(new ListViewItem(new[] { "bps", statistic.bps.ToString() }));
                        lvStatistic.EndUpdate();
                    }
                ));


        }

        private void device_OnPacketArrival(object sender, CaptureEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void tvProcess_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var timer = new System.Timers.Timer(1234);
            timer.Elapsed += updateProcessTree;
            timer.AutoReset = true;
            timer.Enabled = true;

            var list = WinPcapDeviceList.Instance;
            foreach (var dev in list)
            {
                dev.OnPcapStatistics += new StatisticsModeEventHandler(device_OnPcapStatistics);
                dev.Open();
                dev.Filter = "tcp or udp";
                dev.Mode = CaptureMode.Statistics;
                dev.StartCapture();
            }
        }

    }
}
 