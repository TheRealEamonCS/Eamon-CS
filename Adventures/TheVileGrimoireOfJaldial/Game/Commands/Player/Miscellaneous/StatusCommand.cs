
// StatusCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game.Commands
{
	[ClassMappings]
	public class StatusCommand : EamonRT.Game.Commands.StatusCommand, IStatusCommand
	{
		public override void Execute()
		{
			if (gGameState.ParalyzedTargets.ContainsKey(gGameState.Cm))
			{
				gOut.Print("You are paralyzed at this time.");
			}

			if (gGameState.ClumsyTargets.ContainsKey(gGameState.Cm))
			{
				gOut.Print("You are agility impaired at this time.");
			}

			gOut.Print("You are at {0} percent health.", (long)Math.Round((double)(ActorMonster.Hardiness - ActorMonster.DmgTaken) / (double)ActorMonster.Hardiness * 100));

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		public override bool ShouldPreTurnProcess()
		{
			return true;
		}
	}
}
