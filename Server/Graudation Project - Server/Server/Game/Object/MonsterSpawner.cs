﻿using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Game
{
    public class MonsterSpawner : GameObject
    {
        int _nextTick = 0; 


        public MonsterSpawner()
        {
            ObjectType = GameObjectType.Monster;
        }

        public override void Update()
        {
            Random rand = new Random();
            

            // 3초에 1번만 실행하도록 한다.
            if (_nextTick > Environment.TickCount64)
                return;
            _nextTick = Environment.TickCount + 3000;

            float f = (float)rand.NextDouble();

            float x = (f * 500f) + 2000f;
            float y = (f * 300f) + 110f;
            float z = (f * 500f) + 3100f;

            // MonsterSpawner.PosInfo.PosX는 몬스터 스포너의 위치이고
            // MonsterSpwaner.PosInfo.SpineX는 몬스터 스포너 안에서 랜덤으로 몬스터를 생성해줄 위치이다.
            this.Info.PosInfo.SpineX = x;
            this.Info.PosInfo.SpineY = y;
            this.Info.PosInfo.SpineZ = z;

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
