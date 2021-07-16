using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Server.Game
{
    public class Player : GameObject
    {
        public ClientSession Session { get; set; }

        public Player()
        {
            ObjectType = GameObjectType.Player;

            StatInfo.MaxHp = 100;
            StatInfo.Hp = 100;
        }

        //public override void Update()
        //{
        //    //CheckVisibility();
        //    //Check();
        //}

        //int _checkVisibilityTick = 0;
        //private void CheckVisibility()
        //{
        //    if (_checkVisibilityTick > Environment.TickCount64)
        //        return;
        //    _checkVisibilityTick = Environment.TickCount + 3000;

        //    Console.WriteLine("거리 : " + Room.Visibility()); 
        //    float dist = Room.Visibility();

        //    if (dist <= 20f)
        //    {
        //        Console.WriteLine("시야 내에");
        //        State = State.Moving;
        //        BroadcastMove();
        //    }
        //    else
        //    {
        //        Console.WriteLine("시야 바깥에");
        //        State = State.Dead;
        //        BroadcastMove();
        //    }
        //}

        //int _checkTick = 0;
        //private void Check()
        //{
        //    if (_checkTick > Environment.TickCount64)
        //        return;
        //    _checkTick = Environment.TickCount + 3000;

        //    CMonster target = Room.FindCMonster(p =>
        //    {
        //        float dist = DistanceToPoint(p.CellPos,CellPos);
        //        Console.WriteLine("거리? : " + dist);
        //        return dist <= 20f;
        //    });

        //    if (target != null)
        //    {
        //        Console.WriteLine("살아나야해");
        //        //State = State.Moving;
        //        //BroadcastMove();
        //    }
        //    else
        //    {
        //        Console.WriteLine("거리 20 바깥");
        //        //State = State.Dead;
        //        //BroadcastMove();
        //    }
        //}

        //private float DistanceToPoint(Vector3 a, Vector3 b)
        //{
        //    return (float)Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2) + Math.Pow(a.Z - b.Z, 2));
        //}

        //void BroadcastMove()
        //{
        //    S_Move movePacket = new S_Move();
        //    movePacket.ObjectId = Id;
        //    movePacket.PosInfo = PosInfo;
        //    Room.Broadcast(movePacket);
        //}
    }
}
