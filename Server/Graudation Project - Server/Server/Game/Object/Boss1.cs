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
            switch (Pattern)
            {
                case Pattern.No:
                    FindClosedPlayer();
                    break;
                case Pattern.One:
                    RandNum();
                    break;
            }
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
                //Console.WriteLine(dist);
                return dist <= 10f;
            });

            if (target == null)
                return;

            _target = target;

            Pattern = Pattern.One;
            BroadcastBoss();
        }

        int _RandWallTick = 0;
        private void RandNum()
        {
            if (_RandWallTick > Environment.TickCount64)
                return;
            _RandWallTick = Environment.TickCount + 3000;

            Random rand = new Random();
            StatInfo.Speed = 1;
            int r = rand.Next(1, 12);
            RandWall = r;

            // 이 시기에 생성할 수 있도록 하자..
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

