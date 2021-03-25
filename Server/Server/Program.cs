using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ServerCore;

namespace Server
{
    class Program
    {
        static Listener _listener = new Listener();

        static void Main(string[] args)
        {
            PacketManager.Instance.Register();

            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            _listener.Init(endPoint, () => { return new ClientSession(); }); // 리스너는 무엇을 만들어 줄지만 결정
            Console.WriteLine("Listening...");

            while (true)
            {
                // 이 부분은 주 스레드, 비동기 AcceptCompleted는 작업자 스레드 
                // 따라서 2개의 부분이 서로 쓰레드가 건드리기 때문에 data race 조심해야함
                ; 
            }
        }
    }
}
