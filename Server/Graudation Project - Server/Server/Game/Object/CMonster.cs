﻿using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Server.Game
{
    public class CMonster : CMonsterSpawner
    {

        enum MonsterState 
        {
            NONE = 1,
            IDLE,
            MOVE,  
        }

        MonsterState m_state;

        int _nextTick = 0;

        Player _target;

        public CMonster()
        {
            ObjectType = GameObjectType.Cmonster;

            m_state = MonsterState.IDLE;
        }

        public override void Update()
        {
            switch (m_state)
            {
                case MonsterState.NONE:
                    break;

                case MonsterState.IDLE:
                    FindClosedPlayer();
                    break;

                case MonsterState.MOVE:
                    UpdateMoving();
                    break;
            }
        }

        int _checkPlayerTick = 0;
        private void FindClosedPlayer()
        {
            if (_checkPlayerTick > Environment.TickCount64)
                return;
            _checkPlayerTick = Environment.TickCount + 1000;

            Player target = Room.FindPlayer(p =>
            {
                float dist = DistanceToPoint(p.CellPos, CellPos);
                // 거리가 너무 먼 놈은 계산하지 마
                if (dist > 200f)
                    m_state = MonsterState.NONE;

                return dist <= 30f;
            });


            if (target == null)
                return;

            _target = target;

            Console.WriteLine("거리30안에 있는 target" + _target);

            m_state = MonsterState.MOVE;
        }

        int _movingTick = 0;
        private void UpdateMoving()
        {
            if (_movingTick > Environment.TickCount64)
                return;
            _movingTick = Environment.TickCount + 200;

            // 타켓이 있는지 확인
            if (_target == null || _target.Room != Room) 
            {
                _target = null;
                m_state = MonsterState.IDLE;
                BroadcastMove();
                return;
            }

            // 타켓과 몬스터와의 일정 거리가 넘어가면 IDLE 상태로 변경
            Vector3 dir = _target.CellPos - CellPos;
            float dist = DistanceToPoint(_target.CellPos, CellPos);
            if (dist > 50f)
            {
                _target = null;
                m_state = MonsterState.IDLE;
                BroadcastMove();
                return;
            }
            
            // 위의 상황이 모두 아니라면 이제 몬스터가 움직여도 되는 상황
            {
                CellPos = CellPos - new Vector3(0.1f, 0f, 0f);
                BroadcastMove();
            }
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
            Console.WriteLine("target : " + _target);
        }
    }

   
}
