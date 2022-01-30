
// RemoveCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static TheTempleOfNgurct.Game.Plugin.PluginContext;

namespace TheTempleOfNgurct.Game.Commands
{
	[ClassMappings]
	public class RemoveCommand : EamonRT.Game.Commands.RemoveCommand, IRemoveCommand
	{
		public override void PrintCantVerbHere()
		{
			PrintEnemiesNearby();
		}

		public override bool IsAllowedInRoom()
		{
			return gGameState.GetNBTL(Friendliness.Enemy) <= 0;
		}

		public RemoveCommand()
		{
			IsPlayerEnabled = true;
		}
	}
}
