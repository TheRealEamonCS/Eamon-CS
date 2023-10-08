
// FleeCommand.cs

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
	public class FleeCommand : EamonRT.Game.Commands.FleeCommand, IFleeCommand
	{
		public override void PrintCantVerbHere()
		{
			var blueBandedCentipedesMonster = gMDB[3];

			Debug.Assert(blueBandedCentipedesMonster != null);

			var origCurrGroupCount = blueBandedCentipedesMonster.CurrGroupCount;

			blueBandedCentipedesMonster.CurrGroupCount = gGameState.AttackingCentipedeCounter;

			gOut.Print("You can't flee while swarmed by {0}!", blueBandedCentipedesMonster.GetArticleName());

			blueBandedCentipedesMonster.CurrGroupCount = origCurrGroupCount;
		}

		public override bool IsAllowedInRoom()
		{
			var blueBandedCentipedesMonster = gMDB[3];

			Debug.Assert(blueBandedCentipedesMonster != null);

			return !blueBandedCentipedesMonster.IsInRoom(ActorRoom) || gGameState.AttackingCentipedeCounter <= 0;
		}

		public override void ExecuteForPlayer()
		{
			// Loft

			if (DobjArtifact != null && DobjArtifact.Uid == 188)
			{
				if (ActorMonster.CheckNBTLHostility())
				{
					if (gGameState.LadderUsed)
					{
						var ladderArtifact = gADB[112];

						Debug.Assert(ladderArtifact != null);

						// Redirect to ladder

						var command = gEngine.CreateInstance<IFleeCommand>();

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
					PrintCalmDown();

					NextState = gEngine.CreateInstance<IStartState>();
				}
			}

			// Gorge / River

			else if (DobjArtifact != null && (DobjArtifact.Uid == 145 || DobjArtifact.Uid == 146))
			{
				if (ActorMonster.CheckNBTLHostility())
				{
					PrintAttemptingToFlee(DobjArtifact, Direction);

					gEngine.PrintEffectDesc(3);

					gGameState.Die = 1;

					NextState = gEngine.CreateInstance<IPlayerDeadState>(x =>
					{
						x.PrintLineSep = true;
					});
				}
				else
				{
					PrintCalmDown();

					NextState = gEngine.CreateInstance<IStartState>();
				}
			}

			// Courtyard

			else if (DobjArtifact != null && DobjArtifact.Uid == 97)
			{
				if (ActorMonster.CheckNBTLHostility())
				{
					PrintAttemptingToFlee(DobjArtifact, Direction);

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
					PrintCalmDown();

					NextState = gEngine.CreateInstance<IStartState>();
				}
			}

			// Water well

			else if (DobjArtifact != null && DobjArtifact.Uid == 17)
			{
				if (ActorMonster.CheckNBTLHostility())
				{
					gOut.Print("Well, well, well, aren't we feeling adventurous?");
				}
				else
				{
					PrintCalmDown();
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
