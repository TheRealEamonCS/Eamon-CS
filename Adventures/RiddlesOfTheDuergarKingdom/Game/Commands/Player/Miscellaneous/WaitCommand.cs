
// WaitCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static RiddlesOfTheDuergarKingdom.Game.Plugin.PluginContext;

namespace RiddlesOfTheDuergarKingdom.Game.Commands
{
	[ClassMappings]
	public class WaitCommand : EamonRT.Game.Commands.Command, Framework.Commands.IWaitCommand
	{
		public override void Execute()
		{
			gOut.Print("Time passes.");

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		public WaitCommand()
		{
			SortOrder = 450;

			IsNew = true;

			IsDarkEnabled = true;

			Uid = 96;

			Name = "WaitCommand";

			Verb = "wait";

			Type = CommandType.Miscellaneous;
		}
	}
}
