using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SocketServer
{
    class Program
    {
        static void Main(string[] args)
        {
            RunServer();
        }

        public static void RunServer()
        {
            /// <summary>
            /// Establish the endpoint
            /// For the socket. Dns.GetHostName
            /// Return the name of the host
            /// running the application
            /// </summary>
            int port = 5000;
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, port);

            /// <summary>
            /// Creating TCP/IP SOcket using socket constructor 
            /// </summary>
            Socket server = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                /// <summary>
                /// Using Bind() method we associate network address with socket
                /// all client connect to that server must know that network address
                /// </summary>
                server.Bind(endPoint);

                /// <summary>
                /// Using listen() method we create Client List in queue
                /// </summary>
                server.Listen(10);

                while (true)
                {
                    Console.WriteLine($"Waiting connection on IP: {ipAddr} and Port: {port} .....");

                    /// <sumary>
                    /// Suspend while waiting for clients using Accept() method
                    /// </summary>
                    Socket clientSocket = server.Accept();

                    // Data buffer
                    byte[] bytes = new Byte[1024];
                    string data = null;

                    while (true)
                    {
                        int numBytes = clientSocket.Receive(bytes);
                        data += Encoding.ASCII.GetString(bytes, 0, numBytes);

                        if (data.IndexOf("<EOF>") > -1)
                            break;
                    }

                    Console.WriteLine("Text received -> {0} ", data);
                    byte[] message = Encoding.ASCII.GetBytes("1Test Server");

                    // Send a message to Client  
                    // using Send() method 
                    clientSocket.Send(message);

                    // Close client Socket using the 
                    // Close() method. After closing, 
                    // we can use the closed Socket  
                    // for a new Client Connection 
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
