
// AfterPlayerMoveState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static Firestorm.Game.Plugin.Globals;

namespace Firestorm.Game.States
{
	[ClassMappings]
	public class AfterPlayerMoveState : EamonRT.Game.States.AfterPlayerMoveState, IAfterPlayerMoveState
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			Debug.Assert(gGameState != null);

			Debug.Assert(gCharMonster != null);

			Debug.Assert(gCharRoom != null);

			var mystiqueMonster = gMDB[6];

			Debug.Assert(mystiqueMonster != null);

			var fliprootArtifact = gADB[10];

			Debug.Assert(fliprootArtifact != null);

			if (eventType == EventType.AfterExtinguishLightSourceCheck)
			{
				// Reset guard check flag

				if (!gEngine.RestoreGame)
				{
					gGameState.NF = 0;
				}

				// Mystique communicates with entity

				if (gCharRoom.Uid == 27 && mystiqueMonster.IsInRoom(gCharRoom) && fliprootArtifact.IsCarriedByMonster(mystiqueMonster) && !gEngine.RestoreGame)
				{
					if (gGameState.MY == 1)
					{
						gEngine.PrintEffectDesc(13);

						gGameState.MY = 2;
					}
					else if (gGameState.MY == 2)
					{
						gEngine.PrintEffectDesc(14);

						gGameState.Die = 1;

						NextState = gEngine.CreateInstance<IPlayerDeadState>(x =>
						{
							x.PrintLineSep = true;
						});

						GotoCleanup = true;

						goto Cleanup;
					}
				}

				// Sync random fortress guards with player room

				var monsterList = gEngine.GetMonsterList(m => m.Uid >= 23 && m.Uid <= 26 && !m.IsInLimbo());

				foreach (var monster in monsterList)
				{
					monster.SetInRoom(gCharRoom);
				}
			}

		Cleanup:

			;
		}

		public override void Execute()
		{
			base.Execute();

			gEngine.RestoreGame = false;
		}
	}
}
