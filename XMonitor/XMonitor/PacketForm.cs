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

using Be.Windows.Forms;

namespace XMonitor
{

    public struct Field
    {
        public string name;
        public int start;
        public int len;

        public Field(string name,int start, int len)
        {
            this.name = name;
            this.start = start;
            this.len = len;
        }


        internal string decode(byte[] p)
        {
            Int64 val = 0;
            int bytePos = start/ 8;
            int bitPos = 7 - start % 8;
            for (int i = 0; i < len; i++)
            {
                var v = p[bytePos] & ( 1 << (bitPos) );
                val = val * 2 + ( v == 0 ? 0 : 1);
                bitPos--;
                if(bitPos == -1)
                {
                    bitPos = 7;
                    bytePos++;
                }
            }
            
            return string.Format("0x{0:X4}({0})", val);


        }
    }
    public partial class PacketForm : Form
    {

        private RawCapture rawCapture;

        private Field[] tcpFields = {
                new Field("src port",0,16),
                new Field("dst port",16,16),
                new Field("seq number",32,32),
                new Field("ACK number",64,32),
                new Field("data offset",96,4),
                new Field("reserved",100,3),
                new Field("flags",103,9),
                new Field("win size",112,16),
                new Field("checksum",128,16),
                new Field("URG pointer",144,16),
                new Field("options",160,-1)
        };

        private Field[] udpFields = { 
                new Field("src port",0, 16),
                new Field("dst port",16, 16),
                new Field("length",32,16),
                new Field("check sum",48,16)
    
        };

        

        public PacketForm(RawCapture capture)
        {
           
            this.rawCapture = capture;
            InitializeComponent();
        }

        private void PacketForm_Load(object sender, EventArgs e)
        {
            var packet = Packet.ParsePacket(rawCapture.LinkLayerType, rawCapture.Data);
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
            var packet = Packet.ParsePacket(rawCapture.LinkLayerType, rawCapture.Data);
            var ipV4Packet = (IPv4Packet)packet.Extract(typeof(IPv4Packet));
            var udpPacket = (UdpPacket)packet.Extract(typeof(UdpPacket));

            foreach (var field in udpFields)
            {
                var item = new ListViewItem(new[] { field.name, field.decode(udpPacket.Bytes) });
                item.Tag = field;
                lvData.Items.Add(item);
            }


            hexBox1.ByteProvider = new DynamicByteProvider(udpPacket.Bytes);
        }

        private void showTcpInfo()
        {
            var packet = Packet.ParsePacket(rawCapture.LinkLayerType, rawCapture.Data);
            var ipV4Packet = (IPv4Packet)packet.Extract(typeof(IPv4Packet));
            var tcpPacket = (TcpPacket)packet.Extract(typeof(TcpPacket));

            foreach(var field in tcpFields)
            {
                var item = new ListViewItem(new[] { field.name, field.decode(tcpPacket.Bytes) });
                item.Tag = field;
                lvData.Items.Add(item);
            }


            hexBox1.ByteProvider = new DynamicByteProvider(tcpPacket.Bytes);
            
            


        }



        private void lvData_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(lvData.SelectedItems.Count == 0)
            {
                hexBox1.Select(0, 0);
                return;
            }
            var field = (Field)lvData.SelectedItems[0].Tag;
            hexBox1.Select(field.start / 8, field.len / 8 + (field.len%8==0?0:1));
        }
    }
}
