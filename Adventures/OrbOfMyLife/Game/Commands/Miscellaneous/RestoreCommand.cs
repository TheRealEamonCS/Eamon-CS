﻿
// RestoreCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static OrbOfMyLife.Game.Plugin.Globals;

namespace OrbOfMyLife.Game.Commands
{
	[ClassMappings]
	public class RestoreCommand : EamonRT.Game.Commands.RestoreCommand, IRestoreCommand
	{
		public override void ExecuteForPlayer()
		{
			base.ExecuteForPlayer();

			gEngine.RestoreGame = true;
		}
	}
}
