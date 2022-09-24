
// SwCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class SwCommand : Command, ISwCommand
	{
		public override void Execute()
		{
			NextState = Globals.CreateInstance<IBeforePlayerMoveState>(x =>
			{
				x.Direction = Direction.Southwest;
			});
		}

		public SwCommand()
		{
			Synonyms = new string[] { "southwest" };

			SortOrder = 90;

			IsDarkEnabled = true;

			Name = "SwCommand";

			Verb = "sw";

			Type = CommandType.Movement;
		}
	}
}
