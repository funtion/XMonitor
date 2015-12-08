using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Management;

namespace XMonitor
{

    class Proc
    {
        public int processId,parentId;
        public string processName;
        public List<Proc> children = new List<Proc>();
        public List<Connection> connections = new List<Connection>();


        public static List<Proc> getProcessTree()
        {

            var relations = new Dictionary<int, List<int>>();
            var procs = new Dictionary<int, Proc>();
            var searcher = new ManagementObjectSearcher("select * from win32_process");

            foreach(var res in searcher.Get())
            {
                // res info at https://msdn.microsoft.com/en-us/library/aa394372(v=vs.85).aspx

                int pid = Convert.ToInt32(res["ProcessId"].ToString());
                int ppid = Convert.ToInt32(res["ParentProcessId"].ToString());
                string name = res["Name"].ToString();
                procs[pid] = new Proc() { processId = pid, parentId = ppid, processName = name };

            }

            var pc = new ProcessConnection();
            foreach(var con in pc.connections)
            {
                if(con.pid != 0 && procs.ContainsKey(con.pid))
                    procs[con.pid].connections.Add(con);
            }

            var children = new HashSet<int>();
            foreach(var res in procs)
            {
                int pid = res.Value.processId;
                int ppid = res.Value.parentId;

         
                if (ppid != 0 && procs.ContainsKey(ppid))
                {
                    procs[ppid].children.Add(procs[pid]);
                    children.Add(pid);
                }
               
            }

            var result = new List<Proc>();
            foreach (var res in procs)
            { 
                if(!children.Contains(res.Key))
                {
                    result.Add(res.Value);
                }
            }
            return result;
        }

    }

    

}
