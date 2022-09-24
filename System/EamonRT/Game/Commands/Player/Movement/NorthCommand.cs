
// NorthCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class NorthCommand : Command, INorthCommand
	{
		public override void Execute()
		{
			NextState = Globals.CreateInstance<IBeforePlayerMoveState>(x =>
			{
				x.Direction = Direction.North;
			});
		}

		public NorthCommand()
		{
			SortOrder = 0;

			IsDarkEnabled = true;

			Name = "NorthCommand";

			Verb = "north";

			Type = CommandType.Movement;
		}
	}
}
