
// GiveCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Linq;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using static TheVileGrimoireOfJaldial.Game.Plugin.PluginContext;

namespace TheVileGrimoireOfJaldial.Game.Commands
{
	[ClassMappings]
	public class GiveCommand : EamonRT.Game.Commands.GiveCommand, IGiveCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			if (eventType == EventType.BeforeTakePlayerGold)
			{
				var monsterUids = new long[] { 46, 47, 48, 49, 50 };

				// Disable bribing

				if (!monsterUids.Contains(IobjMonster.Uid))
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
			else
			{
				base.ProcessEvents(eventType);
			}
		}
	}
}
