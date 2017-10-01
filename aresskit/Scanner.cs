using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace aresskit
{
    class Scanner
    {
        public void portScanner(string hostname, Array range, bool verbose)
        {
            // port scanner
            if (range == null)
            {
                for (int x = 1; x <= 65535; x++)
                {
                    TcpClient tcpClient = new TcpClient();
                    try
                    {
                        tcpClient.Connect(hostname, x);
                        Console.WriteLine("TCP Port [{1}] on {0} seems to be open!", hostname, x);
                        tcpClient.Close(); // Close connection
                    }
                    catch
                    {
                        tcpClient.Close();
                        // this port is closed
                        if (verbose == true)
                        {
                            Console.WriteLine("TCP Port [{1}] on {0} seems to be closed!", hostname, x);
                        }
                    }
                }
            }
            else
            {
                foreach (int port in range)
                {
                    TcpClient tcpClient = new TcpClient();
                    try
                    {
                        tcpClient.Connect(hostname, port);
                        Console.WriteLine("TCP Port [{1}] on {0} seems to be open!", hostname, port);
                        tcpClient.Close(); // Close connection
                    }
                    catch
                    {
                        // this port is closed
                        if (verbose == true)
                        {
                            Console.WriteLine("TCP Port [{1}] on {0} seems to be closed!", hostname, port);
                        }
                    }
                }
            }
        }

        public void netDiscover()
        {
            var local = NetworkInterface.GetAllNetworkInterfaces().Where(i => i.Name == "Wireless LAN adapter Wi-Fi 2").FirstOrDefault();
            IPHostEntry hostEntry = Dns.GetHostEntry(Environment.MachineName);
            string lanIP = local != null ? local.GetIPProperties().UnicastAddresses[0].Address.ToString() : hostEntry.AddressList[hostEntry.AddressList.Length - 1].ToString();
            string _lanIP = Misc.GetFirst(lanIP, (lanIP.LastIndexOf('.') + 1));

            List<string> range = new List<string>();
            for (int x = 1; x < 256; x++)
            {
                range.Add(_lanIP + x.ToString());
            }

            List<string> hosts = new List<string>();
            foreach (string ip in range)
            {
                Ping p = new Ping();
                PingReply rep = p.Send(ip, 1);
                if (rep.Status == IPStatus.Success)
                {
                    hosts.Add(ip);
                    Console.WriteLine(ip + " is online.");
                }
            }
        }
    }
}
