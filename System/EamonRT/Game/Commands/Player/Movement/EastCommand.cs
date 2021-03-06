
// EastCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class EastCommand : Command, IEastCommand
	{
		public override void Execute()
		{
			NextState = Globals.CreateInstance<IBeforePlayerMoveState>(x =>
			{
				x.Direction = Direction.East;
			});
		}

		public EastCommand()
		{
			SortOrder = 20;

			IsDarkEnabled = true;

			Uid = 69;

			Name = "EastCommand";

			Verb = "east";

			Type = CommandType.Movement;
		}
	}
}
