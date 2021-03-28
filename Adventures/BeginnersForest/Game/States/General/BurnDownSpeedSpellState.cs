
// BurnDownSpeedSpellState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static BeginnersForest.Game.Plugin.PluginContext;

namespace BeginnersForest.Game.States
{
	[ClassMappings]
	public class BurnDownSpeedSpellState : EamonRT.Game.States.BurnDownSpeedSpellState, IBurnDownSpeedSpellState
	{
		public override void PrintSpeedSpellExpired()
		{
			base.PrintSpeedSpellExpired();

			// Super Magic Agility Ring (patent pending)

			var ringArtifact = gADB[2];

			Debug.Assert(ringArtifact != null);

			if (ringArtifact.IsWornByCharacter())
			{
				gEngine.PrintEffectDesc(17);
			}

			ringArtifact.SetInLimbo();
		}
	}
}
