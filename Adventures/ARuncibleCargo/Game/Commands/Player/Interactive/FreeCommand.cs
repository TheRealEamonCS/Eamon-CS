
// FreeCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static ARuncibleCargo.Game.Plugin.PluginContext;

namespace ARuncibleCargo.Game.Commands
{
	[ClassMappings]
	public class FreeCommand : EamonRT.Game.Commands.FreeCommand, IFreeCommand
	{
		public override void PrintFreeActorWithKey(IMonster monster, IArtifact key)
		{
			Debug.Assert(monster != null);

			// Swarmy

			if (monster.Uid == 31)
			{
				PrintFullDesc(monster, false);

				monster.Seen = true;

				gEngine.PrintEffectDesc(64);
			}
			else
			{
				base.PrintFreeActorWithKey(monster, key);
			}
		}
	}
}
