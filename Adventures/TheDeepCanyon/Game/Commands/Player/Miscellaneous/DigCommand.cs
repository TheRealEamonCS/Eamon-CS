
// DigCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static TheDeepCanyon.Game.Plugin.PluginContext;

namespace TheDeepCanyon.Game.Commands
{
	[ClassMappings]
	public class DigCommand : EamonRT.Game.Commands.Command, Framework.Commands.IDigCommand
	{
		public override void Execute()
		{

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		public DigCommand()
		{
			SortOrder = 450;

			IsNew = true;

			Uid = 97;

			Name = "DigCommand";

			Verb = "dig";

			Type = CommandType.Miscellaneous;
		}
	}
}
