
// NwCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class NwCommand : Command, INwCommand
	{
		public override void Execute()
		{
			NextState = Globals.CreateInstance<IBeforePlayerMoveState>(x =>
			{
				x.Direction = Direction.Northwest;
			});
		}

		public NwCommand()
		{
			Synonyms = new string[] { "northwest" };

			SortOrder = 70;

			IsDarkEnabled = true;

			Name = "NwCommand";

			Verb = "nw";

			Type = CommandType.Movement;
		}
	}
}
