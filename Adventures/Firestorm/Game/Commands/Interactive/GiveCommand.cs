
// GiveCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using System.Linq;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static Firestorm.Game.Plugin.Globals;

namespace Firestorm.Game.Commands
{
	[ClassMappings]
	public class GiveCommand : EamonRT.Game.Commands.GiveCommand, IGiveCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			var qualinArtifacts = new long[] { 20, 22, 24 };

			base.ProcessEvents(eventType);

			var beerArtifact = gADB[22];

			Debug.Assert(beerArtifact != null);

			var uniformArtifact = gADB[33];

			Debug.Assert(uniformArtifact != null);

			var rockPileArtifact = gADB[34];

			Debug.Assert(rockPileArtifact != null);

			var scatteredRockPileArtifact = gADB[35];

			Debug.Assert(scatteredRockPileArtifact != null);

			var diamondShapedGemArtifact = gADB[55];

			Debug.Assert(diamondShapedGemArtifact != null);

			var ornateBrassKeyArtifact = gADB[60];

			Debug.Assert(ornateBrassKeyArtifact != null);

			var buzzSwordArtifact = gADB[66];

			Debug.Assert(buzzSwordArtifact != null);

			if (eventType == EventType.AfterEnforceMonsterWeightLimitsCheck)
			{
				// Give Thorak Junior the broad-tipped arrows 

				if (IobjMonster.Uid == 39 && DobjArtifact.Uid == 61)
				{
					DobjArtifact.SetInLimbo();

					PrintGiveObjToActor(DobjArtifact, IobjMonster);

					gEngine.PrintEffectDesc(58);

					if (ActorMonster.CanCarryArtifactWeight(ornateBrassKeyArtifact))
					{
						ornateBrassKeyArtifact.SetCarriedByMonster(ActorMonster);
					}
					else
					{
						ornateBrassKeyArtifact.SetInRoom(ActorRoom);
					}

					IobjMonster.Reaction = Friendliness.Enemy;

					IobjMonster.Friendliness = IobjMonster.Reaction;

					GotoCleanup = true;
				}

				// Give Zephette the fertilizer

				else if (IobjMonster.Uid == 4 && DobjArtifact.Uid == 14)
				{
					DobjArtifact.SetCarriedByMonster(IobjMonster);

					PrintGiveObjToActor(DobjArtifact, IobjMonster);

					gEngine.PrintEffectDesc(34);

					rockPileArtifact.SetInRoomUid(3);

					scatteredRockPileArtifact.SetInLimbo();

					var hint6 = gEngine.HDB[6];

					Debug.Assert(hint6 != null);

					hint6.Active = true;

					GotoCleanup = true;
				}

				// Give Mystique the fliproot

				else if (IobjMonster.Uid == 6 && DobjArtifact.Uid == 10)
				{
					DobjArtifact.SetCarriedByMonster(IobjMonster);

					PrintGiveObjToActor(DobjArtifact, IobjMonster);

					gEngine.PrintEffectDesc(35);

					gOut.Print("Take this gift.");

					diamondShapedGemArtifact.SetInRoom(ActorRoom);

					gEngine.PrintEffectDesc(12);

					gGameState.MY = 1;

					GotoCleanup = true;
				}

				// Give Qualin anything before magic enabled

				else if (IobjMonster.Uid == 10 && !gGameState.MPEnabled)
				{
					gOut.Print("You can't get magic yet.");

					NextState = gEngine.CreateInstance<IStartState>();

					GotoCleanup = true;
				}

				// Give Qualin the beer / magician's robe / magic plate mail

				else if (IobjMonster.Uid == 10 && qualinArtifacts.Contains(DobjArtifact.Uid))
				{
					DobjArtifact.SetInLimbo();

					PrintGiveObjToActor(DobjArtifact, IobjMonster);

					gEngine.PrintEffectDesc(36);

					gGameState.MP += (DobjArtifact.Uid == 22 ? 10 : DobjArtifact.Uid == 20 ? 20 : 50);

					GotoCleanup = true;
				}

				// Give Bryon the token

				else if (IobjMonster.Uid == 13 && DobjArtifact.Uid == 56)
				{
					DobjArtifact.SetInLimbo();

					PrintGiveObjToActor(DobjArtifact, IobjMonster);

					gEngine.PrintEffectDesc(16);

					uniformArtifact.SetInRoom(ActorRoom);

					GotoCleanup = true;
				}

				// Give Buzz the buzz-sword
				
				else if (IobjMonster.Uid == 45 && DobjArtifact.Uid == 66)
				{
					DobjArtifact.SetCarriedByMonster(IobjMonster);

					PrintGiveObjToActor(DobjArtifact, IobjMonster);

					gOut.Print("Buzz thanks you.");

					IobjMonster.Reaction++;

					IobjMonster.Friendliness = IobjMonster.Reaction;

					IobjMonster.Weapon = -1;

					GotoCleanup = true;
				}

				// Give Buzz a weapon

				else if (IobjMonster.Uid == 45 && DobjArtifact.GeneralWeapon != null && !buzzSwordArtifact.IsInLimbo() && !buzzSwordArtifact.IsCarriedByMonster(IobjMonster))
				{
					gOut.Print("He doesn't want that weapon.");

					GotoCleanup = true;
				}

				// Further disable bribing

				else if (!gEngine.IsRulesetVersion(5, 62) && (IobjMonster.Reaction == Friendliness.Enemy || (IobjMonster.Reaction == Friendliness.Neutral && DobjArtifact.Value < 3000)))
				{
					gEngine.PrintMonsterEmotes(IobjMonster);

					gOut.WriteLine();

					GotoCleanup = true;
				}
			}
			else if (eventType == EventType.BeforeTakePlayerGold)
			{
				// Disable bribing

				if (IobjMonster.Reaction < Friendliness.Friend)
				{
					gEngine.PrintMonsterEmotes(IobjMonster);

					gOut.WriteLine();

					GotoCleanup = true;
				}
			}
		}
	}
}
