
// CommandImpl.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game.Commands
{
	[ClassMappings]
	public class CommandImpl : EamonRT.Game.Commands.CommandImpl, ICommandImpl
	{
		public override void Stage()
		{
			base.Stage();

			if (Globals.MonsterCurseFunc != null)
			{
				if (gGameState.Die <= 0)
				{
					Globals.MonsterCurseFunc();
				}

				Globals.MonsterCurseFunc = null;
			}
		}
	}
}
