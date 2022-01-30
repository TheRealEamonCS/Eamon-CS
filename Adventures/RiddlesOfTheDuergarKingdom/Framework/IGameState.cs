
// IGameState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using static RiddlesOfTheDuergarKingdom.Game.Plugin.PluginContext;

namespace RiddlesOfTheDuergarKingdom.Framework
{
	public interface IGameState : Eamon.Framework.IGameState
	{
		#region Properties

		/// <summary></summary>
		long GradStudentCompanionUid { get; set; }

		/// <summary></summary>
		long OreCartTracksRoomUid { get; set; }

		/// <summary></summary>
		long GradStudentSuffixCounter { get; set; }

		/// <summary></summary>
		long WinchCounter { get; set; }

		/// <summary></summary>
		long MedicHealCounter { get; set; }

		/// <summary></summary>
		long MedicAntiVenomCounter { get; set; }

		/// <summary></summary>
		bool BerescroftMet { get; set; }

		/// <summary></summary>
		bool WaiverSigned { get; set; }

		/// <summary></summary>
		bool GradStudentRetreats { get; set; }

		/// <summary></summary>
		bool WoodenCartAscending { get; set; }

		/// <summary></summary>
		bool IronLeverDisabled { get; set; }

		/// <summary></summary>
		bool PushingOreCart { get; set; }

		/// <summary></summary>
		bool SteamTurbineRunning { get; set; }

		/// <summary></summary>
		bool RockCrusherRunning { get; set; }

		/// <summary></summary>
		bool RockGrinderRunning { get; set; }

		/// <summary></summary>
		bool DebrisSifterRunning { get; set; }

		/// <summary></summary>
		bool VolcanoErupting { get; set; }

		/// <summary></summary>
		bool SewagePitVisited { get; set; }

		/// <summary></summary>
		bool CoffeePotUsed { get; set; }

		long[] IbexVegetableUids { get; set; }

		/// <summary></summary>
		IDictionary<long, long> PoisonedTargets { get; set; }

		#endregion
	}
}
