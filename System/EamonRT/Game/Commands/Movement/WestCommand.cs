
// WestCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.Globals;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class WestCommand : Command, IWestCommand
	{
		public override void ExecuteForPlayer()
		{
			NextState = gEngine.CreateInstance<IBeforePlayerMoveState>(x =>
			{
				x.Direction = Direction.West;
			});
		}

		public WestCommand()
		{
			SortOrder = 30;

			IsDarkEnabled = true;

			Name = "WestCommand";

			Verb = "west";

			Type = CommandType.Movement;
		}
	}
}
