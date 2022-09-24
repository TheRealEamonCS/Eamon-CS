
// InCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class InCommand : Command, IInCommand
	{
		public override void Execute()
		{
			NextState = Globals.CreateInstance<IBeforePlayerMoveState>(x =>
			{
				x.Direction = Direction.In;
			});
		}

		public InCommand()
		{
			SortOrder = 93;

			IsDarkEnabled = true;

			Name = "InCommand";

			Verb = "in";

			Type = CommandType.Movement;
		}
	}
}
