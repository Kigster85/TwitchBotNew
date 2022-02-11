using System.Net.Sockets;
using System.Net;
using System.Text;
using System;
using NLog;
using TwitchBot.Logging;
using static System.Console;

namespace TwitchBot.Connections
{
    class SocketConnection
    {
        private static readonly string EndOfLine = "<ØØØ>";

        private static readonly Logger logger = MyLogging.GetLogger();

        public static Socket StablishConnection(int portAddress = 11000)
        {
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, portAddress);

            //Creates the Socket for sending data over TCP.
            Socket socketConnection = new Socket(
                AddressFamily.InterNetwork,
                SocketType.Stream,
                ProtocolType.Tcp);

            bool isConnected = ConnectToServer(socketConnection, localEndPoint);

            if (!isConnected)
            {
                // TODO: Handle exception.
            }

            return socketConnection;
        }

        public static bool ConnectToServer(Socket socketConnection, IPEndPoint localEndPoint)
        {
            // Connects to host using IPEndPoint.
            socketConnection.Connect(localEndPoint);
            if (!socketConnection.Connected)
            {
                Console.WriteLine("Unable to connect to host");
            }
            // Use the SelectWrite enumeration to obtain Socket status.
            if (socketConnection.Poll(-1, SelectMode.SelectWrite))
            {
                Console.WriteLine("This Socket is writable.");
            }
            else if (socketConnection.Poll(-1, SelectMode.SelectRead))
            {
                Console.WriteLine("This Socket is readable.");
            }
            else if (socketConnection.Poll(-1, SelectMode.SelectError))
            {
                Console.WriteLine("This Socket has an error.");
            }
            return socketConnection.Connected;
        }

        // Displays sending with a connected socket
        // using the overload that takes a buffer.
        public static int Send(string msg)
        {
            // string endOfLine = "\r\n";

            logger.Debug($"Sending message to game: {msg}");

            byte[] encodedMessage = Encoding.UTF8.GetBytes(msg + EndOfLine);
            byte[] bytes = new byte[1024];

            try
            {
                // Blocks until send returns.
                Socket socketConnection = StablishConnection();
                int bytesSent = socketConnection.Send(encodedMessage);
                logger.Debug($"Sent {bytesSent} bytes.");
                // Get reply from the _clientSocket.
                int bytesReceived = socketConnection.Receive(bytes);
                logger.Debug($"Received: {Encoding.UTF8.GetString(bytes)}");
            }
            catch (SocketException ex)
            {
                logger.Error(ex);
                return (ex.ErrorCode);
            }
            return 0;
        }

    }

}


