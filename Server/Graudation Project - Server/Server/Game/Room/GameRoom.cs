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
        Dictionary<int, RMonster> _rmonsters = new Dictionary<int, RMonster>();

        Dictionary<int, Buggy> _buggy = new Dictionary<int, Buggy>();
        Dictionary<int, Boss1> _boss1 = new Dictionary<int, Boss1>();

        public List<Vector3> CMonster_pos = new List<Vector3>();
        public List<Vector3> RMonster_pos = new List<Vector3>();

        public void Init(int mapId)
        {
            // 근거리 몬스터 스포너
            //CMonsterSpawner CMonster_spawn = ObjectManager.Instance.Add<CMonsterSpawner>();
            //CMonster_spawn.Info.PosInfo.SpineX = 2261;
            //CMonster_spawn.Info.PosInfo.SpineY = 110;
            //CMonster_spawn.Info.PosInfo.SpineZ = 3476;
            //EnterGame(CMonster_spawn);

            InitCMosPos();
            InitRMosPos();

            for (int i = 0; i < CMonster_pos.Count; ++i)
            {
                CMonster CMonster = ObjectManager.Instance.Add<CMonster>();
                CMonster.CellPos = CMonster_pos[i];
                EnterGame(CMonster);
            }

            for (int i = 0; i < RMonster_pos.Count; ++i)
            {
                RMonster RMonster = ObjectManager.Instance.Add<RMonster>();
                RMonster.CellPos = RMonster_pos[i];
                EnterGame(RMonster);
            }

            Boss1 boss1 = ObjectManager.Instance.Add<Boss1>();
            boss1.CellPos = new Vector3(1412.6f, 225.914f, 4909.44f);
            EnterGame(boss1);
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

                foreach (RMonster RMonster in _rmonsters.Values)
                {
                    RMonster.Update();
                }

                foreach (Boss1 boss1 in _boss1.Values)
                {
                    boss1.Update();
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

                        foreach (RMonster rm in _rmonsters.Values)
                            spawnPacket.Objects.Add(rm.Info);

                        foreach (Buggy b in _buggy.Values)
                            spawnPacket.Objects.Add(b.Info);
                        
                        foreach (Boss1 b in _boss1.Values)
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

                else if (type == GameObjectType.Rmonster)
                {
                    RMonster RMonster = gameObject as RMonster;
                    _rmonsters.Add(gameObject.Id, RMonster);
                    RMonster.Room = this;
                }

                else if (type == GameObjectType.Buggy)
                {
                    Buggy buggy = gameObject as Buggy;
                    _buggy.Add(gameObject.Id, buggy);
                    buggy.Room = this;
                }

                else if (type == GameObjectType.Bossone)
                {
                    Boss1 boss1 = gameObject as Boss1;
                    _boss1.Add(gameObject.Id, boss1);
                    boss1.Room = this;
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
                // player.CellPos = new Vector3(movePacket.PosInfo.PosX, movePacket.PosInfo.PosY, movePacket.PosInfo.PosZ);

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
                CMonster cm = new CMonster();
                RMonster rm = new RMonster();
                Player p = new Player();

                if (_cmonsters.TryGetValue(hpPacket.ObjectId, out cm))
                {
                    S_ChangeHp resHpPacket = new S_ChangeHp();

                    cm.StatInfo.Hp -= 80;
                    resHpPacket.StatInfo = cm.StatInfo;
                    resHpPacket.ObjectId = hpPacket.ObjectId;
                    Broadcast(resHpPacket);
                }

                if (_rmonsters.TryGetValue(hpPacket.ObjectId, out rm))
                {
                    S_ChangeHp resHpPacket = new S_ChangeHp();

                    rm.StatInfo.Hp -= 80;
                    resHpPacket.StatInfo = rm.StatInfo;
                    resHpPacket.ObjectId = hpPacket.ObjectId;
                    Broadcast(resHpPacket);
                }

                if (_players.TryGetValue(hpPacket.ObjectId, out p))
                {
                    S_ChangeHp resHpPacket = new S_ChangeHp();

                    p.StatInfo.Hp -= 20;
                    resHpPacket.StatInfo = p.StatInfo;
                    resHpPacket.ObjectId = hpPacket.ObjectId;
                    Broadcast(resHpPacket);

                    Console.WriteLine(resHpPacket.StatInfo.Hp);
                }


            }
        }

        public void HandleBossOne(Player player, C_BossOne bossPacket)
        { 

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

        public void InitCMosPos()
        {

            //CMonster_pos.Add(new Vector3(2230f, 1100f, 3450f));
            //CMonster_pos.Add(new Vector3(2240f, 110f, 3460f));

            CMonster_pos.Add(new Vector3(1379.603f, 225.955f, 4927.907f));
            CMonster_pos.Add(new Vector3(1375.874f, 225.9664f, 4936.51f));
            CMonster_pos.Add(new Vector3(1379.292f, 225.7642f, 4937.684f));
            CMonster_pos.Add(new Vector3(1381.721f, 226.0201f, 4927.731f));
            CMonster_pos.Add(new Vector3(1378.797f, 225.7046f, 4947.362f));
            CMonster_pos.Add(new Vector3(1383.534f, 227.9f, 4922.81f));
            CMonster_pos.Add(new Vector3(1367.629f, 225.8263f, 4916.181f));
            CMonster_pos.Add(new Vector3(1334.967f, 225.819f, 4917.107f));
            CMonster_pos.Add(new Vector3(1322.452f, 225.8191f, 4919.089f));
            CMonster_pos.Add(new Vector3(1311.953f, 225.8191f, 4923.274f));
            CMonster_pos.Add(new Vector3(1305.114f, 225.8191f, 4923.394f));
            CMonster_pos.Add(new Vector3(1321.666f, 225.8191f, 4923.547f));
            CMonster_pos.Add(new Vector3(1322.427f, 225.8869f, 4895.235f));
            CMonster_pos.Add(new Vector3(1321.666f, 225.8191f, 4923.547f));
            CMonster_pos.Add(new Vector3(1333.071f, 225.7859f, 4897.628f));
            CMonster_pos.Add(new Vector3(1355.344f, 225.7682f, 4891.913f));
            CMonster_pos.Add(new Vector3(1366.68f, 225.7855f, 4894.526f));
        }

        public void InitRMosPos()
        {
            RMonster_pos.Add(new Vector3(1351.342f, 225.7424f, 4932.287f));
            RMonster_pos.Add(new Vector3(1354.841f, 225.7495f, 4940.246f));
            RMonster_pos.Add(new Vector3(1352.508f, 225.8775f, 4936.074f));
            RMonster_pos.Add(new Vector3(1351.052f, 225.8932f, 4927.855f));
            RMonster_pos.Add(new Vector3(1355.406f, 225.8946f, 4946.48f));
            RMonster_pos.Add(new Vector3(1351.418f, 225.7309f, 4943.882f));
            RMonster_pos.Add(new Vector3(1329.277f, 225.8191f, 4949.434f));
            RMonster_pos.Add(new Vector3(1339.585f, 225.819f, 4913.517f));
            RMonster_pos.Add(new Vector3(1336.227f, 225.819f, 4913.339f));
            RMonster_pos.Add(new Vector3(1320.332f, 225.8191f, 4913.95f));
            RMonster_pos.Add(new Vector3(1302.683f, 225.8191f, 4927.36f));
            RMonster_pos.Add(new Vector3(1335.822f, 225.8977f, 4892.334f));
            RMonster_pos.Add(new Vector3(1348.545f, 225.9144f, 4896.864f));
            RMonster_pos.Add(new Vector3(1349.079f, 225.9183f, 4892.653f));
            RMonster_pos.Add(new Vector3(1364.575f, 225.7416f, 4898.844f));
        }
    }
}
