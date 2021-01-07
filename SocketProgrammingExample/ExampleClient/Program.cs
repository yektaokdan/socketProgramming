using ExampleClient.Sockets;
using ExampleDataTransferObjects;
using System;
using System.Net;
using System.Linq;

namespace ExampleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            int port = 5555;
            Console.WriteLine(string.Format("Client Başlatıldı. Port: {0}", port));
            Console.WriteLine("-----------------------------");

            ExampleSocket exampleSocket = new ExampleSocket(new IPEndPoint(IPAddress.Parse("88.245.198.100"), port));
            exampleSocket.Start();

            Console.WriteLine("Göndermek için \"G\", basınız...");

            int count = 1;
            while (Console.ReadLine().ToUpper() == "G")
            {
                ExampleDTO exampleDTO = new ExampleDTO()
                {
                    Status = string.Format("{0}. Alındı", count),
                    Message = string.Format("{0} ip numaralı client üzerinden geliyorum!", GetLocalIPAddress()),
                    ozelData = string.Format("Merhaba Arkadaslar")
                };

                exampleSocket.SendData(exampleDTO);
                count++;
            }

            Console.ReadLine();
        }

        static string GetLocalIPAddress()
        {
            string localIP = Dns.GetHostEntry(Dns.GetHostName()).AddressList.Where(a => a.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).FirstOrDefault().ToString();

            return localIP;
        }
    }
}