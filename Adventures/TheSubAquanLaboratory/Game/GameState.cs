
// GameState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Game.Attributes;

namespace TheSubAquanLaboratory.Game
{
	[ClassMappings(typeof(IGameState))]
	public class GameState : Eamon.Game.GameState, Framework.IGameState
	{
		[FieldName(1121)]
		public virtual long FoodButtonPushes { get; set; }

		public virtual bool Sterilize { get; set; }

		[FieldName(1122)]
		public virtual long Flood { get; set; }

		[FieldName(1123)]
		public virtual long FloodLevel { get; set; }

		[FieldName(1124)]
		public virtual long Elevation { get; set; }

		public virtual bool Energize { get; set; }

		[FieldName(1125)]
		public virtual long EnergyMaceCharge { get; set; }

		[FieldName(1126)]
		public virtual long LaserScalpelCharge { get; set; }

		public virtual bool CabinetOpen { get; set; }

		public virtual bool LockerOpen { get; set; }

		public virtual bool Shark { get; set; }

		public virtual bool FloorAttack { get; set; }

		[FieldName(1127)]
		public virtual long QuestValue { get; set; }

		public virtual bool ReadPlaque { get; set; }

		public virtual bool ReadTerminals { get; set; }

		[FieldName(1128)]
		public virtual long FakeWallExamines { get; set; }

		public virtual bool AlphabetDial { get; set; }

		public virtual bool ReadDisplayScreen { get; set; }

		[FieldName(1129)]
		public virtual long LabRoomsSeen { get; set; }

		public GameState()
		{
			EnergyMaceCharge = 120;

			LaserScalpelCharge = 40;
		}
	}
}
