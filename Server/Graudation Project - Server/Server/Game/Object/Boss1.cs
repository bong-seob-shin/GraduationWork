using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Server.Game
{
    class Boss1 : GameObject
    {
        Player _target;

        bool check;

        public Boss1()
        {
            ObjectType = GameObjectType.Bossone;

            State = State.Idle;
            Pattern = Pattern.No;

            StatInfo.MaxHp = 3000;
            StatInfo.Hp = 3000;
        }

        public override void Update()
        {
            Random rand = new Random();

            FindClosedPlayer();
            if (check)
            {
                switch (Pattern) 
                {
                    case Pattern.One:
                        RandWall = rand.Next(1, 12);
                        Console.WriteLine(RandWall);
                        BroadcastBoss();
                        break;
                }
            }
            else if(!check)
                Pattern = Pattern.No;
        }

        int _checkPlayerTick = 0;
        private void FindClosedPlayer()
        {
            if (_checkPlayerTick > Environment.TickCount64)
                return;
            _checkPlayerTick = Environment.TickCount + 1000;

            Player target = Room.FindPlayer(p =>
            {
                float dist = DistanceToPoint(p.CellPos, CellPos);
                Console.WriteLine(dist);
                return dist <= 10f;
            });

            if (target == null)
                return;

            _target = target;

            Pattern = Pattern.One;
            BroadcastBoss();
        }

        private float DistanceToPoint(Vector3 a, Vector3 b)
        {
            return (float)Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2) + Math.Pow(a.Z - b.Z, 2));
        }

        void BroadcastBoss()
        {
            S_BossOne bossPkacet = new S_BossOne();
            bossPkacet.ObjectId = Id;
            bossPkacet.StatInfo = StatInfo;
            Room.Broadcast(bossPkacet);
        }
    }
}

