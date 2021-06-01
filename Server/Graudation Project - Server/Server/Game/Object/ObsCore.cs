using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Game
{
    public class ObsCore : GameObject
    {
        public ObsCore()
        {
            StatInfo.MaxHp = 80;
            StatInfo.Hp = 80;
        }
    }
}
