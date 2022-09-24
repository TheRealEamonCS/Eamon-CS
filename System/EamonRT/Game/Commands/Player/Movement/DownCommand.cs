
// DownCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class DownCommand : Command, IDownCommand
	{
		public override void Execute()
		{
			NextState = Globals.CreateInstance<IBeforePlayerMoveState>(x =>
			{
				x.Direction = Direction.Down;
			});
		}

		public DownCommand()
		{
			SortOrder = 50;

			IsDarkEnabled = true;

			Name = "DownCommand";

			Verb = "down";

			Type = CommandType.Movement;
		}
	}
}
