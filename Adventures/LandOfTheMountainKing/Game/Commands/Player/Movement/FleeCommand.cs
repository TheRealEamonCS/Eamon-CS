
// FleeCommand.cs

// Copyright (c) 2014+ by Kenneth Pedersen.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static LandOfTheMountainKing.Game.Plugin.PluginContext;

namespace LandOfTheMountainKing.Game.Commands
{
	[ClassMappings]
	public class FleeCommand : EamonRT.Game.Commands.FleeCommand, IFleeCommand
	{
		public override void PrintCantVerbHere()
		{
			gEngine.PrintEffectDesc(59);
		}

		public override bool IsAllowedInRoom()
		{
			return false; // Disable Fleeing everywhere
		}
	}
}
