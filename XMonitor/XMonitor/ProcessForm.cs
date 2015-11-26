using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using SharpPcap;
using PacketDotNet;

namespace XMonitor
{

    public partial class ProcessForm : Form
    {
        public int pid;
        public Process process;
        private const int readTimeoutMilliseconds = 1000;
        private CaptureDeviceList devices = CaptureDeviceList.Instance;
        private List<RawCapture> rawCaptures;
        private PacketStatistic statistic;

        public ProcessForm(int pid, PacketStatistic statistic)
        {
            this.pid = pid;
            this.process = Process.GetProcessById(pid);
            this.statistic = statistic;
            InitializeComponent();
            Tag = new Size(Size.Width, Size.Height);

            foreach (Control ctrl in this.Controls)
            {

                ctrl.Tag = new Tuple<Tuple<double, double>, Size>(
                    new Tuple<double, double>(ctrl.Location.X / (double)Size.Width, ctrl.Location.Y / (double)Size.Height),
                    ctrl.Size);
            }
        }



        private void ProcessForm_Load(object sender, EventArgs e)
        {
            Text = string.Format("Process: {0} ({1})", process.ProcessName, process.Id);
            //TODO ohter info

            var timer = new System.Timers.Timer(1233);
            timer.Elapsed += updateView;

            timer.AutoReset = true;
            timer.Enabled = true;
        }

        private void updateView(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (IsDisposed)
                return;
            rawCaptures = new List<RawCapture>();
            var connectsions = new ProcessConnection().getConnectionByPID(pid);
            var packets = new List<RawCapture>();
            foreach (var con in connectsions)
            {
                if (statistic.packets.ContainsKey(con))
                    packets.AddRange(statistic.packets[con]);
            }
            var newData = new List<List<string>>();

            foreach (var rawPacket in packets.OrderBy((p) => p.Timeval))
            {
                var packet = Packet.ParsePacket(rawPacket.LinkLayerType, rawPacket.Data);

                var ipV4Packet = (IPv4Packet)packet.Extract(typeof(IPv4Packet));
                if (ipV4Packet != null)
                {
                    var data = new List<string>();
                    var time = rawPacket.Timeval;
                    var tcpPacket = (TcpPacket)packet.Extract(typeof(TcpPacket));
                    var udpPacket = (UdpPacket)packet.Extract(typeof(UdpPacket));
                    if (tcpPacket != null)
                    {
                        rawCaptures.Add(rawPacket);
                        data.Add(string.Format("{0}.{1}", time.Seconds, time.MicroSeconds));
                        data.Add(rawPacket.Data.Length.ToString());
                        data.Add("TCP");
                        data.Add(ipV4Packet.SourceAddress.ToString());
                        data.Add(tcpPacket.SourcePort.ToString());
                        data.Add(ipV4Packet.DestinationAddress.ToString());
                        data.Add(tcpPacket.DestinationPort.ToString());


                    }
                    else if (udpPacket != null)
                    {

                        rawCaptures.Add(rawPacket);
                        data.Add(string.Format("{0}.{1}", time.Seconds, time.MicroSeconds));
                        data.Add(rawPacket.Data.Length.ToString());
                        data.Add("UDP");
                        data.Add(ipV4Packet.SourceAddress.ToString());
                        data.Add(udpPacket.SourcePort.ToString());
                        data.Add(ipV4Packet.DestinationAddress.ToString());
                        data.Add(udpPacket.DestinationPort.ToString());


                    }

                    newData.Add(data);
                }
                
            }

            this.Invoke(new Action(
                ()=>{
                    listView1.BeginUpdate();
                    listView1.Items.Clear();
                    foreach (var data in newData)
                    {
                        listView1.Items.Add(new ListViewItem(data.ToArray()));
                    }
                    listView1.Columns[0].Width = -1;
                    listView1.Columns[3].Width = -1;
                    listView1.Columns[5].Width = -1;
                    listView1.EndUpdate();
                    listView1.Items[listView1.Items.Count - 1].EnsureVisible(); //scroll to end;

                    label1.Text = string.Format("{0} packets", listView1.Items.Count);

                }
            ));



        }


        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0 )
            {
                var id = listView1.SelectedIndices[0];
                if (id >= rawCaptures.Count)
                    return;
                var capture = rawCaptures[id];
                var packetForm = new PacketForm(capture);
                packetForm.Show();
            }
        }

        private void ProcessForm_SizeChanged(object sender, EventArgs e)
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
