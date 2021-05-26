using Google.Protobuf;
using Google.Protobuf.Protocol;
using Server;
using Server.Game;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;

class PacketHandler
{
	public static void C_MoveHandler(PacketSession session, IMessage packet)
	{
		C_Move movePacket = packet as C_Move;
		ClientSession clientSession = session as ClientSession;

		Player player = clientSession.MyPlayer;
		if (player == null)
			return;

		GameRoom room = player.Room;
		if (room == null)
			return;

		room.HandleMove(player, movePacket);
	}

	public static void C_MonsterHandler(PacketSession session, IMessage packet)
	{
		C_Monster monsterPacket = packet as C_Monster;
        ClientSession clientSession = session as ClientSession;

		CMonster cmonster = new CMonster();

		Player player = clientSession.MyPlayer;
		if (player == null)
			return;

		GameRoom room = player.Room;
		if (room == null)
			return;

		room.HandleMonster(player, cmonster, monsterPacket);
	}
	


}
