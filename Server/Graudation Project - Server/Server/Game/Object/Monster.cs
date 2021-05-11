using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Game
{
    class Monster : GameObject
    {
        public Monster()
        {
            ObjectType = GameObjectType.Monster;

            StatInfo.Level = 1;
            StatInfo.Hp = 100;
            StatInfo.MaxHp = 100;
            StatInfo.Speed = 5.0f;
        }

        public override void Update()
        {
        }
    }
}
