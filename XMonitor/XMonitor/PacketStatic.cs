using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpPcap;
using SharpPcap.WinPcap;
using PacketDotNet;
using System.Runtime.CompilerServices;
using System.Collections.Concurrent;

namespace XMonitor
{
    class DevicStatistic 
    {
        public long packetReceivedNum = 0;
        public long packetReceiveSize = 0;
        public long lastUpdateTime = 0;
        public long pps = 0;
        public long bps = 0;
    }
    class PacketStatistic
    {

        public Dictionary<ICaptureDevice, DevicStatistic> devs = new Dictionary<ICaptureDevice, DevicStatistic>();
        public Dictionary<Connection, List<Packet>> packets = new Dictionary<Connection,List<Packet>>();
        public PacketStatistic()
        {
            var devList = SharpPcap.CaptureDeviceList.Instance;
            foreach(var dev in devList)
            {
                devs.Add(dev, new DevicStatistic());
            }
        }
        

        public long packetReceivedNum { get { return devs.Values.Sum(x => x.packetReceivedNum);} }
        public long packetReceiveSize { get { return devs.Values.Sum(x => x.packetReceiveSize); } }
        
        public long pps { get { return devs.Values.Sum(x=>x.pps);} }
        public long bps { get { return devs.Values.Sum(x=>x.bps);} }

        [MethodImpl(MethodImplOptions.Synchronized)]
        internal void update(CaptureEventArgs captureEventArgs)
        {
         
            var dev = captureEventArgs.Device;
            if( dev == null)
            {
                return;
            }
            if (!devs.ContainsKey(dev))
            {
                devs.Add(dev, new DevicStatistic());
            }
            var statistic = devs[dev];

            var now = (long)captureEventArgs.Packet.Timeval.MicroSeconds;
            if (statistic.lastUpdateTime == 0)
            {
                statistic.lastUpdateTime = now;
            }
            else
            {
                long intevel = now - statistic.lastUpdateTime;
                if(intevel < 0 )
                {
                    intevel *= -1;
                }
                statistic.pps = 1000000 / intevel;
                statistic.bps = (captureEventArgs.Packet.Data.Count() * 1000000) / intevel;


            }
            statistic.packetReceivedNum += 1;
            statistic.packetReceiveSize += captureEventArgs.Packet.Data.Count();

            var packet = Packet.ParsePacket(captureEventArgs.Packet.LinkLayerType, captureEventArgs.Packet.Data);

            var ipV4Packet = (IPv4Packet)packet.Extract(typeof(IPv4Packet));
            if (ipV4Packet != null)
            {

                var data = new List<string>();
                var time = captureEventArgs.Packet.Timeval;
                var tcpPacket = (TcpPacket)packet.Extract(typeof(TcpPacket));
                var udpPacket = (UdpPacket)packet.Extract(typeof(UdpPacket));
                var con = new Connection();
                if (tcpPacket != null)
                {
                    con.type = "TCP";
                    con.srcIp = ipV4Packet.SourceAddress.ToString();
                    con.srcPort = tcpPacket.SourcePort.ToString();
                    con.dstIp = ipV4Packet.DestinationAddress.ToString();
                    con.dstPort = tcpPacket.DestinationPort.ToString();
                    con.pid = -1;
                    if(!packets.ContainsKey(con))
                    {
                        packets[con] = new List<Packet>();
                    }
                    packets[con].Add(packet);
                    
                    

                }
                else if (udpPacket != null)
                {

                    con.type = "UDP";
                    con.srcIp = ipV4Packet.SourceAddress.ToString();
                    con.srcPort = udpPacket.SourcePort.ToString();
                    con.dstIp = ipV4Packet.DestinationAddress.ToString();
                    con.dstPort = udpPacket.DestinationPort.ToString();
                    con.pid = -1;
                    if (!packets.ContainsKey(con))
                    {
                        packets[con] = new List<Packet>();
                    }
                    packets[con].Add(packet);

                }

            }
        }
    }
}
