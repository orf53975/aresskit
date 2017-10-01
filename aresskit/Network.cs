using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace aresskit
{
    public class Network
    {
        public static bool checkInternetConn(string server)
        {
            using (Ping pingSender = new Ping())
            {
                PingReply reply = pingSender.Send(server);
                return reply.Status == IPStatus.Success ? true : false;
            }
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }

        public static string GetPublicIPAddress()
        {
            using (WebClient client = new WebClient())
            {
                client.Headers.Add("user-agent", "curl");
                return client.DownloadString("http://ipinfo.io/");
            }
            // return new WebClient().DownloadString("ipinfo.io");
        }
    }
}
