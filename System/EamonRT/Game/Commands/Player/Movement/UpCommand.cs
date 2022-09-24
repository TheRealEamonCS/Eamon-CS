
// UpCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class UpCommand : Command, IUpCommand
	{
		public override void Execute()
		{
			NextState = Globals.CreateInstance<IBeforePlayerMoveState>(x =>
			{
				x.Direction = Direction.Up;
			});
		}

		public UpCommand()
		{
			SortOrder = 40;

			IsDarkEnabled = true;

			Name = "UpCommand";

			Verb = "up";

			Type = CommandType.Movement;
		}
	}
}
