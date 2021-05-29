﻿using Google.Protobuf.Protocol;
using System.Numerics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Server.Game
{
	public class GameObject
	{
        object _lock = new object();

        public GameObjectType ObjectType { get; protected set; } = GameObjectType.None;
		public int Id
		{
			get { return Info.ObjectId; }
			set { Info.ObjectId = value; }
		}

		public GameRoom Room { get; set; }

		public ObjectInfo Info { get; set; } = new ObjectInfo();
		public PositionInfo PosInfo { get; private set; } = new PositionInfo();
        public AttackInfo AttackInfo { get; private set; } = new AttackInfo();
        public StatInfo StatInfo { get; private set; } = new StatInfo();

		public GameObject()
		{
			Info.PosInfo = PosInfo;
            Info.AttackInfo = AttackInfo;
			Info.StatInfo = StatInfo;
		}

        public Vector3 CellPos 
        {
            get 
            {
                return new Vector3(PosInfo.PosX, PosInfo.PosY, PosInfo.PosZ);
            }
            set 
            {
                PosInfo.PosX = value.X;
                PosInfo.PosY = value.Y;
                PosInfo.PosZ = value.Z;
            }
        }

        public State State 
        {
            get
            {
                return PosInfo.State;
            }
            set 
            {
                PosInfo.State = value;
            }       
        }

        public bool IsShoot
        {
            get { return AttackInfo.IsShoot; }
            set
            {
                AttackInfo.IsShoot = value;
            }
        }

        public bool IsEquiq
        {
            get { return AttackInfo.IsEquiq; }

            set
            {
                AttackInfo.IsEquiq = value;
            }
        }

        public bool IsHit
        {
            get { return AttackInfo.IsHit; }

            set
            {
                AttackInfo.IsHit = value;
            }
        }

        public bool IsDead
        {
            get { return AttackInfo.IsDead; }

            set
            {
                AttackInfo.IsDead = value;
            }
        }


        //public int MaxHP_TESTING
        //{
        //    get { return AttackInfo.MaxHP; }

        //    set
        //    {
        //        AttackInfo.MaxHP = value;
        //    }
        //}

        //public int HP_TESTING
        //{
        //    get { return AttackInfo.HP; }

        //    set
        //    {
        //        AttackInfo.HP = value;
        //    }
        //}

        public int Level
        {
            get
            {
                return StatInfo.Level;
            }
            set
            {
                StatInfo.Level = value;
            }
        }

        public int HP
        {
            get
            {
                return StatInfo.Hp;
            }
            set
            {
                StatInfo.Hp = value;
            }
        }

        public int MaxHP
        {
            get
            {
                return StatInfo.MaxHp;
            }
            set
            {
                StatInfo.MaxHp = value;
            }
        }

        public int Attack
        {
            get
            {
                return StatInfo.Attack;
            }
            set
            {
                StatInfo.Attack = value;
            }
        }

        public float SSpeed
        {
            get
            {
                return StatInfo.Speed;
            }
            set
            {
                StatInfo.Speed = value;
            }
        }

        public int Exp
        {
            get
            {
                return StatInfo.TotalExp;
            }
            set
            {
                StatInfo.TotalExp = value;
            }
        }

        public virtual void Update()
		{

		}

        public virtual void OnDamaged()
        {
            //lock (_lock)
            //{
            //    if (Room.m_damaged)
            //    {
            //        StatInfo.Hp -= 80;
            //        Console.WriteLine(this.StatInfo.Hp);
            //    }
            //}
        }
	}
}
