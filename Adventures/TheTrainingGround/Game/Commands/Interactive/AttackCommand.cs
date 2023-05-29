
// AttackCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static TheTrainingGround.Game.Plugin.Globals;

namespace TheTrainingGround.Game.Commands
{
	[ClassMappings]
	public class AttackCommand : EamonRT.Game.Commands.AttackCommand, IAttackCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			// Attack Bozworth

			if (eventType == EventType.AfterAttackNonEnemyCheck && DobjMonster != null && DobjMonster.Uid == 20)
			{
				gEngine.PrintEffectDesc(20);

				DobjMonster.SetInLimbo();

				NextState = gEngine.CreateInstance<IStartState>();

				GotoCleanup = true;
			}
		}

		public override void ExecuteForPlayer()
		{
			Debug.Assert(DobjArtifact != null || DobjMonster != null);

			// Attack backpack

			if (ActorMonster.Weapon > 0 && DobjArtifact != null && DobjArtifact.Uid == 13)
			{
				PrintDontNeedTo();

				NextState = gEngine.CreateInstance<IStartState>();
			}
			else
			{
				base.ExecuteForPlayer();
			}
		}
	}
}
