using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf;
using Google.Protobuf.Protocol;
using Google.Protobuf.WellKnownTypes;
using Server.Game;
using ServerCore;

namespace Server
{
	class Program
	{
		static Listener _listener = new Listener();

		static void FlushRoom()
		{
			JobTimer.Instance.Push(FlushRoom, 250);
		}

		static void Main(string[] args)
		{
			RoomManager.Instance.Add(1);

			// DNS (Domain Name System)
			string host = Dns.GetHostName();
			IPHostEntry ipHost = Dns.GetHostEntry(host);
			IPAddress ipAddr = ipHost.AddressList[0];
			IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

			_listener.Init(endPoint, () => { return SessionManager.Instance.Generate(); });
			Console.WriteLine("Listening...");

			// 원격 접속위함
			//string host = Dns.GetHostName();
			//IPHostEntry ipHost = Dns.GetHostEntry(host);
			//IPAddress ipAddr = ipHost.AddressList[0];
			//IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

			//IPAddress ServerIp = IPAddress.Parse("192.168.0.17");
			//IPEndPoint ep = new IPEndPoint(ServerIp, 7777);

			//_listener.Init(ep, () => { return SessionManager.Instance.Generate(); });
			//Console.WriteLine("Listening...");

			// --원래 주석인 놈--
			//FlushRoom();
			//JobTimer.Instance.Push(FlushRoom);

			while (true)
			{
				//JobTimer.Instance.Flush();
				RoomManager.Instance.Find(1).Update();
			}
		}
	}
}
