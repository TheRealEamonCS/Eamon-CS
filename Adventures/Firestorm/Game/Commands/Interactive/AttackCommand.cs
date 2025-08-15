
// AttackCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static Firestorm.Game.Plugin.Globals;

namespace Firestorm.Game.Commands
{
	[ClassMappings]
	public class AttackCommand : EamonRT.Game.Commands.AttackCommand, IAttackCommand
	{
		public override void ExecuteForPlayer()
		{
			Debug.Assert(DobjArtifact != null || DobjMonster != null);

			var teleportationCoinArtifact = gADB[53];

			Debug.Assert(teleportationCoinArtifact != null);

			if ((BlastSpell || ActorMonster.Weapon > 0) && DobjMonster != null)
			{
				// Ghost

				if (DobjMonster.Uid == 29)
				{
					if (ActorMonster.Weapon != 17)
					{
						gOut.Print("Your weapon passes through the ghost, doing no damage.");

						NextState = gEngine.CreateInstance<IMonsterStartState>();
					}
					else if (gGameState.GH == 0)
					{
						gOut.Print("You have no idea how to kill it!");

						NextState = gEngine.CreateInstance<IStartState>();
					}
					else
					{
						gOut.Print("The ghost disappears. You see something on the ground in its place.");

						DobjMonster.SetInLimbo();

						teleportationCoinArtifact.SetInRoom(ActorRoom);

						NextState = gEngine.CreateInstance<IMonsterStartState>();
					}
				}

				// Thorak Junior

				else if (DobjMonster.Uid == 39 && DobjMonster.Reaction == Friendliness.Neutral)
				{
					gOut.Print("You can't do that yet.");

					NextState = gEngine.CreateInstance<IStartState>();
				}
				else
				{
					base.ExecuteForPlayer();
				}
			}
			else
			{
				base.ExecuteForPlayer();
			}
		}

		public override void ExecuteForMonster()
		{
			Debug.Assert(ActorMonster != null && DobjMonster != null);

			var rl = gEngine.RollDice(1, 100, 0);

			// Stunned opponent

			if (ActorMonster.Reaction == Friendliness.Enemy && gGameState.ST > 0)
			{
				gOut.Print("One opponent is stunned and does not attack {0}.", DobjMonster.IsCharacterMonster() ? "you" : ActorRoom.EvalViewability("an unseen defender", DobjMonster.GetTheName()));
			}

			// Snake poisons player

			else if (DobjMonster.IsCharacterMonster() && gGameState.PZ == 0 && (ActorMonster.Uid == 41 || ActorMonster.Uid == 42) && rl >= 80)
			{
				gOut.Print("The snake bites you! You are poisoned. If you do not find the antidote soon, you will die.");

				gGameState.PZ = 1;
			}
			else
			{
				base.ExecuteForMonster();
			}
		}
	}
}
