
// GameState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Game.Attributes;

namespace ARuncibleCargo.Game
{
	[ClassMappings(typeof(IGameState))]
	public class GameState : Eamon.Game.GameState, Framework.IGameState
	{
		[FieldName(1121)]
		public virtual long DreamCounter { get; set; }

		[FieldName(1122)]
		public virtual long SwarmyCounter { get; set; }

		[FieldName(1123)]
		public virtual long CargoOpenCounter { get; set; }

		[FieldName(1124)]
		public virtual long CargoInRoom { get; set; }

		[FieldName(1125)]
		public virtual long GiveAmazonMoney { get; set; }

		public virtual bool[] PookaMet { get; set; }

		public virtual bool AmazonMet { get; set; }

		public virtual bool BillAndAmazonMeet { get; set; }

		public virtual bool PrinceMet { get; set; }

		public virtual bool AmazonLilWarning { get; set; }

		public virtual bool BillLilWarning { get; set; }

		public virtual bool FireEscaped { get; set; }

		public virtual bool CampEntered { get; set; }

		public virtual bool PaperRead { get; set; }

		public virtual bool Explosive { get; set; }

		public virtual bool GetPookaMet(long index)
		{
			return PookaMet[index];
		}

		public virtual void SetPookaMet(long index, bool value)
		{
			PookaMet[index] = value;
		}

		public GameState()
		{
			DreamCounter = 1;

			SwarmyCounter = 1;

			PookaMet = new bool[3];
		}
	}
}
