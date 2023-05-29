
// MagicComponent.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Linq;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Components;
using EamonRT.Framework.Primitive.Enums;
using static StrongholdOfKahrDur.Game.Plugin.Globals;

namespace StrongholdOfKahrDur.Game.Components
{
	[ClassMappings]
	public class MagicComponent : EamonRT.Game.Components.MagicComponent, IMagicComponent
	{
		public override void PrintFortuneCookie()
		{
			gOut.Print("The air crackles with magical energy but nothing interesting happens.");
		}

		public override void PlayerSpellCastBrainOverload(Spell s, ISpell spell)
		{
			Debug.Assert(Enum.IsDefined(typeof(Spell), s));

			Debug.Assert(spell != null);

			gOut.Print("Spell backlash!  Your ability to cast {0} temporarily diminishes!", spell.Name);

			if (gGameState.GetSa(s) > 10)
			{
				gGameState.SetSa(s, 10);
			}
		}

		public override bool ShouldShowBlastSpellAttack()
		{
			var artUids = new long[] { 3, 15 };

			return (DobjMonster != null || !artUids.Contains(DobjArtifact.Uid)) && base.ShouldShowBlastSpellAttack();
		}

		public override void CheckAfterAggravateMonster()
		{
			var helmArtifact = gADB[25];

			Debug.Assert(helmArtifact != null);

			// Necromancer cannot be blasted unless wearing Wizard's Helm

			if (DobjMonster != null && DobjMonster.Uid == 22 && !helmArtifact.IsWornByMonster(ActorMonster))
			{
				var rl = gEngine.RollDice(1, 4, 56);

				gEngine.PrintEffectDesc(rl);

				gEngine.PauseCombat();

				MagicState = MagicState.EndMagic;

				goto Cleanup;
			}

			base.CheckAfterAggravateMonster();

		Cleanup:

			;
		}

		public override void CheckAfterCastPower()
		{
			var cauldronArtifact = gADB[24];

			Debug.Assert(cauldronArtifact != null);

			// If the cauldron is prepared (see Effect #50) and the magic words have been spoken, unlock the portcullis

			if (ActorRoom.Uid == 43 && gGameState.UsedCauldron && (cauldronArtifact.IsCarriedByMonster(ActorMonster) || cauldronArtifact.IsInRoom(ActorRoom)) && gEngine.SpellReagentsInCauldron(cauldronArtifact))
			{
				gEngine.PrintEffectDesc(52);

				// Unlock portcullis and destroy the cauldron

				gGameState.UsedCauldron = false;

				var eastPortcullisArtifact = gADB[7];

				Debug.Assert(eastPortcullisArtifact != null);

				var ac = eastPortcullisArtifact.DoorGate;

				Debug.Assert(ac != null);

				ac.SetOpen(true);

				var westPortcullisArtifact = gADB[8];

				Debug.Assert(westPortcullisArtifact != null);

				ac = westPortcullisArtifact.DoorGate;

				Debug.Assert(ac != null);

				ac.SetOpen(true);

				cauldronArtifact.SetInLimbo();

				gOut.Print("The cauldron disintegrates!");

				MagicState = MagicState.EndMagic;

				goto Cleanup;
			}

			// Move companions into pit

			if (ActorRoom.Uid > 93 && ActorRoom.Uid < 110)
			{
				var monsterList = gEngine.GetMonsterList(m => !m.IsCharacterMonster() && m.Reaction == Friendliness.Friend && m.Seen && (m.Location < 94 || m.Location > 109));

				if (monsterList.Count > 0)
				{
					gEngine.PrintEffectDesc(49);

					foreach (var m in monsterList)
					{
						gOut.Print("{0} suddenly appears!", m.GetTheName(true));

						m.SetInRoom(ActorRoom);
					}

					MagicState = MagicState.EndMagic;

					goto Cleanup;
				}
			}

			// Move companions out of pit

			if (ActorRoom.Uid < 94 || ActorRoom.Uid > 109)
			{
				var monsterList = gEngine.GetMonsterList(m => !m.IsCharacterMonster() && m.Reaction == Friendliness.Friend && m.Seen && (m.Location > 93 && m.Location < 110));

				if (monsterList.Count > 0)
				{
					gEngine.PrintEffectDesc(49);

					foreach (var m in monsterList)
					{
						gOut.Print("{0} suddenly appears!", m.GetTheName(true));

						m.SetInRoom(ActorRoom);
					}

					MagicState = MagicState.EndMagic;

					goto Cleanup;
				}
			}

			base.CheckAfterCastPower();

		Cleanup:

			;
		}
	}
}
