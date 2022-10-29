
// GiveCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using static TheBeginnersCave.Game.Plugin.Globals;

namespace TheBeginnersCave.Game.Commands
{
	[ClassMappings]
	public class GiveCommand : EamonRT.Game.Commands.GiveCommand, IGiveCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.BeforeTakePlayerGold && (IobjMonster.Uid == 1 || IobjMonster.Uid == 5 || IobjMonster.Uid == 7))
			{
				gEngine.PrintMonsterEmotes(IobjMonster);

				gOut.WriteLine();

				GotoCleanup = true;
			}
		}
	}
}
