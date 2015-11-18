using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SharpPcap;
using PacketDotNet;
using System.ComponentModel.Design;

namespace XMonitor
{
    public partial class PacketForm : Form
    {
        public CaptureEventArgs captureEventArgs;

        public PacketForm(CaptureEventArgs args)
        {
            captureEventArgs = args;
            InitializeComponent();
        }

        private void PacketForm_Load(object sender, EventArgs e)
        {
            var packet = Packet.ParsePacket(captureEventArgs.Packet.LinkLayerType, captureEventArgs.Packet.Data);
            var tcpPacket = (TcpPacket)packet.Extract(typeof(TcpPacket));
            if(tcpPacket != null)
            {
                showTcpInfo();
            }
            else
            {

                showUdpInfo();
            }
        }

        private void showUdpInfo()
        {
            throw new NotImplementedException();
        }

        private void showTcpInfo()
        {
            var packet = Packet.ParsePacket(captureEventArgs.Packet.LinkLayerType, captureEventArgs.Packet.Data);
            var ipV4Packet = (IPv4Packet)packet.Extract(typeof(IPv4Packet));
            var tcpPacket = (TcpPacket)packet.Extract(typeof(TcpPacket));

            var items = listBox1.Items;

            items.Add("TCP PACKET");
            var time = captureEventArgs.Packet.Timeval;
            items.Add(string.Format("Time : {0} {1}.{2}",time.Date.ToLongDateString(), time.Date.ToLongTimeString(),time.Date.Millisecond));
            items.Add(string.Format("src port: {0}", tcpPacket.SourcePort));
            
            ByteViewer bv = new ByteViewer();
            byte[] bytes = tcpPacket.Bytes;
            bv.SetBytes(bytes);
            this.Controls.Add(bv);



        }
    }
}
