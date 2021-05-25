using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Game
{
    public class CMonsterSpawner : GameObject
    {
        public static float rand_x;
        public static float rand_y;
        public static float rand_z;

        int _nextTick = 0;

        public CMonsterSpawner()
        {
            ObjectType = GameObjectType.Cmonsterspawner;
        }

        public override void Update()
        {
            Random rand = new Random();
            
            // 3초에 1번만 실행하도록 한다.
            if (_nextTick > Environment.TickCount64)
                return;
            _nextTick = Environment.TickCount + 3000;

            float f = (float)rand.NextDouble();

            rand_x = (f * 500f) + 2000f;
            rand_y = (f * 300f) + 110f;
            rand_z = (f * 500f) + 3100f;

            // MonsterSpawner.PosInfo.PosX는 몬스터 스포너의 위치이고
            // MonsterSpwaner.PosInfo.SpineX는 몬스터 스포너 안에서 랜덤으로 몬스터를 생성해줄 위치이다.
            this.Info.PosInfo.SpineX = rand_x;
            this.Info.PosInfo.SpineY = rand_y;
            this.Info.PosInfo.SpineZ = rand_z;

            Console.WriteLine(this.Info.PosInfo.SpineX);

            
            BroadcastMove();
        }

        void BroadcastMove()
        {
            S_Move movePacket = new S_Move();
            movePacket.ObjectId = Id;
            movePacket.PosInfo = PosInfo;
            Room.Broadcast(movePacket);
        }
    }
}
