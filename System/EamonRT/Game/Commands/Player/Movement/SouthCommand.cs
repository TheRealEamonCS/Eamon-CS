
// SouthCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class SouthCommand : Command, ISouthCommand
	{
		public override void Execute()
		{
			NextState = Globals.CreateInstance<IBeforePlayerMoveState>(x =>
			{
				x.Direction = Direction.South;
			});
		}

		public SouthCommand()
		{
			SortOrder = 10;

			IsDarkEnabled = true;

			Uid = 78;

			Name = "SouthCommand";

			Verb = "south";

			Type = CommandType.Movement;
		}
	}
}
