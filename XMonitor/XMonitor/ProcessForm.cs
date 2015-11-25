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
        private List<CaptureEventArgs> capturedEvens = new List<CaptureEventArgs>();
        public ProcessForm(int pid)
        {
            this.pid = pid;
            this.process = Process.GetProcessById(pid);
            InitializeComponent();
            Tag = new Size(Size.Width, Size.Height);

            foreach (Control ctrl in this.Controls)
            {

                ctrl.Tag = new Tuple<Tuple<double, double>, Size>(
                    new Tuple<double, double>(ctrl.Location.X / (double)Size.Width, ctrl.Location.Y / (double)Size.Height),
                    ctrl.Size);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void ProcessForm_Load(object sender, EventArgs e)
        {
            Text = string.Format("Process: {0} ({1})", process.ProcessName, process.Id);


            var connectsions = new ProcessConnection().getConnectionByPID(pid);
            var filter = "";
            foreach (var con in connectsions)
            {

                filter += string.Format("( {0} and src host {1} and src port {2} and dst host {3} and dst port {4})", con.type.ToLower(), con.inIp, con.inPort, con.outIp, con.outPort);
                if (!con.Equals(connectsions.Last()))
                {
                    filter += " or ";
                }
            }

            foreach (ICaptureDevice dev in devices)
            {
                dev.OnPacketArrival += new SharpPcap.PacketArrivalEventHandler(device_OnPacketArrival);

                dev.Open(DeviceMode.Promiscuous, readTimeoutMilliseconds);
                dev.Filter = filter;
                dev.StartCapture();
            }


        }
        private delegate void handlePackageDelegate(CaptureEventArgs packet);
        private void device_OnPacketArrival(object sender, CaptureEventArgs packet)
        {
            if (IsDisposed)
                return;
            this.Invoke(new handlePackageDelegate(handlePackage), packet);

        }

        private void handlePackage(CaptureEventArgs captureEventArgs)
        {
            
            var packet = Packet.ParsePacket(captureEventArgs.Packet.LinkLayerType, captureEventArgs.Packet.Data);

            var ipV4Packet = (IPv4Packet)packet.Extract(typeof(IPv4Packet));
            if (ipV4Packet != null)
            {
                var data = new List<string>();
                var time = captureEventArgs.Packet.Timeval;
                var tcpPacket = (TcpPacket)packet.Extract(typeof(TcpPacket));
                if (tcpPacket != null)
                {
                    capturedEvens.Add(captureEventArgs);
                    data.Add(string.Format("{0}.{1}", time.Seconds, time.MicroSeconds));
                    data.Add(captureEventArgs.Packet.Data.Length.ToString());
                    data.Add("TCP");
                    data.Add(ipV4Packet.SourceAddress.ToString());
                    data.Add(tcpPacket.SourcePort.ToString());
                    data.Add(ipV4Packet.DestinationAddress.ToString());
                    data.Add(tcpPacket.DestinationPort.ToString());
                    listView1.Items.Add(new ListViewItem(data.ToArray()));
                    listView1.Items[listView1.Items.Count - 1].EnsureVisible(); //scroll to end;

                }
                else
                {
                    var udpPacket = (UdpPacket)packet.Extract(typeof(UdpPacket));
                    if (udpPacket != null)
                    {
                        capturedEvens.Add(captureEventArgs);
                        data.Add(string.Format("{0}.{1}", time.Seconds, time.MicroSeconds));
                        data.Add(captureEventArgs.Packet.Data.Length.ToString());
                        data.Add("UDP");
                        data.Add(ipV4Packet.SourceAddress.ToString());
                        data.Add(udpPacket.SourcePort.ToString());
                        data.Add(ipV4Packet.DestinationAddress.ToString());
                        data.Add(udpPacket.DestinationPort.ToString());
                        listView1.Items.Add(new ListViewItem(data.ToArray()));
                        listView1.Items[listView1.Items.Count - 1].EnsureVisible(); //scroll to end;
                    }

                }


                


            }

        }

        private void ProcessForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            foreach (ICaptureDevice dev in devices)
            {
                //dev.Close();       
                //dev.StopCapture();

            }
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                var id = listView1.SelectedIndices[0];
                var capture = capturedEvens[id];
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
