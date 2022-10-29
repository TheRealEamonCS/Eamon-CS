
// NeCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.Globals;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class NeCommand : Command, INeCommand
	{
		public override void Execute()
		{
			NextState = gEngine.CreateInstance<IBeforePlayerMoveState>(x =>
			{
				x.Direction = Direction.Northeast;
			});
		}

		public NeCommand()
		{
			Synonyms = new string[] { "northeast" };

			SortOrder = 60;

			IsDarkEnabled = true;

			Name = "NeCommand";

			Verb = "ne";

			Type = CommandType.Movement;
		}
	}
}
