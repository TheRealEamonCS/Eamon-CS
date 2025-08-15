
// CastCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static Firestorm.Game.Plugin.Globals;

namespace Firestorm.Game.Commands
{
	[ClassMappings]
	public class CastCommand : EamonRT.Game.Commands.Command, Framework.Commands.ICastCommand
	{
		public virtual string SpellName { get; set; }

		public override void ExecuteForPlayer()
		{
			Debug.Assert(!string.IsNullOrWhiteSpace(SpellName));

			var buzzMonster = gMDB[45];

			Debug.Assert(buzzMonster != null);

			var secretGatewayArtifact = gADB[4];

			Debug.Assert(secretGatewayArtifact != null);

			var unmarkedPassageArtifact = gADB[5];

			Debug.Assert(unmarkedPassageArtifact != null);

			var secretDoorArtifact = gADB[11];

			Debug.Assert(secretDoorArtifact != null);

			var hiddenDoorInTheWallArtifact = gADB[12];

			Debug.Assert(hiddenDoorInTheWallArtifact != null);

			var hiddenDoorArtifact = gADB[13];

			Debug.Assert(hiddenDoorArtifact != null);

			var magicPlateMailArtifact = gADB[24];

			Debug.Assert(magicPlateMailArtifact != null);

			var teleportationCoinArtifact = gADB[53];

			Debug.Assert(teleportationCoinArtifact != null);

			var diamondShapedGemArtifact = gADB[55];

			Debug.Assert(diamondShapedGemArtifact != null);

			if (!gGameState.MPEnabled)
			{
				gOut.Print("No magic points.");

				NextState = gEngine.CreateInstance<IStartState>();

				goto Cleanup;
			}

			if (ActorRoom.Uid > 83)
			{
				gOut.Print("Your spells are nonfunctional.");

				NextState = gEngine.CreateInstance<IStartState>();

				goto Cleanup;
			}

			if (SpellName.Equals("healself", StringComparison.OrdinalIgnoreCase))
			{
				if (gGameState.MP < 5)
				{
					gOut.Print("Not enough magic points.");

					goto Cleanup;
				}

				gGameState.MP -= 5;

				if (gCharMonster.DmgTaken > 1)
				{
					gCharMonster.DmgTaken = 1;
				}

				gOut.Print("You are close to perfect health.");

				goto Cleanup;
			}

			if (SpellName.Equals("alibo", StringComparison.OrdinalIgnoreCase))
			{
				if (gGameState.MP < 100)
				{
					gOut.Print("Not enough magic points.");

					goto Cleanup;
				}

				if (ActorRoom.Uid != 48)
				{
					gOut.Print("That would be a real waste of magic.");

					NextState = gEngine.CreateInstance<IStartState>();

					goto Cleanup;
				}

				if (!teleportationCoinArtifact.IsCarriedByMonster(ActorMonster))
				{
					gOut.Print("You don't have the coin.");

					NextState = gEngine.CreateInstance<IStartState>();

					goto Cleanup;
				}

				if (!diamondShapedGemArtifact.IsCarriedByMonster(ActorMonster))
				{
					gOut.Print("You're still missing something!");

					NextState = gEngine.CreateInstance<IStartState>();

					goto Cleanup;
				}

				gGameState.MP -= 100;

				if (buzzMonster.IsInRoom(ActorRoom) && buzzMonster.Reaction == Friendliness.Friend)
				{
					gOut.Print("Buzz does not survive the teleportation.");

					buzzMonster.SetInLimbo();
				}

				gOut.Print("Your stomach twists...");

				gGameState.R2 = 50;

				gGameState.R3 = 48;

				NextState = gEngine.CreateInstance<IAfterPlayerMoveState>(x =>
				{
					x.MoveMonsters = false;
				});

				goto Cleanup;
			}

			if (SpellName.Equals("shake", StringComparison.OrdinalIgnoreCase))
			{
				var affected = false;

				if (gGameState.MP < 20)
				{
					gOut.Print("Not enough magic points.");

					goto Cleanup;
				}

				gGameState.MP -= 20;

				gEngine.PrintEffectDesc(23);

				if (ActorRoom.Uid == 45 && magicPlateMailArtifact.Location == -98)
				{
					gOut.Print("The armor falls to the ground.");

					magicPlateMailArtifact.SetInRoom(ActorRoom);

					affected = true;
				}

				var monsterList = gEngine.GetHostileMonsterList(ActorMonster);

				if (monsterList.Count > 0)
				{
					gOut.Print("Some of your enemies look like they may be a little bit shaken up.");

					gGameState.ST = gEngine.RollDice(1, 3, 1);

					affected = true;
				}

				if (!affected)
				{
					gOut.Print("Nothing is affected.");
				}

				goto Cleanup;
			}

			if (SpellName.Equals("reveal", StringComparison.OrdinalIgnoreCase))
			{
				if (gGameState.MP < 2)
				{
					gOut.Print("Not enough magic points.");

					goto Cleanup;
				}

				gGameState.MP -= 2;

				if (ActorRoom.Uid == 23 && !secretGatewayArtifact.IsInRoom(ActorRoom))
				{
					gOut.Print("Secret door detected.");

					secretGatewayArtifact.SetInRoom(ActorRoom);

					secretGatewayArtifact.Field4 = 0;

					goto Cleanup;
				}

				if (ActorRoom.Uid == 46 && !unmarkedPassageArtifact.IsInRoom(ActorRoom))
				{
					gOut.Print("Secret door detected.");

					unmarkedPassageArtifact.SetInRoom(ActorRoom);

					unmarkedPassageArtifact.Field4 = 0;

					goto Cleanup;
				}

				if (ActorRoom.Uid == 73 && !secretDoorArtifact.IsInRoom(ActorRoom))
				{
					gOut.Print("Secret door detected.");

					secretDoorArtifact.SetInRoom(ActorRoom);

					secretDoorArtifact.Field4 = 0;

					goto Cleanup;
				}

				if (ActorRoom.Uid == 77 && !hiddenDoorInTheWallArtifact.IsInRoom(ActorRoom))
				{
					gOut.Print("Secret door detected.");

					hiddenDoorInTheWallArtifact.SetInRoom(ActorRoom);

					hiddenDoorInTheWallArtifact.Field4 = 0;

					goto Cleanup;
				}

				if (ActorRoom.Uid == 82 && !hiddenDoorArtifact.IsInRoom(ActorRoom))
				{
					gOut.Print("Secret door detected.");

					hiddenDoorArtifact.SetInRoom(ActorRoom);

					hiddenDoorArtifact.Field4 = 0;

					goto Cleanup;
				}

				gOut.Print("No secret doors here.");

				goto Cleanup;
			}

			if (SpellName.Equals("spoof", StringComparison.OrdinalIgnoreCase))
			{
				if (gGameState.MP < 1)
				{
					gOut.Print("Not enough magic points.");

					goto Cleanup;
				}

				gGameState.MP -= 1;

				if (ActorRoom.Uid != 83)
				{
					gOut.Print("Nothing happens.");

					goto Cleanup;
				}

				gOut.Print("You black out for a few seconds...");

				gEngine.PrintEffectDesc(54);

				gGameState.R2 = 84;

				NextState = gEngine.CreateInstance<IAfterPlayerMoveState>(x =>
				{
					x.MoveMonsters = true;
				});

				goto Cleanup;
			}

			gOut.Print("You know no such spell.");

			NextState = gEngine.CreateInstance<IStartState>();

		Cleanup:

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
		}

		public CastCommand()
		{
			SortOrder = 470;

			IsNew = true;

			Name = "CastCommand";

			Verb = "cast";

			Type = CommandType.Miscellaneous;
		}
	}
}
