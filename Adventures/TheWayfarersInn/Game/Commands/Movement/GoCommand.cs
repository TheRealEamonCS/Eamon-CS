
// GoCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Components;
using EamonRT.Framework.States;
using static TheWayfarersInn.Game.Plugin.Globals;

namespace TheWayfarersInn.Game.Commands
{
	[ClassMappings]
	public class GoCommand : EamonRT.Game.Commands.GoCommand, IGoCommand
	{
		public override void ExecuteForPlayer()
		{
			Debug.Assert(DobjArtifact != null);

			// Loft

			if (DobjArtifact.Uid == 188)
			{
				if (!ActorMonster.CheckNBTLHostility())
				{
					if (gGameState.LadderUsed)
					{
						var ladderArtifact = gADB[112];

						Debug.Assert(ladderArtifact != null);

						// Redirect to ladder

						var command = gEngine.CreateInstance<IGoCommand>();

						CopyCommandData(command);

						command.Dobj = ladderArtifact;

						NextState = command;
					}
					else
					{
						gOut.Print("Sadly, it is well beyond your reach.");

						NextState = gEngine.CreateInstance<IStartState>();
					}
				}
				else
				{
					PrintEnemiesNearby();

					NextState = gEngine.CreateInstance<IStartState>();
				}
			}

			// Gorge / River

			else if (DobjArtifact.Uid == 145 || DobjArtifact.Uid == 146)
			{
				if (!ActorMonster.CheckNBTLHostility())
				{
					gEngine.PrintEffectDesc(3);

					gGameState.Die = 1;

					NextState = gEngine.CreateInstance<IPlayerDeadState>(x =>
					{
						x.PrintLineSep = true;
					});
				}
				else
				{
					PrintEnemiesNearby();

					NextState = gEngine.CreateInstance<IStartState>();
				}
			}

			// Courtyard

			else if (DobjArtifact.Uid == 97)
			{
				if (!ActorMonster.CheckNBTLHostility())
				{
					gEngine.PrintEffectDesc(124);

					var combatComponent = gEngine.CreateInstance<ICombatComponent>(x =>
					{
						x.SetNextStateFunc = s => NextState = s;

						x.ActorRoom = ActorRoom;

						x.Dobj = ActorMonster;

						x.OmitArmor = true;
					});

					combatComponent.ExecuteCalculateDamage(1, 1);

					if (gGameState.Die <= 0)
					{
						// TODO: injure companions as well ???

						gGameState.R2 = ActorRoom.Uid == 50 ? 30 : 31;

						NextState = gEngine.CreateInstance<IAfterPlayerMoveState>();
					}
				}
				else
				{
					PrintEnemiesNearby();

					NextState = gEngine.CreateInstance<IStartState>();
				}
			}

			// Water well

			else if (DobjArtifact.Uid == 17)
			{
				if (!ActorMonster.CheckNBTLHostility())
				{
					gOut.Print("Well, well, well, aren't we feeling adventurous?");
				}
				else
				{
					PrintEnemiesNearby();
				}

				NextState = gEngine.CreateInstance<IStartState>();
			}
			else
			{
				base.ExecuteForPlayer();
			}
		}
	}
}
