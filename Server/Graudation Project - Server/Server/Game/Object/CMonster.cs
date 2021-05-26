using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Game
{
    public class CMonster : CMonsterSpawner
    {
        public ClientSession Session { get; set; }

        int _nextTick = 0;

        public CMonster()
        {
            ObjectType = GameObjectType.Cmonster;
        }

        //public override void Update()
        //{
        //}

        //void BroadcastMove()
        //{
        //}
    }

   
}
