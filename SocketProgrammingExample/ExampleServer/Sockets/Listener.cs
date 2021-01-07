using ExampleDataTransferObjects;
using System;
using System.Net;
using System.Net.Sockets;

namespace ExampleServer.Sockets
{
    public class Listener
    {
        #region Variables
        Socket _Socket;
        int _Port;
        int _MaxConnectionQueue;
        #endregion

        #region Constructor
        public Listener(int port, int maxConnectionQueue)
        {
            _Port = port;
            _MaxConnectionQueue = maxConnectionQueue;

            // Socket'i tanımlıyoruz IPv4, socket tipimiz stream olacak ve TCP Protokolü
            //ile haberleşeceğiz. 
            // TCP Protokolünde server belirlenen portu dinler ve gelen istekleri 
            //karşılar oysaki UDP Protokolünde tek bir socket üzerinden birden çok client'a ulaşmak mümkündür.
            _Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }
        #endregion

        #region Public Methods
        public void Start()
        {
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, _Port);

            // Socket'e herhangi bir yerden ve belirttiğimiz porttan gelecek olan bağlantıları belirtmeliyiz.
            _Socket.Bind(ipEndPoint);

            // Socketten gelecek olan bağlantıları dinlemeye başlıyoruz ve maksimum dinleyeceği bağlantıyı belirtiyoruz.
            _Socket.Listen(_MaxConnectionQueue);

            // BeginAccept ile asenkron olarak gelen bağlantıları kabul ediyoruz.
            _Socket.BeginAccept(OnBeginAccept, _Socket);
        }
        #endregion

        #region Private Methods
        void OnBeginAccept(IAsyncResult asyncResult)
        {
            Socket socket = _Socket.EndAccept(asyncResult);
            Client client = new Client(socket);

            // Client tarafından gönderilen datamızı işleyeceğimiz kısım.
            client._OnExampleDTOReceived += new Sockets.OnExampleDTOReceived(OnExampleDTOReceived);

            Cevap cevap = new Cevap()
            {
                Status = string.Format("{0}. Alındı", 12),
                V1 = string.Format("{ 0  A 990 } ip numaralı client üzerinden geliyorum!"),
                V2 = string.Format("Merhaba Arkadaslar".ToUpper())
            };

            client.SendData(cevap);

            client.Start();

            // Tekrardan dinlemeye devam diyoruz.
            _Socket.BeginAccept(OnBeginAccept, null);
        }

        void OnExampleDTOReceived(ExampleDTO exampleDTO)
        {
            // Client tarafından gelen data, istediğiniz gibi burada handle edebilirsiniz senaryonuza göre.
            Console.WriteLine(string.Format("Status: {0}", exampleDTO.Status));
            Console.WriteLine(string.Format("Message: {0}", exampleDTO.Message));
            Console.WriteLine(string.Format("Ozel Message: {0}", exampleDTO.ozelData));
        }

        void onSendData(ExampleDTO exampleDTO)
        {
            // Client tarafından gelen data, istediğiniz gibi burada handle edebilirsiniz senaryonuza göre.
            Console.WriteLine(string.Format("Status: {0}", exampleDTO.Status));
            Console.WriteLine(string.Format("Message: {0}", exampleDTO.Message));
            Console.WriteLine(string.Format("Ozel Message: {0}", exampleDTO.ozelData));
        }
        #endregion


    }
}