﻿using Google.Protobuf;
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

	public static void C_AttackHandler(PacketSession session, IMessage packet)
	{
		C_Attack attackPacket = packet as C_Attack;
		ClientSession clientSession = session as ClientSession;

		Player player = clientSession.MyPlayer;
		if (player == null)
			return;

		GameRoom room = player.Room;
		if (room == null)
			return;

		room.HandleAttack(player, attackPacket);
	}

    public static void C_ChangeHpHandler(PacketSession session, IMessage packet)
    {
        C_ChangeHp hpPacket = packet as C_ChangeHp;

        ClientSession clientSession = session as ClientSession;

        Player player = clientSession.MyPlayer;
        if (player == null)
            return;

        GameRoom room = player.Room;
        if (room == null)
            return;

        room.HandleHp(player, hpPacket);
    }

	public static void C_CarHandler(PacketSession session, IMessage packet)
	{
		C_Car carPacket = packet as C_Car;
		ClientSession clientSession = session as ClientSession;

		Player player = clientSession.MyPlayer;
		if (player == null)
			return;

		GameRoom room = player.Room;
		if (room == null)
			return;

		room.HandleCar(player, carPacket);
	}

	public static void C_ButtonHandler(PacketSession session, IMessage packet)
	{
		C_Button buttonPacket = packet as C_Button;
		ClientSession clientSession = session as ClientSession;

		Player player = clientSession.MyPlayer;
		if (player == null)
			return;

		GameRoom room = player.Room;
		if (room == null)
			return;

		room.HandleButton(player, buttonPacket);
	}
	


	public static void C_BossOneHandler(PacketSession session, IMessage packet)
	{

    }

	

}
