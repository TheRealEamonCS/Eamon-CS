
// WaitCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static TheDeepCanyon.Game.Plugin.Globals;

namespace TheDeepCanyon.Game.Commands
{
	[ClassMappings]
	public class WaitCommand : EamonRT.Game.Commands.Command, Framework.Commands.IWaitCommand
	{
		public override void Execute()
		{
			PrintOkay();

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
		}

		public WaitCommand()
		{
			SortOrder = 460;

			IsNew = true;

			Name = "WaitCommand";

			Verb = "wait";

			Type = CommandType.Miscellaneous;
		}
	}
}
