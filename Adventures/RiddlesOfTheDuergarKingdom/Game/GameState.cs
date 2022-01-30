
// GameState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using Eamon.Framework;
using Eamon.Game.Attributes;
using static RiddlesOfTheDuergarKingdom.Game.Plugin.PluginContext;

namespace RiddlesOfTheDuergarKingdom.Game
{
	[ClassMappings(typeof(IGameState))]
	public class GameState : Eamon.Game.GameState, Framework.IGameState
	{
		public virtual long GradStudentCompanionUid { get; set; }

		public virtual long OreCartTracksRoomUid { get; set; }

		public virtual long GradStudentSuffixCounter { get; set; }

		public virtual long WinchCounter { get; set; }

		public virtual long MedicHealCounter { get; set; }

		public virtual long MedicAntiVenomCounter { get; set; }

		public virtual bool BerescroftMet { get; set; }

		public virtual bool WaiverSigned { get; set; }

		public virtual bool GradStudentRetreats { get; set; }

		public virtual bool WoodenCartAscending { get; set; }

		public virtual bool IronLeverDisabled { get; set; }

		public virtual bool PushingOreCart { get; set; }

		public virtual bool SteamTurbineRunning { get; set; }

		public virtual bool RockCrusherRunning { get; set; }

		public virtual bool RockGrinderRunning { get; set; }

		public virtual bool DebrisSifterRunning { get; set; }

		public virtual bool VolcanoErupting { get; set; }

		public virtual bool SewagePitVisited { get; set; }

		public virtual bool CoffeePotUsed { get; set; }

		public virtual long[] IbexVegetableUids { get; set; }

		public virtual IDictionary<long, long> PoisonedTargets { get; set; }

		public GameState()
		{
			// A counter to strip "the grad student" from the companion's name

			GradStudentSuffixCounter = 100;

			WinchCounter = 2;

			MedicHealCounter = 3;

			MedicAntiVenomCounter = 2;

			IbexVegetableUids = new long[3];

			PoisonedTargets = new Dictionary<long, long>();
		}
	}
}
