﻿using Google.Protobuf;
using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Game
{
    public class GameRoom
    {
        object _lock = new object();
        public int RoomId { get; set; }

        Dictionary<int, Player> _players = new Dictionary<int, Player>();
        Dictionary<int, MonsterSpawner> _monsters = new Dictionary<int, MonsterSpawner>();
        Dictionary<int, Buggy> _buggy = new Dictionary<int, Buggy>();

        public void Init(int mapId)
        {
            // 근거리 몬스터 스포너
            //MonsterSpawner monster = ObjectManager.Instance.Add<MonsterSpawner>();
            //monster.PosInfo.PosX = 2261;
            //monster.PosInfo.PosY = 110;
            //monster.PosInfo.PosZ = 3476;
            //EnterGame(monster);
        }

        public void Update()
        {
            lock (_lock)
            {
                foreach (MonsterSpawner monster in _monsters.Values)
                {
                    monster.Update();
                }
            }
        }

        public void EnterGame(GameObject gameObject)
        {
            if (gameObject == null)
                return;

            GameObjectType type = ObjectManager.GetObjectTypeById(gameObject.Id);

            lock (_lock)
            {
                if (type == GameObjectType.Player)
                {
                    Player player = gameObject as Player;
                    _players.Add(gameObject.Id, player);
                    player.Room = this;

                    // 본인한테 본인에 대한 정보 보내기
                    {
                        S_EnterGame enterPacket = new S_EnterGame();
                        enterPacket.Player = player.Info;
                        player.Session.Send(enterPacket);

                        // 본인한테 타인에 대한 정보 보내기
                        S_Spawn spawnPacket = new S_Spawn();
                        foreach (Player p in _players.Values)
                        {
                            if (player != p)
                                spawnPacket.Objects.Add(p.Info);
                        }

                        foreach (MonsterSpawner m in _monsters.Values)                        
                            spawnPacket.Objects.Add(m.Info);

                        foreach (Buggy b in _buggy.Values)
                            spawnPacket.Objects.Add(b.Info);

                        player.Session.Send(spawnPacket);
                    }
                }

                else if (type == GameObjectType.Monster)
                {
                    MonsterSpawner monster = gameObject as MonsterSpawner;
                    _monsters.Add(gameObject.Id, monster);
                    monster.Room = this;
                }

                else if (type == GameObjectType.Buggy)
                {
                    Buggy buggy = gameObject as Buggy;
                    _buggy.Add(gameObject.Id, buggy);
                    buggy.Room = this;
                }


                // 타인한테 새로운 신입에 대한 정보 보내기
                {
                    S_Spawn spawnPacket = new S_Spawn();
                    spawnPacket.Objects.Add(gameObject.Info);                    
                    foreach (Player p in _players.Values)
                    {
                        if (p.Id != gameObject.Id)
                            p.Session.Send(spawnPacket);
                    }
                }
            }
        }

        public void LeaveGame(int objectId)
        {
            GameObjectType type = ObjectManager.GetObjectTypeById(objectId);

            lock (_lock)
            {
                if (type == GameObjectType.Player)
                {
                    Player player = null;
                    if (_players.Remove(objectId, out player) == false)
                        return;

                    player.Room = null;

                    // 본인한테 정보 전송
                    {
                        S_LeaveGame leavePacket = new S_LeaveGame();
                        player.Session.Send(leavePacket);
                    }
                }

                //else if (type == GameObjectType.Monster)
                //{
                //    Monster monster = null;
                //    if (_monsters.Remove(objectId, out monster) == false)
                //        return;

                //    monster.Room = null;
                //}

                //else if (type == GameObjectType.Buggy)
                //{
                //    Buggy buggy = null;
                //    if (_buggy.Remove(objectId, out buggy) == false)
                //        return;

                //    buggy.Room = null;
                //}

                // 타인한테 정보 전송
                {
                    S_Despawn despawnPacket = new S_Despawn();
                    despawnPacket.ObjectIds.Add(objectId);
                    foreach (Player p in _players.Values)
                    {
                        if (p.Id != objectId)
                            p.Session.Send(despawnPacket);
                    }
                }
            }
        }

        public void HandleMove(Player player, C_Move movePacket)
        {
            if (player == null)
                return;

            lock (_lock)
            {
                // 일단 서버에서 좌표 이동
                PositionInfo movePosInofo = movePacket.PosInfo;
                ObjectInfo info = player.Info;
                
                // 다른 플레이어한테도 알려준다
                S_Move resMovePacket = new S_Move();
                resMovePacket.ObjectId = player.Info.ObjectId;
                resMovePacket.PosInfo = movePacket.PosInfo;

                Broadcast(resMovePacket);
            }
        }

        public Player FindPlayer(Func<GameObject, bool> condition)
        {
            foreach (Player player in _players.Values)
            {
                if (condition.Invoke(player))
                    return player;
            }

            return null;
        }

        public void Broadcast(IMessage packet)
        {
            lock (_lock)
            {
                foreach (Player p in _players.Values)
                {
                    p.Session.Send(packet);
                }
            }
        }
    }
}
