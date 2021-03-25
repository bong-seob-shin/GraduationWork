using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace ServerCore
{
    // 분산 서버를 위함
    public class Connector
    {
        Func<Session> _sessionFactory;

        public void Connect(IPEndPoint endPoint, Func<Session> sessionFactory) 
        {
            Socket socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _sessionFactory = sessionFactory;
            
            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.Completed += OnConnectCompleted;
            args.RemoteEndPoint = endPoint;
            args.UserToken = socket;            // // --> 추가 정보 넣기(식별자 구별, 연동하고 싶은 데이터)


            RegisterConnect(args);
        }

        void RegisterConnect(SocketAsyncEventArgs args)
        {
            Socket socket = args.UserToken as Socket;
            if (socket == null)
                return;
            
            bool pending = socket.ConnectAsync(args);
            if (pending == false)
                OnConnectCompleted(null, args);
        }

        void OnConnectCompleted(object sender, SocketAsyncEventArgs args)
        {
            if (args.SocketError == SocketError.Success)
            {
                // 어떤 세션으로 갈 것인지 정해주자
                Session session = _sessionFactory.Invoke(); // 컨텐츠 딴에서 요구한 방식한 세션을 만듬
                session.Start(args.ConnectSocket);          // Start함수 -> recv까지 완료됨
                session.OnConnected(args.RemoteEndPoint);
            }
            else 
            {
                Console.WriteLine($"OnConnectCompleted Fail : {args.SocketError}");
            }

        }
    }
}
