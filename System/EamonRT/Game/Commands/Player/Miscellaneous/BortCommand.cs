
// BortCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class BortCommand : Command, IBortCommand
	{
		public override void Execute()
		{


			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IStartState>();
			}
		}

		public BortCommand()
		{
			SortOrder = 435;

			IsSentenceParserEnabled = false;

			IsDarkEnabled = true;

#if !DEBUG
			IsPlayerEnabled = false;
#endif

			Uid = 99;

			Name = "BortCommand";

			Verb = "bort";

			Type = CommandType.Miscellaneous;
		}
	}
}
