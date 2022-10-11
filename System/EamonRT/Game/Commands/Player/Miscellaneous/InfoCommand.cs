﻿
// InfoCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class InfoCommand : Command, IInfoCommand
	{
		public override void Execute()
		{
			Globals.ShouldPreTurnProcess = false;
		
			Globals.Module.PrintInfo();

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IStartState>();
			}
		}

		public InfoCommand()
		{
			SortOrder = 380;

			IsSentenceParserEnabled = false;

			IsDarkEnabled = true;

			Name = "InfoCommand";

			Verb = "info";

			Type = CommandType.Miscellaneous;
		}
	}
}
