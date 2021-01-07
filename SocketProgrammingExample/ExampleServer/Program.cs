using ExampleServer.Sockets;
using System;

namespace ExampleServer
{
    class Program
    {
        static void Main(string[] args)
        {
            int port = 5555;
            Console.WriteLine(string.Format("Server Başlatıldı. Port: {0}", port));
            Console.WriteLine("-----------------------------");

            Listener listener = new Listener(port, 50);//ServerSocket

            listener.Start();

            Console.ReadLine();
        }
    }
}
