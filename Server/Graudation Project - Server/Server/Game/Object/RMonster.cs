using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Server.Game
{
    public class RMonster : GameObject
    {
        Player _target;

        private float rand_x;
        private float rand_z;

        public RMonster()
        {
            ObjectType = GameObjectType.Rmonster;

            State = State.Idle;

            StatInfo.MaxHp = 400;
            StatInfo.Hp = 400;
        }

        public override void Update()
        {
            RandomPos();
            Attack();
        }

        int _randTick = 0;
        private void RandomPos()
        {
            Random rand = new Random();

            if (_randTick > Environment.TickCount64)
                return;
            _randTick = Environment.TickCount + 3000;

            float minX, maxX, minZ, maxZ;
            float range = 10f;

            minX = CellPos.X - range;
            maxX = CellPos.X + range;
            minZ = CellPos.Z - range;
            maxZ = CellPos.Z - range;

            float f = (float)rand.NextDouble();

            rand_x = (f * 20f) + minX;
            rand_z = (f * 20f) + minZ;

            this.PosInfo.SpineX = rand_x;
            this.PosInfo.SpineZ = rand_z;

            BroadcastMove();
        }

        int _attackTick = 0;
        private void Attack()
        {
            if (_attackTick > Environment.TickCount64)
                return;
            _attackTick = Environment.TickCount + 2000;

            State = State.Attack;
            BroadcastMove();
        }


        private float DistanceToPoint(Vector3 a, Vector3 b)
        {
            return (float)Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2) + Math.Pow(a.Z - b.Z, 2));
        }

        void BroadcastMove()
        {
            S_Move movePacket = new S_Move();
            movePacket.ObjectId = Id;
            movePacket.PosInfo = PosInfo;
            Room.Broadcast(movePacket);
        }
    }
}