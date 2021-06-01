using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Game
{
    public class ObsWall : GameObject
    {
        public ObsWall()
        {
            StatInfo.MaxHp = 80;
            StatInfo.Hp = 80;
        }
    }
}
