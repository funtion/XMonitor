using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace XMonitor
{

    struct Connection
    {
        public string type;
        public string inIp;
        public string inPort;
        public string outIp;
        public string outPort;
        public string status;
        public int pid;
    }

    class ProcessConnection
    {
        public List<Connection> connections = new List<Connection>();
        
        private string getPort(string s)
        {
            s =  s.Split(':').Last();
            if(s.Equals("*"))
            {
                s = "0";
            }
            return s;
        }

        private string getIp(string s)
        {
            s =  s.Substring(0, s.LastIndexOf(':'));
            if(s.Equals("*"))
            {
                s = "0.0.0.0";
            }
            return s;
        }

        public ProcessConnection() 
        {
            var process = new Process();
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.FileName = "netstat";
            process.StartInfo.Arguments = "-ano";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.ErrorDialog = false;
            process.Start();
            var outReader = process.StandardOutput;
            //
            string line;
            while( (line = outReader.ReadLine()) != null)
            {
                string[] tokens = Regex.Split(line.Trim(), "\\s+");
                if (tokens[0] == "TCP")
                {
                    connections.Add(new Connection() { 
                        type = "TCP",
                        inIp = getIp(tokens[1]),
                        inPort = getPort(tokens[1]),
                        outIp = getIp(tokens[2]),
                        outPort = getPort(tokens[2]),
                        status = tokens[3],
                        pid = Int32.Parse(tokens[4])
                    });
                }
                else if(tokens[0] == "UDP")
                {
                    connections.Add(new Connection()
                    {
                        type = "UDP",
                        inIp = getIp(tokens[1]),
                        inPort = getPort(tokens[1]),
                        outIp = getIp(tokens[2]),
                        outPort = getPort(tokens[2]),
                        //status = tokens[3],
                        pid = Int32.Parse(tokens[3])
                    });
                }

            }
        }
        public List<Connection> getConnectionByPID(int pid)
        {
            return connections.FindAll(con => con.pid == pid);
        }


  

    }
}
