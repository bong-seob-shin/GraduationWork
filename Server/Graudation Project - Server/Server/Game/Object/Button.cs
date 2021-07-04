using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Game
{
    public class Button : GameObject
    {
        public Button()
        {
            ObjectType = GameObjectType.Button;
        }

        public override void Update()
        {
        }
    }
}
