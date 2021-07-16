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

        bool IsPatternStart = false;

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
            // 처음에 플레이어를 찾다가 
            if (IsPatternStart == false)
                FindClosedPlayer();

            // 플레이어가 찾아지면 Pattern을 랜덤하게 주기 시작한다
            if (IsPatternStart == true)
                RandPattern();
            
            // 랜덤하게 주어진 Pattern에 따라 동작해야할 행동을 준다
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
                Console.WriteLine("거리" + dist);
                return dist <= 10f;
            });

            if (target == null)
                return;

            _target = target;

            //Pattern = Pattern.One;
            //BroadcastBoss();

            IsPatternStart = true;
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

        // 15초 마다 4개의 패턴을 랜덤하게 주자.
        int _RandPatternTick = 0;
        private void RandPattern()
        {
            if (_RandPatternTick > Environment.TickCount64)
                return;
            _RandPatternTick = Environment.TickCount + 10000;

            Random rand = new Random();

            //int randPattern = rand.Next(1, 5);
            int randPattern = 4;

            if (randPattern == 1)
            {
                Pattern = Pattern.One;
                BroadcastBoss();
            }

            if (randPattern == 2)
            {
                Pattern = Pattern.Two;
                BroadcastBoss();
            }

            if (randPattern == 3)
            {
                Pattern = Pattern.Three;
                BroadcastBoss();
            }

            if (randPattern == 4)
            {
                Pattern = Pattern.Four;

                StatInfo.Speed = 1;
                int r = rand.Next(1, 12);
                RandWall = r;
                BroadcastBoss();
            }
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

