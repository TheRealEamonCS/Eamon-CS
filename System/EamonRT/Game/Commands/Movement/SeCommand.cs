﻿
// SeCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.Globals;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class SeCommand : Command, ISeCommand
	{
		public override void ExecuteForPlayer()
		{
			NextState = gEngine.CreateInstance<IBeforePlayerMoveState>(x =>
			{
				x.Direction = Direction.Southeast;
			});
		}

		public SeCommand()
		{
			Synonyms = new string[] { "southeast" };

			SortOrder = 80;

			IsDarkEnabled = true;

			Name = "SeCommand";

			Verb = "se";

			Type = CommandType.Movement;
		}
	}
}
