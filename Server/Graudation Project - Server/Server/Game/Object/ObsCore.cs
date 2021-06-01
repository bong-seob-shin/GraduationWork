using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Game
{
    public class CoreObstacle : GameObject
    {
        public CoreObstacle()
        {
            StatInfo.MaxHp = 80;
            StatInfo.Hp = 80;
        }
    }
}
