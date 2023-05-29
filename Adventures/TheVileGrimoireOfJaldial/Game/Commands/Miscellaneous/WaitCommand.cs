
// WaitCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static TheVileGrimoireOfJaldial.Game.Plugin.Globals;

namespace TheVileGrimoireOfJaldial.Game.Commands
{
	[ClassMappings]
	public class WaitCommand : EamonRT.Game.Commands.Command, Framework.Commands.IWaitCommand
	{
		public virtual long Minutes { get; set; }

		public override void ExecuteForPlayer()
		{
			Debug.Assert(Minutes >= 0 && Minutes <= 55);

			gOut.Print("Time passes.");

			gGameState.Minute += Minutes;

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
		}

		public WaitCommand()
		{
			SortOrder = 460;

			IsNew = true;

			IsDarkEnabled = true;

			Name = "WaitCommand";

			Verb = "wait";

			Type = CommandType.Miscellaneous;

			Minutes = 15;
		}
	}
}
