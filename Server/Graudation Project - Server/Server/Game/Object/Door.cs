using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Game
{
    public class Door : GameObject
    {
        public Door()
        {
            ObjectType = GameObjectType.Door;

            StatInfo.MaxHp = 150;
            StatInfo.Hp = 150;
        }
    }
}



