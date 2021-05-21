using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Game
{
    class Buggy : GameObject
    {
        public Buggy()
        {
            ObjectType = GameObjectType.Buggy;
        }

        public override void Update()
        {
        }
    }
}
