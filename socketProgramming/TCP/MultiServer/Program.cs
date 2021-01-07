using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MultiServer
{
    class Program
    {
        private static readonly Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static readonly List<Socket> clientSockets = new List<Socket>();
        private const int BUFFER_SIZE = 2048;
        private const int PORT = 12235;
        private static readonly byte[] buffer = new byte[BUFFER_SIZE];

        static void Main()
        {
            Console.Title = "Server";
            SetupServer();
            Console.ReadLine();
            CloseAllSockets();
        }

        private static void SetupServer()
        {
            Console.WriteLine("Setting up server...");
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, PORT));
            serverSocket.Listen(0);
            serverSocket.BeginAccept(AcceptCallback, null);
            Console.WriteLine("Server setup complete");
        }

        private static void CloseAllSockets()
        {
            foreach (Socket socket in clientSockets)
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }

            serverSocket.Close();
        }

        private static void AcceptCallback(IAsyncResult AR)
        {
            Socket socket;

            try
            {
                socket = serverSocket.EndAccept(AR);
            }
            catch (ObjectDisposedException) 
            {
                return;
            }

           clientSockets.Add(socket);
           socket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback, socket);
           Console.WriteLine("Istemci baglandi istek bekleniyor...");
           serverSocket.BeginAccept(AcceptCallback, null);
        }

        private static void ReceiveCallback(IAsyncResult AR)
        {
            Socket current = (Socket)AR.AsyncState;
            int received;

            try
            {
                received = current.EndReceive(AR);
            }
            catch (SocketException)
            {
                Console.WriteLine("İstemci baglantisi kesildi..");
                current.Close(); 
                clientSockets.Remove(current);
                return;
            }

            byte[] recBuf = new byte[received];
            Array.Copy(buffer, recBuf, received);
            string text = Encoding.ASCII.GetString(recBuf);
            
            Console.WriteLine("Received Text: " + text);
          
            if (text.ToLower() == "merhaba")
            {
                //Console.WriteLine("Text is a get time request");
                byte[] data = Encoding.ASCII.GetBytes("Merhaba, hangi resmi almak istiyorsunuz? 1) Aslan 2) Kedi 3) Balik 4) Fil 5) Kus ");
                current.Send(data);
               // Console.WriteLine("Time sent to client");
            }
            else if (text.ToLower() == "1")
            {
                byte[] data = Encoding.ASCII.GetBytes("Aslan");
                current.Send(data);
            }
            else if (text.ToLower() == "2")
            {
                byte[] data = Encoding.ASCII.GetBytes("Kedi");
                current.Send(data);
            }
            else if (text.ToLower() == "3")
            {
                byte[] data = Encoding.ASCII.GetBytes("Balik");
                current.Send(data);
            }
            else if (text.ToLower() == "4")
            {
                byte[] data = Encoding.ASCII.GetBytes("Fil");
                current.Send(data);
            }
            else if (text.ToLower() == "5")
            {
                byte[] data = Encoding.ASCII.GetBytes("Kus");
                current.Send(data);
            }
            else if (text.ToLower() == "exit") 
            {

                current.Shutdown(SocketShutdown.Both);
                current.Close();
                clientSockets.Remove(current);
                Console.WriteLine("Client disconnected");
                return;
            }
            else
            {
               
                byte[] data = Encoding.ASCII.GetBytes("Gecersiz deger");
                current.Send(data);
                Console.WriteLine("Uyari");
            }

            current.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, ReceiveCallback, current);
        }
    }
}
