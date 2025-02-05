
// CombatComponent.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Components;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static OrbOfMyLife.Game.Plugin.Globals;

namespace OrbOfMyLife.Game.Components
{
	[ClassMappings]
	public class CombatComponent : EamonRT.Game.Components.CombatComponent, ICombatComponent
	{
		public override void RollToHitOrMiss()
		{
			base.RollToHitOrMiss();

			// Always hit unconscious Darrk Ness (and omit skill gains)

			if (DobjMonster?.Uid == 1 && DobjMonster.StateDesc.Length > 0)
			{
				if (_rl < 97 && _rl > _odds)
				{
					_rl = _odds;
				}

				OmitSkillGains = true;
			}
		}

		public override void CalculateDamage()
		{
			Debug.Assert(DobjMonster != null);

			base.CalculateDamage();

			// Sword of Light damages demons

			if (ActorMonster != null && ActorMonster.IsCharacterMonster() && ActorWeaponUid == 6 && DobjMonster.Uid > 8 && DobjMonster.Uid < 13)
			{
				_d2 += 2;
			}
		}

		public override void CheckMonsterStatus()
		{
			Debug.Assert(SetNextStateFunc != null);

			Debug.Assert(DobjMonster != null);

			Debug.Assert(_d2 >= 0);

			DobjMonster.DmgTaken += _d2;

			if (!OmitMonsterStatus || ActorMonster == DobjMonster)
			{
				PrintHealthStatus(ActorRoom, ActorMonster, DobjMonster, BlastSpell, NonCombat);
			}

			if (gGameState.SCR <= 0)
			{
				if ((DobjMonster.Uid > 8 && DobjMonster.Uid < 13) || (DobjMonster.Uid == 1 && DobjMonster.StateDesc.Length <= 0))
				{
					var monster = gMDB[25];

					Debug.Assert(monster != null);

					var deadBodyArtifact = gADB[monster.DeadBody];

					Debug.Assert(deadBodyArtifact != null);

					var index = gEngine.GetMonsterHealthStatusIndex(DobjMonster.Hardiness, DobjMonster.DmgTaken);

					if ((index == 4 || index == 5) && !monster.IsInRoom(ActorRoom))     // TODO: should this be IsInLimbo() to match other ???
					{
						deadBodyArtifact.SetInLimbo();

						gOut.WriteLine();

						gOut.Print("{0} waves {1} hand and ...", DobjMonster.GetTheName(true), DobjMonster.EvalGender("his", "her", "its"));

						gEngine.PrintEffectDesc(27);

						gEngine.BuildRandomMonster(ActorRoom, monster, deadBodyArtifact);
					}
				}
			}

			if (DobjMonster.IsDead())
			{
				if (DobjMonster.IsCharacterMonster())
				{
					gGameState.Die = 1;

					SetNextStateFunc(gEngine.CreateInstance<IPlayerDeadState>(x =>
					{
						x.PrintLineSep = true;
					}));
				}
				else
				{
					gEngine.MonsterDies(ActorMonster, DobjMonster);
				}
			}

			CombatState = CombatState.EndAttack;
		}
	}
}
