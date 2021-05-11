using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.Game
{
	public class GameObject
	{
		public GameObjectType ObjectType { get; protected set; } = GameObjectType.None;
		public int Id
		{
			get { return Info.ObjectId; }
			set { Info.ObjectId = value; }
		}

		public GameRoom Room { get; set; }

		public ObjectInfo Info { get; set; } = new ObjectInfo();
		public PositionInfo PosInfo { get; private set; } = new PositionInfo();
		public StatInfo StatInfo { get; private set; } = new StatInfo();

		public GameObject()
		{
			Info.PosInfo = PosInfo;
			Info.StatInfo = StatInfo;
		}


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

	}
}
