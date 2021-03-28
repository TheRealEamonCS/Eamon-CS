
// GiveCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using static TheBeginnersCave.Game.Plugin.PluginContext;

namespace TheBeginnersCave.Game.Commands
{
	[ClassMappings]
	public class GiveCommand : EamonRT.Game.Commands.GiveCommand, IGiveCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			if (eventType == EventType.BeforeMonsterTakesGold && (IobjMonster.Uid == 1 || IobjMonster.Uid == 5 || IobjMonster.Uid == 7))
			{
				gEngine.MonsterEmotes(IobjMonster);

				gOut.WriteLine();

				GotoCleanup = true;
			}
			else
			{
				base.ProcessEvents(eventType);
			}
		}
	}
}
