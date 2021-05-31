using Google.Protobuf;
using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Server.Game
{
    public class GameRoom
    {
        object _lock = new object();
        public int RoomId { get; set; }

        public int attacker { get; set; }
        public bool m_damaged { get; set; }
        public int test = 100;

        Dictionary<int, Player> _players = new Dictionary<int, Player>();
        Dictionary<int, CMonsterSpawner> _cmonsterspawn = new Dictionary<int, CMonsterSpawner>();
        Dictionary<int, CMonster> _cmonsters = new Dictionary<int, CMonster>();
        Dictionary<int, Buggy> _buggy = new Dictionary<int, Buggy>();

        public void Init(int mapId)
        {
            // 근거리 몬스터 스포너
            CMonsterSpawner CMonster_spawn = ObjectManager.Instance.Add<CMonsterSpawner>();
            CMonster_spawn.Info.PosInfo.SpineX = 2261;
            CMonster_spawn.Info.PosInfo.SpineY = 110;
            CMonster_spawn.Info.PosInfo.SpineZ = 3476;
            EnterGame(CMonster_spawn);

            CMonster CMonster = ObjectManager.Instance.Add<CMonster>();
            CMonster.CellPos = new Vector3(2240f, 110f, 3460f);
            EnterGame(CMonster);
        }

        public void Update()
        {
            lock (_lock)
            {
                foreach (CMonsterSpawner CMonseter_spawn in _cmonsterspawn.Values)
                {
                    CMonseter_spawn.Update();
                }

                foreach (CMonster CMonster in _cmonsters.Values)
                {
                    CMonster.Update();
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

                        foreach (CMonsterSpawner cms in _cmonsterspawn.Values)                        
                            spawnPacket.Objects.Add(cms.Info);

                        foreach (CMonster cm in _cmonsters.Values)
                            spawnPacket.Objects.Add(cm.Info);

                        foreach (Buggy b in _buggy.Values)
                            spawnPacket.Objects.Add(b.Info);

                        player.Session.Send(spawnPacket);
                    }
                }

                else if (type == GameObjectType.Cmonsterspawner)
                {
                    CMonsterSpawner CMonsterspawn = gameObject as CMonsterSpawner;
                    _cmonsterspawn.Add(gameObject.Id, CMonsterspawn);
                    CMonsterspawn.Room = this;
                }

                else if (type == GameObjectType.Cmonster)
                {
                    CMonster CMonster = gameObject as CMonster;
                    _cmonsters.Add(gameObject.Id, CMonster);
                    CMonster.Room = this;
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

                // 실시간으로 플레이어 좌표를 CellPos에 받아와서 몬스터와의 거리를 Check할 예정
                player.CellPos = new Vector3(movePacket.PosInfo.PosX, movePacket.PosInfo.PosY, movePacket.PosInfo.PosZ);

                // 다른 플레이어한테도 알려준다
                S_Move resMovePacket = new S_Move();
                resMovePacket.ObjectId = player.Info.ObjectId;
                resMovePacket.PosInfo = movePacket.PosInfo;

                Broadcast(resMovePacket);
            }
        }

        public void HandleAttack(Player player, C_Attack attackPacket)
        {
            if (player == null)
                return;

            lock (_lock)
            {
                AttackInfo attackInfo = attackPacket.AttackInfo;
                ObjectInfo info = player.Info;

                // 플레이어가 몬스터를 Hit하는 순간
                if (attackInfo.IsHit)
                {

                    attacker = info.ObjectId;
                    m_damaged = true;

                }

                // 다른 플레이어한테도 알려준다
                S_Attack resMovePacket = new S_Attack();
                resMovePacket.ObjectId = player.Info.ObjectId;
                resMovePacket.AttackInfo = attackPacket.AttackInfo;

                Broadcast(resMovePacket);
            }
        }

        public void HandleHp(Player player, C_ChangeHp hpPacket)
        {
            if (player == null)
                return;

            lock (_lock)
            {
                S_ChangeHp resHpPacket = new S_ChangeHp();

                CMonster cm = new CMonster();

                if (_cmonsters.TryGetValue(hpPacket.ObjectId, out cm))
                {
                    cm.StatInfo.Hp -= 80;
                    resHpPacket.StatInfo = cm.StatInfo;
                }

                resHpPacket.ObjectId = hpPacket.ObjectId;

                Console.WriteLine(resHpPacket.StatInfo.Hp);

                Broadcast(resHpPacket);
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
