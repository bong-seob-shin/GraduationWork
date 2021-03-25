using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;


namespace ServerCore
{
    public class Listener
    {
        Socket _listenSocket;
        Func<Session> _sessionFactory;  // 세션을 어떤 방식으로, 누구를 만들어 줄지 정의

        public void Init(IPEndPoint endPoint, Func<Session> sessionFactory) 
        {
            _listenSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _sessionFactory += sessionFactory;

            _listenSocket.Bind(endPoint);

            _listenSocket.Listen(10);

            // Connect 요청시 OnAcceptCompleted가 CallBack 방식으로 호출
            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptCompleted);
            RegisterAccept(args);        
        }

        void RegisterAccept(SocketAsyncEventArgs args) 
        {
            args.AcceptSocket = null;     // 이벤트 재사용시 초기화 반드시 필요!!

            // 비동기 방식으로 Accept 예약
            bool pending = _listenSocket.AcceptAsync(args);     // C++과 다른 부분
            if (pending == false)                               // 비동기 방식이지만 바로 완료 되었다는 뜻
                OnAcceptCompleted(null, args);
        }

        void OnAcceptCompleted(object sender, SocketAsyncEventArgs args)
        {
            if (args.SocketError == SocketError.Success) 
            {
                Session session = _sessionFactory.Invoke();
                session.Start(args.AcceptSocket);
                session.OnConnected(args.AcceptSocket.RemoteEndPoint);
            }
            else
                Console.WriteLine(args.SocketError.ToString());

            RegisterAccept(args);   // 위에까지 했으면 모든 일이 끝났으니 다음번 접속을 위해 다시 등록
        }

    }
}
