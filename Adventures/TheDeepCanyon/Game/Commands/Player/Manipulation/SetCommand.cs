
// SetCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static TheDeepCanyon.Game.Plugin.PluginContext;

namespace TheDeepCanyon.Game.Commands
{
	[ClassMappings]
	public class SetCommand : EamonRT.Game.Commands.Command, Framework.Commands.ISetCommand
	{
		public override void Execute()
		{

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		public SetCommand()
		{
			SortOrder = 245;

			IsNew = true;

			Uid = 96;

			Name = "SetCommand";

			Verb = "set";

			Type = CommandType.Manipulation;
		}
	}
}
