using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpPcap;
using SharpPcap.WinPcap;
using PacketDotNet;
using System.Runtime.CompilerServices;

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
        
        private Dictionary<ICaptureDevice, DevicStatistic> devs = new Dictionary<ICaptureDevice, DevicStatistic>();
        
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void update(StatisticsModeEventArgs e)
        {
            if(e.Device == null)
            {
                return;
            }
            if(!devs.ContainsKey(e.Device))
            {
                devs.Add(e.Device, new DevicStatistic());
            }
            var dev = devs[e.Device];
            var now = (long)e.Statistics.Timeval.MicroSeconds;
            var s = e.Statistics;
            if (dev.lastUpdateTime == 0)
            {
                dev.lastUpdateTime = now;
            }
            else
            {
                var intevel = now - dev.lastUpdateTime;
                dev.pps = (s.RecievedPackets * 1000000) / intevel;
                dev.bps = (s.RecievedBytes * 8 * 1000000) / intevel;
            }
            dev.packetReceivedNum += s.RecievedPackets;
            dev.packetReceiveSize += s.RecievedBytes;
        }

        public long packetReceivedNum { get { return devs.Values.Sum(x=>x.packetReceivedNum);} }
        public long packetReceiveSize { get { return devs.Values.Sum(x => x.packetReceiveSize); } }
        
        public long pps { get { return devs.Values.Sum(x=>x.pps);} }
        public long bps { get { return devs.Values.Sum(x=>x.bps);} }
    }
}
