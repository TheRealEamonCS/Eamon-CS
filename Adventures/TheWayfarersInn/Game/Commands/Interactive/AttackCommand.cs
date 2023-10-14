
// AttackCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static TheWayfarersInn.Game.Plugin.Globals;

namespace TheWayfarersInn.Game.Commands
{
	[ClassMappings]
	public class AttackCommand : EamonRT.Game.Commands.AttackCommand, IAttackCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			// Attack unseen apparition

			if (eventType == EventType.AfterAttackNonEnemyCheck && DobjMonster != null && DobjMonster.Uid == 2)
			{
				gEngine.PrintEffectDesc(23);

				gGameState.Die = 1;

				NextState = gEngine.CreateInstance<IPlayerDeadState>(x =>
				{
					x.PrintLineSep = true;
				});

				GotoCleanup = true;
			}
		}
	}
}
