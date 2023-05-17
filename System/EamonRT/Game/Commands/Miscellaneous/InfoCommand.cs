
// InfoCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.Globals;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class InfoCommand : Command, IInfoCommand
	{
		public override void ExecuteForPlayer()
		{
			gEngine.ShouldPreTurnProcess = false;
		
			gEngine.Module.PrintInfo();

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IStartState>();
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
