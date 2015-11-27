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
        private WinPcapDeviceList winPcapDeviceList = WinPcapDeviceList.Instance;

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

            
            
            Tag = new Size(Size.Width, Size.Height);
            
            foreach (Control ctrl in this.Controls)
            {
                
                ctrl.Tag = new Tuple<Tuple<double, double>,Size>(
                    new Tuple<double, double>(ctrl.Location.X / (double)Size.Width, ctrl.Location.Y / (double)Size.Height),
                    ctrl.Size);
            }
            
        }

        private void updateView(object sender, ElapsedEventArgs e)
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
                       

                        lvStatistic.BeginUpdate();
                        var items = lvStatistic.Items;
                        items.Clear();
                        foreach (var v in statistic.devs)
                        {
                            var dev = v.Key;
                            var data = v.Value;
                            var friendlyName = ((WinPcapDevice)dev).Interface.FriendlyName;
                            items.Add(new ListViewItem(
                                    new[] { 
                                    friendlyName,
                                    data.packetReceivedNum.ToString(),
                                    data.pps.ToString(),
                                    data.packetReceiveSize.ToString(),
                                    data.bps.ToString()
                                    }));
                        }

                        items.Add(new ListViewItem(
                                    new[] { 
                                    "Total",
                                    statistic.packetReceivedNum.ToString(),
                                    statistic.pps.ToString(),
                                    statistic.packetReceiveSize.ToString(),
                                    statistic.bps.ToString()
                                    }));
                        lvStatistic.Columns[0].Width = -1;

                        lvStatistic.EndUpdate();
                    }
                ));
        }


        private TreeNode buildTreeNode(Proc proc)
        {
            string info;
            if (proc.connections.Count == 0)
            {
                info = string.Format("{0} (pid:{1})", proc.processName, proc.processId); 
            }
            else
            {
                info = string.Format("{0} (pid :{1} - {2} connections )", proc.processName, proc.processId, proc.connections.Count);
            }
            var res = new TreeNode(info);
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
            var pf = new ProcessForm(pid, statistic);
            pf.TopLevel = true;
            pf.Show();

            
            
        }

        private void device_OnPcapStatistics(object sender, StatisticsModeEventArgs e)
        {
            statistic.update(e);
        }

        
        private void device_OnPacketArrival(object sender, CaptureEventArgs e)
        {
            statistic.update(e);

        }

        private void tvProcess_BeforeCollapse(object sender, TreeViewCancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var timer = new System.Timers.Timer(1000);
            timer.Elapsed += updateView;
            
            timer.AutoReset = true;
            timer.Enabled = true;

            
            foreach (var dev in winPcapDeviceList)
            {
               
                dev.OnPacketArrival += new PacketArrivalEventHandler(device_OnPacketArrival);
                dev.Open();
                dev.StartCapture();
            }
        }


        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            var oldSzie = (Size)Tag;          
            foreach (Control ctrl in this.Controls)
            {
                var tag = (Tuple<Tuple<double, double>, Size>)ctrl.Tag;
                var pos = tag.Item1;
                var size = tag.Item2;

                ctrl.Left = (int)(Size.Width * pos.Item1);
                ctrl.Top = (int)(Size.Height * pos.Item2);

                ctrl.Width = (int)(Size.Width / (float)oldSzie.Width * size.Width);
                ctrl.Height = (int)(Size.Height / (float)oldSzie.Height * size.Height);
            }

            
            
        }

    }
}
 