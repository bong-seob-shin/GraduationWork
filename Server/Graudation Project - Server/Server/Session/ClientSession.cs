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
using System.Numerics;

namespace Server
{
	public class ClientSession : PacketSession
	{
        public Player MyPlayer { get; set; }
		public CMonster CMonster { get; set; }
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

                // 던전 광장 앞
                //MyPlayer.Info.PosInfo.PosX = 1370 + num;
                //MyPlayer.Info.PosInfo.PosY = 226;
                //MyPlayer.Info.PosInfo.PosZ = 4930 + num;

                // 던전 외부
                //MyPlayer.Info.PosInfo.PosX = 1436 + num;
                //MyPlayer.Info.PosInfo.PosY = 263;
                //MyPlayer.Info.PosInfo.PosZ = 4935 + num;

                // 던전 내부
                MyPlayer.Info.PosInfo.PosX = 1394 + num;
                MyPlayer.Info.PosInfo.PosY = 226;
                MyPlayer.Info.PosInfo.PosZ = 4903 + num;

                // 마을 앞
                //MyPlayer.Info.PosInfo.PosX = 2235 + num;
                //MyPlayer.Info.PosInfo.PosY = 110;
                //MyPlayer.Info.PosInfo.PosZ = 3455 + num;

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
