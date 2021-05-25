using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Game
{
    public class CMonster : CMonsterSpawner
    {
        // int _nextTick = 0;

        public CMonster()
        {
            ObjectType = GameObjectType.Cmonster;
        }

        //public override void Update()
        //{
        //    if (_nextTick > Environment.TickCount64)
        //        return;
        //    _nextTick = Environment.TickCount + 3000;

        //    this.Info.PosInfo.PosX = rand_x;
        //    this.Info.PosInfo.PosY = rand_y;
        //    this.Info.PosInfo.PosZ = rand_z;

        //    Console.WriteLine(this.Info.PosInfo.PosY);
        //    BroadcastMove();
        //}

        //void BroadcastMove()
        //{
        //    S_Move movePacket = new S_Move();
        //    movePacket.ObjectId = Id;
        //    movePacket.PosInfo = PosInfo;
        //    Room.Broadcast(movePacket);
        //}
    }
}
