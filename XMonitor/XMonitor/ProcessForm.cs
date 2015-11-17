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
        public ProcessForm(int pid)
        {
            this.pid = pid;
            this.process = Process.GetProcessById(pid);
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void ProcessForm_Load(object sender, EventArgs e)
        {
            Text = string.Format("Process: {0} ({1})", process.ProcessName, process.Id);

            var devices = CaptureDeviceList.Instance;
            var connectsions = new ProcessConnection().getConnectionByPID(pid);
            var filter = "";
            foreach(var con in connectsions)
            {
                
                filter += string.Format("( {0} and src host {1} and src port {2} and dst host {3} and dst port {4})",con.type.ToLower(),con.inIp,con.inPort,con.outIp,con.outPort);
                if(!con.Equals(connectsions.Last()))
                {
                    filter += " or ";
                }
            }
            
            foreach(ICaptureDevice dev in devices)
            {
                dev.OnPacketArrival += new SharpPcap.PacketArrivalEventHandler(device_OnPacketArrival);
                
                dev.Open(DeviceMode.Promiscuous, readTimeoutMilliseconds);
                dev.Filter = filter;
                dev.StartCapture();
            }

            
        }
        private delegate void delAddPackageToView(CaptureEventArgs packet);
        private void device_OnPacketArrival(object sender, CaptureEventArgs packet)
        {
            this.Invoke(new delAddPackageToView(addPackageToView), packet);
        }

        private void addPackageToView(CaptureEventArgs captureEventArgs)
        {
            var packet = Packet.ParsePacket(captureEventArgs.Packet.LinkLayerType, captureEventArgs.Packet.Data);

            var tcpPacket = TcpPacket.GetEncapsulated(packet);
            var item = new ListViewItem();
            item.SubItems.Add(captureEventArgs.Packet.Timeval.MicroSeconds.ToString());
            item.SubItems.Add(captureEventArgs.Packet.Data.Length.ToString());
            if(tcpPacket != null)
            {
                item.SubItems.Add("TCP");
            }
            else
            {
                var udpPacket = PacketDotNet.UdpPacket.GetEncapsulated(packet);
                if(udpPacket != null)
                {
                    item.SubItems.Add("UDP");
                }
            }
            
            listView1.Items.Add(item);
        }

    }
}
