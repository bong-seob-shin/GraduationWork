using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Game
{
    public class MonsterSpawner : GameObject
    {
        public ClientSession Session { get; set; }

        public MonsterSpawner()
        {
            ObjectType = GameObjectType.Monster;
        }

        public override void Update()
        {
            Random rand = new Random();
            
            int x = rand.Next(0, 3000);
            int y = rand.Next(0, 500);
            int z = rand.Next(0, 2000);

            this.Info.PosInfo.PosX = x;
            this.Info.PosInfo.PosY = y;
            this.Info.PosInfo.PosZ = z;

        }
    }
}
