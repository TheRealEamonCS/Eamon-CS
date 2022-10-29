
// LookCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.Globals;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class LookCommand : Command, ILookCommand
	{
		public override void Execute()
		{
			ActorRoom.Seen = false;

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
		}

		public LookCommand()
		{
			SortOrder = 330;

			IsDobjPrepEnabled = true;

			Name = "LookCommand";

			Verb = "look";

			Type = CommandType.Miscellaneous;
		}
	}
}
