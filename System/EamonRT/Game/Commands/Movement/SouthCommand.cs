
// SouthCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.Globals;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class SouthCommand : Command, ISouthCommand
	{
		public override void ExecuteForPlayer()
		{
			NextState = gEngine.CreateInstance<IBeforePlayerMoveState>(x =>
			{
				x.Direction = Direction.South;
			});
		}

		public SouthCommand()
		{
			SortOrder = 10;

			IsDarkEnabled = true;

			Name = "SouthCommand";

			Verb = "south";

			Type = CommandType.Movement;
		}
	}
}
