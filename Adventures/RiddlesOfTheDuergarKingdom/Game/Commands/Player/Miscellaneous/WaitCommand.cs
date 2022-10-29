
// WaitCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static RiddlesOfTheDuergarKingdom.Game.Plugin.Globals;

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
				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
		}

		public WaitCommand()
		{
			SortOrder = 450;

			IsNew = true;

			IsDarkEnabled = true;

			Name = "WaitCommand";

			Verb = "wait";

			Type = CommandType.Miscellaneous;
		}
	}
}
