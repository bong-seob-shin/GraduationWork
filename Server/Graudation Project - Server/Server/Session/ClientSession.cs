using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using ServerCore;
using System.Net;
using Google.Protobuf.Protocol;
using Google.Protobuf;
using Server.Game;

namespace Server
{
	public class ClientSession : PacketSession
	{
		public Player MyPlayer { get; set; }
		public int SessionId { get; set; }

		public void Send(IMessage packet)
		{
			// ID를 주는 방법 --> 프로토 버퍼에서 정의된 이름을 추출해서 그것을 아이디로 쓰자 
			string msgName = packet.Descriptor.Name.Replace("_", string.Empty);  
			MsgId msgid = (MsgId)Enum.Parse(typeof(MsgId), msgName);

			ushort size = (ushort)packet.CalculateSize();
			byte[] sendBuffer = new byte[size + 4];
			Array.Copy(BitConverter.GetBytes((ushort)(size + 4)), 0, sendBuffer, 0, sizeof(ushort));
			Array.Copy(BitConverter.GetBytes((ushort)msgid), 0, sendBuffer, 2, sizeof(ushort));
			Array.Copy(packet.ToByteArray(), 0, sendBuffer, 4, size);

			Send(new ArraySegment<byte>(sendBuffer));
		}

		public override void OnConnected(EndPoint endPoint)
		{
			Console.WriteLine($"OnConnected : {endPoint}");

			// 원래는 클라가 로딩이 끝나고 ok사인을 서버에 보내면 그때부터 처리하기 시작해야함
			MyPlayer = ObjectManager.Instance.Add<Player>();			
			{
				MyPlayer.Info.Name = $"Player_{MyPlayer.Info.ObjectId}";


				Random rand = new Random();
				int num = rand.Next(0, 5);

				MyPlayer.Info.PosInfo.PosX = 2259 + num;
                MyPlayer.Info.PosInfo.PosY = 110;
				MyPlayer.Info.PosInfo.PosZ = 3473 + num;
                //MyPlayer.Info.PosInfo.DirX = 0;
                //MyPlayer.Info.PosInfo.DirZ = 0;
                //MyPlayer.Info.PosInfo.IsJump = false;
                //MyPlayer.Info.PosInfo.RotY = 0;
                //MyPlayer.Info.PosInfo.State = State.Idle;

                MyPlayer.Session = this;
			}

			RoomManager.Instance.Find(1).EnterGame(MyPlayer);
		}

		public override void OnRecvPacket(ArraySegment<byte> buffer)
		{
			PacketManager.Instance.OnRecvPacket(this, buffer);
		}

		public override void OnDisconnected(EndPoint endPoint)
		{
			RoomManager.Instance.Find(1).LeaveGame(MyPlayer.Info.ObjectId);

			SessionManager.Instance.Remove(this);

			Console.WriteLine($"OnDisconnected : {endPoint}");
		}

		public override void OnSend(int numOfBytes)
		{
			//Console.WriteLine($"Transferred bytes: {numOfBytes}");
		}
	}
}
