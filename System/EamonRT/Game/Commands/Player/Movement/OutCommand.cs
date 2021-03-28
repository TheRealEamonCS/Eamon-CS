
// OutCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class OutCommand : Command, IOutCommand
	{
		public override void Execute()
		{
			NextState = Globals.CreateInstance<IBeforePlayerMoveState>(x =>
			{
				x.Direction = Direction.Out;
			});
		}

		public OutCommand()
		{
			SortOrder = 95;

			IsDarkEnabled = true;

			Uid = 76;

			Name = "OutCommand";

			Verb = "out";

			Type = CommandType.Movement;
		}
	}
}
