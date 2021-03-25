using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace ServerCore
{

    // 패킷 사용 요소
    public abstract class PacketSession : Session
    {
        public static readonly int HeaderSize = 2;
        // sealed - 다른 클래스가 패킷 세션을 상속 받은 다음 OnRecv 오버라이드 하려하면 Error 남 
        // 봉인 했다는 뜻 -> 패킷 세션을 상속 받은 애들은 이 인터페이스에서 받는 것이 아님
        public sealed override int OnRecv(ArraySegment<byte> buffer)
        {
            int processLen = 0;

            while (true)
            {
                // 최소한 헤더는 파싱할 수 있는지 확인
                if (buffer.Count < HeaderSize)
                    break;

                // 패킷이 완전체로 도착했는지 확인
                ushort dataSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
                if (buffer.Count < dataSize)
                    break;

                // 여기까지 왔으면 패킷 조립 가능
                OnRecvPacket(new ArraySegment<byte>(buffer.Array, buffer.Offset, dataSize));

                processLen += dataSize;
                buffer = new ArraySegment<byte>(buffer.Array, buffer.Offset + dataSize, buffer.Count - dataSize);
            }

            return processLen;
        }

        // PacketSession 상속 받은 애들은 OnRecv 말고 OnRecvPacket에서 받아라
        public abstract void OnRecvPacket(ArraySegment<byte> buffer);

    }


    // Session - 서버 엔진 요소
    public abstract class Session
    {
        Socket _socket;
        int _disconnected = 0;

        RecvBuffer _recvBuffer = new RecvBuffer(1024);

        object _lock = new object();
        Queue<ArraySegment<byte>> _sendQueue = new Queue<ArraySegment<byte>>();
        List<ArraySegment<byte>> _pendingList = new List<ArraySegment<byte>>();                // BufferList는 먼저 리스트에 Add하고 그걸 BufferList = list 이런식으로 넣어주면서 사용해야함. 바로 Add가 안됨
        SocketAsyncEventArgs _sendArgs = new SocketAsyncEventArgs();                           // 재사용하면 안되기 때문에 Send 함수에서 밖으로 빼줌
        SocketAsyncEventArgs _recvArgs = new SocketAsyncEventArgs();

        public abstract void OnConnected(EndPoint endPoint);
        public abstract int  OnRecv(ArraySegment<byte> buffer);
        public abstract void OnSend(int numOfBytes);
        public abstract void OnDisconnected(EndPoint endPoint);


        public void Start(Socket socket) 
        {
            _socket = socket;

            _recvArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnRecvCompleted);   
            _sendArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnSendCompleted);

            RegisterRecv();
        }

        public void Send(ArraySegment<byte> sendBuff) 
        {
            lock (_lock)
            {
                _sendQueue.Enqueue(sendBuff);
                if (_pendingList.Count == 0)
                    RegisterSend();
            }
        }

        public void Disconnect() 
        {
            // CAS 써도 됨
            if (Interlocked.Exchange(ref _disconnected, 1) == 1)
                return;

            OnDisconnected(_socket.RemoteEndPoint);
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
        }
        
        #region 네트워크 통신
        void RegisterSend()
        {          
            while (_sendQueue.Count > 0)
            {
                ArraySegment<byte> buff = _sendQueue.Dequeue();
                _pendingList.Add(buff);                                     // 버퍼를 하나씩 보내는건 비효율적(SetBuffer 대신 BufferList에 넣자)
                                                                            // C# - Class는 힙 영역, Struct는 스택 영역 --> ArraySegment는 구조체
                                                                            // C# 배열 - 포인터 없어서 어떤 인덱스에 접근 힘듬. 따라서  <배열,시작 인덱스,크기> 세트로 접근
            }
            _sendArgs.BufferList = _pendingList;

            bool pending = _socket.SendAsync(_sendArgs);
            if (pending == false)
                OnSendCompleted(null, _sendArgs);
        }

        void OnSendCompleted(object sender, SocketAsyncEventArgs args) 
        {
            lock (_lock)
            {
                if (args.BytesTransferred > 0 && args.SocketError == SocketError.Success)
                {
                    try
                    {
                        _sendArgs.BufferList = null;    // 버퍼리스트가 pendingList 가지고 있을 필요 없음 --> 초기화
                        _pendingList.Clear();           // 내가 예약하고 있는건 모두 성공했다는 의미니까 초기화

                        OnSend(_sendArgs.BytesTransferred);
                        _pendingList.Clear();

                        if (_sendQueue.Count > 0)
                            RegisterSend();                      
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"OnSendCompleted Failed {e}");
                    }
                }
                else
                {
                    Disconnect();
                }
            }
        }

        void RegisterRecv() 
        {
            _recvBuffer.Clean();
            ArraySegment<byte> segment = _recvBuffer.WriteSegment;
            _recvArgs.SetBuffer(segment.Array, segment.Offset, segment.Count);

            bool pending = _socket.ReceiveAsync(_recvArgs);
            if (pending == false)
                OnRecvCompleted(null, _recvArgs);
        }

        void OnRecvCompleted(object sender, SocketAsyncEventArgs args) 
        {
            if (args.BytesTransferred > 0 && args.SocketError == SocketError.Success)    // 몇 바이트 받았는지 check
            {
                try
                {
                    // Write 커서 이동
                    if (_recvBuffer.OnWrite(args.BytesTransferred) == false)
                    {
                        Disconnect();
                        return;
                    }

                    // 컨텐츠 쪽으로 데이터를 넘겨주고 얼마나 처리했는지 받는다
                    int processLen = OnRecv(_recvBuffer.ReadSegment);
                    if (processLen < 0 || _recvBuffer.DataSize < processLen)
                    {
                        Disconnect();
                        return;
                    }

                    // Read 커서 이동
                    if (_recvBuffer.OnRead(processLen) == false) 
                    {
                        Disconnect();
                        return;
                    }

                    RegisterRecv();
                }
                catch (Exception e) 
                {
                    Console.WriteLine($"OnRecvCompleted Failed {e}");
                }
            }
            else 
            {
                Disconnect();
            }
        }
        #endregion

    }
}
