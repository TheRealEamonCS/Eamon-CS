
// WaitCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game.Commands
{
	[ClassMappings]
	public class WaitCommand : EamonRT.Game.Commands.Command, Framework.Commands.IWaitCommand
	{
		public virtual long Minutes { get; set; }

		public override void Execute()
		{
			Debug.Assert(Minutes >= 0 && Minutes <= 55);

			gOut.Print("Time passes.");

			gGameState.Minute += Minutes;

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		public WaitCommand()
		{
			SortOrder = 460;

			IsNew = true;

			IsDarkEnabled = true;

			Uid = 91;

			Name = "WaitCommand";

			Verb = "wait";

			Type = CommandType.Miscellaneous;

			Minutes = 15;
		}
	}
}
