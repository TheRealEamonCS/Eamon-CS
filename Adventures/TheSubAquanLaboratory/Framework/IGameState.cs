
// IGameState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

namespace TheSubAquanLaboratory.Framework
{
	/// <summary></summary>
	public interface IGameState : Eamon.Framework.IGameState
	{
		/// <summary></summary>
		long FoodButtonPushes { get; set; }

		/// <summary></summary>
		bool Sterilize { get; set; }

		/// <summary></summary>
		long Flood { get; set; }

		/// <summary></summary>
		long FloodLevel { get; set; }

		/// <summary></summary>
		long Elevation { get; set; }

		/// <summary></summary>
		bool Energize { get; set; }

		/// <summary></summary>
		long EnergyMaceCharge { get; set; }

		/// <summary></summary>
		long LaserScalpelCharge { get; set; }

		/// <summary></summary>
		bool CabinetOpen { get; set; }

		/// <summary></summary>
		bool LockerOpen { get; set; }

		/// <summary></summary>
		bool Shark { get; set; }

		/// <summary></summary>
		bool FloorAttack { get; set; }

		/// <summary></summary>
		long QuestValue { get; set; }

		/// <summary></summary>
		bool ReadPlaque { get; set; }

		/// <summary></summary>
		bool ReadTerminals { get; set; }

		/// <summary></summary>
		long FakeWallExamines { get; set; }

		/// <summary></summary>
		bool AlphabetDial { get; set; }

		/// <summary></summary>
		bool ReadDisplayScreen { get; set; }

		/// <summary></summary>
		long LabRoomsSeen { get; set; }
	}
}
