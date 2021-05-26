using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Server.Game
{

    public class CMonsterSpawner : GameObject
    {
        public List<Vector3> list = new List<Vector3>();

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

            this.Info.PosInfo.PosX = rand_x;
            this.Info.PosInfo.PosY = rand_y;
            this.Info.PosInfo.PosZ = rand_z;


            Vector3 pos = new Vector3(rand_x, rand_y, rand_z);

            list.Add(pos);

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
