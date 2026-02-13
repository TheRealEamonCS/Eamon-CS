
// CombatComponent.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Components;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.Globals;

namespace EamonRT.Game.Components
{
	[ClassMappings]
	public class CombatComponent : Component, ICombatComponent
	{
		/// <summary></summary>
		public long _odds = 0;

		/// <summary></summary>
		public long _rl = 0;

		/// <summary></summary>
		public long _d2 = 0;

		public virtual long MemberNumber { get; set; }

		public virtual long AttackNumber { get; set; }

		public virtual bool BlastSpell { get; set; }

		public virtual bool MaxDamage { get; set; }

		public virtual bool OmitArmor { get; set; }

		public virtual bool OmitMonsterStatus { get; set; }

		public virtual AttackResult FixedResult { get; set; }

		public virtual WeaponRevealType WeaponRevealType { get; set; }

		/// <summary></summary>
		public virtual CombatState CombatState { get; set; }

		/// <summary></summary>
		public virtual IList<IArtifact> WeaponList { get; set; }

		/// <summary></summary>
		public virtual IList<IArtifact> SpilledArtifactList { get; set; }

		/// <summary></summary>
		public virtual IMonster DisguisedMonster { get; set; }

		/// <summary></summary>
		public virtual IArtifactCategory SpilledArtifactContainerAc { get; set; }

		/// <summary></summary>
		public virtual IArtifactCategory ActorAc { get; set; }

		/// <summary></summary>
		public virtual IArtifactCategory DobjAc { get; set; }

		/// <summary></summary>
		public virtual IArtifact SpilledArtifactContainer { get; set; }

		/// <summary></summary>
		public virtual IArtifact ReadyWeapon { get; set; }

		/// <summary></summary>
		public virtual IArtifact ActorWeapon { get; set; }

		/// <summary></summary>
		public virtual IArtifact DobjWeapon { get; set; }

		/// <summary></summary>
		public virtual ContainerType SpilledArtifactContainerType { get; set; }

		/// <summary></summary>
		public virtual Weapon ActorWeaponType { get; set; }

		/// <summary></summary>
		public virtual Weapon DobjWeaponType { get; set; }

		/// <summary></summary>
		public virtual bool SpilledArtifactContainerSeen { get; set; }

		/// <summary></summary>
		public virtual bool SpillContents { get; set; }

		/// <summary></summary>
		public virtual bool UseFractionalStrength { get; set; }

		/// <summary></summary>
		public virtual bool OmitBboaPadding { get; set; }

		/// <summary></summary>
		public virtual bool LightOut { get; set; }

		/// <summary></summary>
		public virtual bool NonCombat { get; set; }

		/// <summary></summary>
		public virtual bool GotoCleanup { get; set; }

		/// <summary></summary>
		public virtual long WeaponCount { get; set; }

		/// <summary></summary>
		public virtual long KeyArtifactUid { get; set; }

		/// <summary></summary>
		public virtual long BreakageStrength { get; set; }

		/// <summary></summary>
		public virtual long ActorWeaponUid { get; set; }

		/// <summary></summary>
		public virtual long DobjWeaponUid { get; set; }

		/// <summary></summary>
		public virtual long D { get; set; }

		/// <summary></summary>
		public virtual long S { get; set; }

		/// <summary></summary>
		public virtual long M { get; set; }

		/// <summary></summary>
		public virtual long A { get; set; }

		/// <summary></summary>
		public virtual long Af { get; set; }

		/// <summary></summary>
		public virtual double S2 { get; set; }

		public virtual void ExecuteCalculateDamage(long numDice, long numSides, long mod = 0, bool nonCombat = true)
		{
			CombatState = CombatState.CalculateDamage;

			D = Math.Max(1, numDice);

			S = Math.Max(1, numSides);

			M = mod;

			NonCombat = nonCombat;
			
			A = BlastSpell || OmitArmor ? 0 : 1;

			OmitBboaPadding = true;

			ExecuteStateMachine();
		}

		public virtual void ExecuteCheckMonsterStatus()
		{
			CombatState = CombatState.CheckMonsterStatus;

			_d2 = 0;

			ExecuteStateMachine();
		}

		public virtual void ExecuteAttack()
		{
			Debug.Assert(DobjArtifact != null || DobjMonster != null);

			if (DobjMonster != null)
			{ 
				if (BlastSpell)
				{
					if (gEngine.IsRulesetVersion(5))
					{
						ExecuteCalculateDamage(1, 6, 0, false);
					}
					else
					{
						ExecuteCalculateDamage(2, 5, 0, false);
					}
				}
				else
				{
					CombatState = CombatState.BeginAttack;

					ExecuteStateMachine();
				}

				gEngine.PauseCombatAfterSkillGains = gEngine.SkillIncreaseFuncList.Count > 0;
				
				if (!gEngine.PauseCombatAfterSkillGains)
				{
					gEngine.PauseCombat();
				}
			}
			else
			{
				CombatState = CombatState.CheckDisguisedMonster;

				ExecuteStateMachine();
			}
		}

		/// <summary></summary>
		/// <param name="ac"></param>
		/// <param name="af"></param>
		public virtual void CheckPlayerCombatSkillGains(IArtifactCategory ac, long af)
		{
			Debug.Assert(ac != null && ac.IsWeapon01());

			var s = (Weapon)ac.Field2;

			var rl = gEngine.RollDice(1, 100, 0);

			if (rl > 75)
			{
				rl = gEngine.RollDice(1, 100, 0);

				rl += gCharacter.GetIntellectBonusPct();

				if (rl > gCharacter.GetWeaponAbility(s) && !gGameState.InteractiveFiction)
				{
					var weapon = gEngine.GetWeapon(s);

					Debug.Assert(weapon != null);

					gEngine.SkillIncreaseFuncList.Add(() =>
					{
						if (!gEngine.IsRulesetVersion(5, 62))
						{
							PrintWeaponAbilityIncreases(s, weapon);
						}

						gCharacter.ModWeaponAbility(s, 2);

						if (gCharacter.GetWeaponAbility(s) > weapon.MaxValue)
						{
							gCharacter.SetWeaponAbility(s, weapon.MaxValue);
						}
					});
				}

				var x = Math.Abs(af);

				if (x > 0)
				{
					rl = gEngine.RollDice(1, x, 0);

					rl += (long)Math.Round(((double)x / 100.0) * (double)gCharacter.GetIntellectBonusPct());

					if (rl > gCharacter.ArmorExpertise && !gGameState.InteractiveFiction)
					{
						gEngine.SkillIncreaseFuncList.Add(() =>
						{
							if (!gEngine.IsRulesetVersion(5, 62))
							{
								PrintArmorExpertiseIncreases();
							}

							gCharacter.ArmorExpertise += 2;

							if (gCharacter.ArmorExpertise <= 66 && gCharacter.ArmorExpertise > x)
							{
								gCharacter.ArmorExpertise = x;
							}

							if (gCharacter.ArmorExpertise > 79)
							{
								gCharacter.ArmorExpertise = 79;
							}
						});
					}
				}
			}
		}

		/// <summary></summary>
		public virtual void RollToHitOrMiss()
		{
			if (FixedResult != AttackResult.None)
			{
				_odds = 50;

				switch (FixedResult)
				{
					case AttackResult.Fumble:

						_rl = 100;

						break;

					case AttackResult.Miss:

						_rl = _odds + 1;

						break;

					case AttackResult.Hit:

						_rl = _odds;

						break;

					case AttackResult.CriticalHit:

						_rl = 1;

						break;
				}
			}
			else
			{
				_rl = gEngine.RollDice(1, 100, 0);
			}
		}

		/// <summary></summary>
		public virtual void BeginAttack()
		{
			Debug.Assert(ActorMonster != null && ActorMonster.CombatCode != CombatCode.NeverFights);

			Debug.Assert(DobjMonster != null);

			Debug.Assert(MemberNumber > 0);

			ActorWeaponUid = ActorMonster.Weapon;

			ReadyWeapon = ActorWeaponUid > 0 ? gADB[ActorWeaponUid] : null;

			if (ActorWeaponUid > 0 && ActorMonster.GroupCount == 1 && AttackNumber > 1 && ActorMonster.CanAttackWithMultipleWeapons())
			{
				WeaponList = ActorMonster.GetCarriedList().Where(x => x.IsReadyableByMonster(ActorMonster)).ToList();

				WeaponCount = WeaponList.Count;

				if ((AttackNumber - 1) % WeaponCount != 0)
				{
					ActorWeapon = gEngine.GetNthArtifact(WeaponList, (AttackNumber - 1) % WeaponCount, x => x.Uid != ActorWeaponUid);

					ActorWeaponUid = ActorWeapon.Uid;
				}
				else
				{
					ActorWeapon = gADB[ActorWeaponUid];
				}
			}
			else if (ActorWeaponUid > 0 && ActorMonster.CurrGroupCount > 1 && MemberNumber > 1)
			{
				ActorWeapon = gEngine.GetNthArtifact(ActorMonster.GetCarriedList(), MemberNumber - 1, x => x.IsReadyableByMonster(ActorMonster) && x.Uid != ActorWeaponUid);

				ActorWeaponUid = ActorWeapon != null ? ActorWeapon.Uid : ActorMonster.NwDice > 0 && ActorMonster.NwSides > 0 ? 0 : -1;

				if (ActorWeaponUid < 0)
				{
					CombatState = CombatState.EndAttack;

					goto Cleanup;
				}
			}
			else
			{
				ActorWeapon = ActorWeaponUid > 0 ? gADB[ActorWeaponUid] : null;
			}

			Debug.Assert(ActorWeaponUid == 0 || (ReadyWeapon != null && ReadyWeapon.GeneralWeapon != null && ActorWeapon != null && ActorWeapon.GeneralWeapon != null));

			ActorAc = ActorWeapon != null ? ActorWeapon.GeneralWeapon : null;

			Af = gEngine.GetArmorFactor(gGameState.Ar, gGameState.Sh);

			gEngine.GetOddsToHit(ActorMonster, DobjMonster, ActorAc, Af, ref _odds);

			RollToHitOrMiss();

			if (gGameState.InteractiveFiction)
			{
				_odds = 50;

				if (ActorMonster.IsCharacterMonster() || ActorMonster.Reaction == Friendliness.Friend)
				{
					if (_rl > _odds)
					{
						_rl = _odds;
					}
				}
				else
				{
					if (_rl <= _odds)
					{
						_rl = _odds + 1;
					}
				}
			}

			if (ActorMonster.IsCharacterMonster() && _rl < 97 && (_rl < 5 || _rl <= _odds) && ActorAc != null && !OmitSkillGains)
			{
				CheckPlayerCombatSkillGains(ActorAc, Af);
			}

			ActorWeaponType = (Weapon)(ActorAc != null ? ActorAc.Field2 : 0);

			PrintAttack(ActorRoom, ActorMonster, DobjMonster, ActorWeapon, AttackNumber, WeaponRevealType);

			if (_rl < 97 && (_rl < 5 || _rl <= _odds))
			{
				CombatState = CombatState.AttackHit;
			}
			else
			{
				CombatState = CombatState.AttackMiss;
			}

		Cleanup:

			;
		}

		/// <summary></summary>
		public virtual void AttackMissMiss()
		{
			PrintMiss(DobjMonster, DobjWeapon);

			CombatState = CombatState.EndAttack;

			GotoCleanup = true;
		}

		/// <summary></summary>
		public virtual void AttackMiss()
		{
			GotoCleanup = false;

			DobjWeaponUid = DobjMonster.Weapon;

			DobjWeapon = DobjWeaponUid > 0 ? gADB[DobjWeaponUid] : null;

			DobjAc = DobjWeapon != null ? DobjWeapon.GeneralWeapon : null;

			DobjWeaponType = (Weapon)(DobjAc != null ? DobjAc.Field2 : 0);

			if (_rl < 97 || ActorWeaponUid == 0)
			{
				AttackMissMiss();

				if (GotoCleanup)
				{
					goto Cleanup;
				}
			}

			CombatState = CombatState.AttackFumble;

		Cleanup:

			;
		}

		/// <summary></summary>
		public virtual void AttackFumbleRecovered()
		{
			PrintRecovered();

			CombatState = CombatState.EndAttack;

			GotoCleanup = true;
		}

		/// <summary></summary>
		public virtual void AttackFumbleWeaponDropped()
		{
			RetCode rc;

			if (gGameState.Ls > 0 && gGameState.Ls == ActorWeaponUid)
			{
				LightOut = true;
			}

			ActorWeapon.SetInRoom(ActorRoom);

			rc = ReadyWeapon.RemoveStateDesc(ReadyWeapon.GetReadyWeaponDesc());

			Debug.Assert(gEngine.IsSuccess(rc));

			ActorMonster.Weapon = !ActorMonster.IsCharacterMonster() ? -ActorWeaponUid - 1 : -1;

			PrintWeaponDropped(ActorRoom, ActorMonster, ActorWeapon, WeaponRevealType);

			CombatState = CombatState.EndAttack;

			GotoCleanup = true;
		}

		/// <summary></summary>
		public virtual void AttackFumbleWeaponHitsUser()
		{
			PrintWeaponHitsUser();

			Dobj = ActorMonster;

			CombatState = CombatState.AttackHit;

			GotoCleanup = true;
		}

		/// <summary></summary>
		public virtual void AttackFumbleSparksFly()
		{
			PrintSparksFly(ActorRoom, ActorMonster, ActorWeapon, WeaponRevealType);

			CombatState = CombatState.EndAttack;

			GotoCleanup = true;
		}

		/// <summary></summary>
		public virtual void AttackFumbleWeaponDamaged()
		{
			ActorAc.Field4--;

			if (ActorAc.Field4 > 0)
			{
				PrintWeaponDamaged();

				CombatState = CombatState.EndAttack;

				GotoCleanup = true;
			}
		}

		/// <summary></summary>
		public virtual void AttackFumbleWeaponBroken()
		{
			RetCode rc;

			PrintWeaponBroken();

			if (gGameState.Ls > 0 && gGameState.Ls == ActorWeaponUid)
			{
				LightOut = true;
			}

			ActorWeapon.SetInLimbo();

			if (ReadyWeapon.Uid == ActorWeaponUid)
			{
				rc = ActorWeapon.RemoveStateDesc(ActorWeapon.GetReadyWeaponDesc());

				Debug.Assert(gEngine.IsSuccess(rc));

				ActorMonster.Weapon = -1;
			}

			_rl = gEngine.RollDice(1, 100, 0);

			if (_rl > 50 || ActorAc.Field4 <= 0)
			{
				if (ActorAc.Field4 <= 0)
				{
					ActorAc.Field4 = 1;
				}

				CombatState = CombatState.EndAttack;

				GotoCleanup = true;
			}
		}

		/// <summary></summary>
		public virtual void AttackFumbleBrokenWeaponHitsUser()
		{
			PrintBrokenWeaponHitsUser();

			Dobj = ActorMonster;

			_rl = gEngine.RollDice(1, 5, 95);

			CombatState = CombatState.AttackHit;

			GotoCleanup = true;
		}

		/// <summary></summary>
		public virtual void AttackFumble()
		{
			GotoCleanup = false;

			PrintFumble();

			_rl = gEngine.RollDice(1, 100, 0);

			if ((gEngine.IsRulesetVersion(5, 62) && _rl < 36) || (!gEngine.IsRulesetVersion(5, 62) && _rl < 41))
			{
				AttackFumbleRecovered();

				if (GotoCleanup)
				{
					goto Cleanup;
				}
			}

			if ((gEngine.IsRulesetVersion(5, 62) && _rl < 76) || (!gEngine.IsRulesetVersion(5, 62) && _rl < 81))
			{
				AttackFumbleWeaponDropped();

				if (GotoCleanup)
				{
					goto Cleanup;
				}
			}

			if (_rl > 95)
			{
				AttackFumbleWeaponHitsUser();

				if (GotoCleanup)
				{
					goto Cleanup;
				}
			}

			if (ActorAc.Type == ArtifactType.MagicWeapon)
			{
				AttackFumbleSparksFly();

				if (GotoCleanup)
				{
					goto Cleanup;
				}
			}

			if (_rl < 91)
			{
				AttackFumbleWeaponDamaged();

				if (GotoCleanup)
				{
					goto Cleanup;
				}
			}

			AttackFumbleWeaponBroken();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			AttackFumbleBrokenWeaponHitsUser();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

		Cleanup:

			;
		}

		/// <summary></summary>
		public virtual void AttackHitHit()
		{
			PrintHit();

			CombatState = CombatState.CalculateDamage;

			GotoCleanup = true;
		}

		/// <summary></summary>
		public virtual void AttackHitCriticalHitKill()
		{
			_d2 = DobjMonster.Hardiness - DobjMonster.DmgTaken - (gEngine.IsRulesetVersion(5, 62) ? 0 : 2);

			CombatState = CombatState.CheckArmor;

			GotoCleanup = true;
		}

		/// <summary></summary>
		public virtual void AttackHitCriticalHitNoArmor()
		{
			A = 0;

			CombatState = CombatState.CalculateDamage;

			GotoCleanup = true;
		}

		/// <summary></summary>
		public virtual void AttackHitCriticalHit()
		{
			PrintCriticalHit();

			if (ActorMonster != DobjMonster || !gEngine.IsRulesetVersion(5, 62))
			{
				_rl = gEngine.RollDice(1, 100, 0);

				if (_rl == 100)
				{
					AttackHitCriticalHitKill();

					if (GotoCleanup)
					{
						goto Cleanup;
					}
				}

				if (_rl < (gEngine.IsRulesetVersion(5, 62) ? 51 : 50))
				{
					AttackHitCriticalHitNoArmor();

					if (GotoCleanup)
					{
						goto Cleanup;
					}
				}

				if (_rl < 86 || !gEngine.IsRulesetVersion(5, 62))
				{
					S2 = S;

					S2 *= (1 + .5 + (_rl > 85 ? 1 : 0) + (_rl > 95 ? 1 : 0));

					S = (long)Math.Round(S2);
				}
				else
				{
					D *= (1 + (_rl > 85 ? 1 : 0) + (_rl > 95 ? 1 : 0));
				}
			}
			else
			{
				D *= 2;

				A = 0;
			}

			CombatState = CombatState.CalculateDamage;

			GotoCleanup = true;

		Cleanup:

			;
		}

		/// <summary></summary>
		public virtual void AttackHit()
		{
			GotoCleanup = false;

			if (ActorAc != null)
			{
				D = Math.Max(1, ActorAc.Field3);

				S = Math.Max(1, ActorAc.Field4);
			}
			else
			{
				D = Math.Max(1, ActorMonster.NwDice);

				S = Math.Max(1, ActorMonster.NwSides);
			}

			A = OmitArmor ? 0 : 1;

			PrintStarPlus(DobjMonster);

			if ((ActorMonster != DobjMonster && _rl >= 5) || (ActorMonster == DobjMonster && _rl < 100))
			{
				AttackHitHit();

				if (GotoCleanup)
				{
					goto Cleanup;
				}
			}

			AttackHitCriticalHit();

			if (GotoCleanup)
			{
				goto Cleanup;
			}

		Cleanup:

			;
		}

		/// <summary></summary>
		public virtual void CalculateDamageForFractionalStrength()
		{
			Debug.Assert(ActorMonster != null && UseFractionalStrength);

			_d2 = 0;

			var xx = (double)(ActorMonster.Hardiness - ActorMonster.DmgTaken) / (double)ActorMonster.Hardiness;      // Fractional strength

			var yy = xx < .5 ? .5 + (xx * (.083 + (.833 * xx))) : (-.75) + (xx * (4.25 - (2.5 * xx)));

			if (yy > 1)
			{
				yy = 1;
			}

			for (var i = 0; i < D; i++)
			{
				_d2 += (long)Math.Round(yy * (MaxDamage ? S : gEngine.RollDice(1, S, 0)));
			}

			_d2 += (long)Math.Round(yy * M);
		}

		/// <summary></summary>
		public virtual void CalculateDamage()
		{
			Debug.Assert(ActorMonster != null || !UseFractionalStrength);

			Debug.Assert(DobjMonster != null);

			Debug.Assert(D > 0 && S > 0 && A >= 0 && A <= 1);

			if (UseFractionalStrength)
			{
				CalculateDamageForFractionalStrength();
			}
			else
			{
				_d2 = MaxDamage ? (D * S) + M : gEngine.RollDice(D, S, M);
			}

			_d2 -= (A * DobjMonster.Armor);

			if (gGameState.InteractiveFiction)
			{
				if (DobjMonster.IsCharacterMonster() || DobjMonster.Reaction == Friendliness.Friend)
				{
					_d2 = 0;
				}
				else
				{
					_d2 = DobjMonster.Hardiness - DobjMonster.DmgTaken;
				}
			}

			CombatState = CombatState.CheckArmor;
		}

		/// <summary></summary>
		public virtual void CheckArmorBlowTurned()
		{
			PrintBlowTurned(DobjMonster, OmitBboaPadding);

			CombatState = CombatState.EndAttack;

			GotoCleanup = true;
		}

		/// <summary></summary>
		public virtual void CheckArmor()
		{
			GotoCleanup = false;

			if (_d2 < 1)
			{
				CheckArmorBlowTurned();

				if (GotoCleanup)
				{
					goto Cleanup;
				}
			}

			CombatState = CombatState.CheckMonsterStatus;

		Cleanup:

			;
		}

		/// <summary></summary>
		public virtual void CheckMonsterStatus()
		{
			Debug.Assert(SetNextStateFunc != null);

			Debug.Assert(DobjMonster != null);

			Debug.Assert(_d2 >= 0);

			DobjMonster.DmgTaken += _d2;

			if (!OmitMonsterStatus || ActorMonster == DobjMonster)
			{
				PrintHealthStatus(ActorRoom, ActorMonster, DobjMonster, BlastSpell, NonCombat);
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

		/// <summary></summary>
		public virtual void CheckDisguisedMonster()
		{
			Debug.Assert(SetNextStateFunc != null && CopyCommandDataFunc != null);

			Debug.Assert(DobjArtAc != null);

			if (DobjArtAc.Type == ArtifactType.DisguisedMonster)
			{
				gEngine.RevealDisguisedMonster(ActorRoom, DobjArtifact);

				DisguisedMonster = gMDB[DobjArtAc.Field1];

				Debug.Assert(DisguisedMonster != null);

				RedirectCommand = null;

				if (BlastSpell)
				{
					RedirectCommand = gEngine.CreateInstance<IBlastCommand>(x =>
					{
						x.CastSpell = false;
					});
				}
				else
				{
					RedirectCommand = gEngine.CreateInstance<IAttackCommand>();
				}

				CopyCommandDataFunc(RedirectCommand);

				RedirectCommand.Dobj = DisguisedMonster;

				SetNextStateFunc(RedirectCommand);

				if (BlastSpell)
				{
					gEngine.ActionListCounter++;
				}

				CombatState = CombatState.EndAttack;

				goto Cleanup;
			}

			if (!BlastSpell)
			{
				PrintWhamHitObj(DobjArtifact);
			}

			CombatState = CombatState.CheckDeadBody;

		Cleanup:
			
			;
		}

		/// <summary></summary>
		public virtual void CheckDeadBody()
		{
			Debug.Assert(DobjArtifact != null && DobjArtAc != null);

			if (DobjArtAc.Type == ArtifactType.DeadBody)
			{
				DobjArtifact.SetInLimbo();

				PrintHackToBits(DobjArtifact, ActorMonster, BlastSpell);

				CombatState = CombatState.EndAttack;

				goto Cleanup;
			}

			CombatState = CombatState.CheckAlreadyBroken;

		Cleanup:

			;
		}

		/// <summary></summary>
		public virtual void CheckAlreadyBroken()
		{
			Debug.Assert(DobjArtAc != null);

			/*
				Damage it...
			*/

			KeyArtifactUid = DobjArtAc.GetKeyUid();

			if (KeyArtifactUid == -2)
			{
				PrintAlreadyBrokeIt(DobjArtifact);

				CombatState = CombatState.EndAttack;

				goto Cleanup;
			}

			CombatState = CombatState.CheckNotBreakable;

		Cleanup:

			;
		}

		/// <summary></summary>
		public virtual void CheckNotBreakable()
		{
			Debug.Assert(DobjArtAc != null);

			BreakageStrength = DobjArtAc.GetBreakageStrength();

			if (BreakageStrength < 1000)
			{
				PrintNothingHappens();

				CombatState = CombatState.EndAttack;

				goto Cleanup;
			}

			CombatState = CombatState.AttackHitArtifact;

		Cleanup:

			;
		}

		/// <summary></summary>
		public virtual void AttackHitArtifact()
		{
			Debug.Assert(ActorMonster != null);

			M = 0;

			if (BlastSpell)
			{
				if (gEngine.IsRulesetVersion(5))
				{
					D = 1;

					S = 6;
				}
				else
				{
					D = 2;

					S = 5;
				}
			}
			else
			{
				ActorWeaponUid = ActorMonster.Weapon;

				ActorWeapon = ActorWeaponUid > 0 ? gADB[ActorWeaponUid] : null;

				ActorAc = ActorWeapon != null ? ActorWeapon.GeneralWeapon : null;

				if (ActorAc != null)
				{
					D = Math.Max(1, ActorAc.Field3);

					S = Math.Max(1, ActorAc.Field4);
				}
				else
				{
					D = Math.Max(1, ActorMonster.NwDice);

					S = Math.Max(1, ActorMonster.NwSides);
				}
			}

			CombatState = CombatState.CalculateDamageArtifact;
		}

		/// <summary></summary>
		public virtual void CalculateDamageArtifact()
		{
			Debug.Assert(ActorMonster != null || !UseFractionalStrength);

			Debug.Assert(DobjArtifact != null);

			Debug.Assert(D > 0 && S > 0);

			if (UseFractionalStrength)
			{
				CalculateDamageForFractionalStrength();
			}
			else
			{
				_d2 = MaxDamage ? (D * S) + M : gEngine.RollDice(D, S, M);
			}

			CombatState = CombatState.CheckArtifactStatus;
		}

		/// <summary></summary>
		public virtual void CheckArtifactStatus()
		{
			RetCode rc;

			Debug.Assert(DobjArtifact != null && DobjArtAc != null);

			Debug.Assert(_d2 >= 0);

			BreakageStrength -= _d2;

			if (BreakageStrength > 1000)
			{
				DobjArtAc.SetBreakageStrength(BreakageStrength);

				CombatState = CombatState.EndAttack;

				goto Cleanup;
			}

			/*
				Broken!
			*/

			DobjArtAc.SetOpen(true);

			DobjArtAc.SetKeyUid(-2);

			DobjArtAc.Field4 = 0;

			DobjArtifact.Value = 0;

			rc = DobjArtifact.AddStateDesc(DobjArtifact.GetBrokenDesc());

			Debug.Assert(gEngine.IsSuccess(rc));

			CombatState = CombatState.CheckSpillContents;

		Cleanup:

			;
		}

		/// <summary></summary>
		public virtual void CheckSpillContents()
		{
			RetCode rc;

			Debug.Assert(DobjArtifact != null && DobjArtAc != null);

			try
			{
				gEngine.RevealContentCounter--;

				SpillContents = false;

				if (DobjArtAc.Type == ArtifactType.InContainer)
				{
					SpilledArtifactList = DobjArtifact.GetContainedList(containerType: ContainerType.In);

					if (DobjArtifact.OnContainer != null && DobjArtifact.IsInContainerOpenedFromTop())
					{
						SpilledArtifactList.AddRange(DobjArtifact.GetContainedList(containerType: ContainerType.On));
					}

					SpilledArtifactList = SpilledArtifactList.OrderByDescending(a => a.RecursiveWeight).ToList();

					foreach (var artifact in SpilledArtifactList)
					{
						artifact.Location = DobjArtifact.Location;

						while (true) 
						{
							SpilledArtifactContainer = artifact.GetCarriedByContainer();

							SpilledArtifactContainerType = artifact.GetCarriedByContainerContainerType();

							SpilledArtifactContainerAc = SpilledArtifactContainer != null && Enum.IsDefined(typeof(ContainerType), SpilledArtifactContainerType) ? gEngine.EvalContainerType(SpilledArtifactContainerType, SpilledArtifactContainer.InContainer, SpilledArtifactContainer.OnContainer, SpilledArtifactContainer.UnderContainer, SpilledArtifactContainer.BehindContainer) : null;

							if (SpilledArtifactContainer != null && SpilledArtifactContainerAc != null)
							{
								SpilledArtifactContainerSeen = true;

								var count = 0L;

								var weight = 0L;

								rc = SpilledArtifactContainer.GetContainerInfo(ref count, ref weight, SpilledArtifactContainerType, false);

								Debug.Assert(gEngine.IsSuccess(rc));

								if (count > SpilledArtifactContainerAc.Field4 || weight > SpilledArtifactContainerAc.Field3)
								{
									artifact.Location = SpilledArtifactContainer.Location;

									continue;
								}
							}

							break;
						}
					}

					if (SpilledArtifactList.Count > 0)
					{
						SpillContents = true;
					}

					DobjArtAc.Field3 = 0;
				}

				PrintSmashesToPieces(SpilledArtifactContainerSeen ? null : ActorRoom, DobjArtifact, SpillContents);

				CombatState = CombatState.EndAttack;
			}
			finally
			{
				gEngine.RevealContentCounter++;
			}
		}

		/// <summary></summary>
		public virtual void ExecuteStateMachine()
		{
			Debug.Assert(CombatState == CombatState.BeginAttack || CombatState == CombatState.CalculateDamage || CombatState == CombatState.CheckMonsterStatus || CombatState == CombatState.CheckDisguisedMonster);

			while (true)
			{
				switch (CombatState)
				{
					case CombatState.BeginAttack:

						BeginAttack();

						break;

					case CombatState.AttackMiss:

						AttackMiss();

						break;

					case CombatState.AttackFumble:

						AttackFumble();

						break;

					case CombatState.AttackHit:

						AttackHit();

						break;

					case CombatState.CalculateDamage:

						CalculateDamage();

						break;

					case CombatState.CheckArmor:

						CheckArmor();

						break;

					case CombatState.CheckMonsterStatus:

						CheckMonsterStatus();

						break;

					case CombatState.CheckDisguisedMonster:

						CheckDisguisedMonster();

						break;

					case CombatState.CheckDeadBody:

						CheckDeadBody();

						break;

					case CombatState.CheckAlreadyBroken:

						CheckAlreadyBroken();

						break;

					case CombatState.CheckNotBreakable:

						CheckNotBreakable();

						break;

					case CombatState.AttackHitArtifact:

						AttackHitArtifact();

						break;

					case CombatState.CalculateDamageArtifact:

						CalculateDamageArtifact();

						break;

					case CombatState.CheckArtifactStatus:

						CheckArtifactStatus();

						break;

					case CombatState.CheckSpillContents:

						CheckSpillContents();

						break;

					case CombatState.EndAttack:

						goto Cleanup;

					default:

						Debug.Assert(false, "Invalid CombatState");

						break;
				}
			}

		Cleanup:

			if (!OmitFinalNewLine)
			{
				gOut.WriteLine();
			}

			if (LightOut && ActorWeapon != null)
			{
				gEngine.LightOut(ActorWeapon);
			}
		}

		public CombatComponent()
		{
			ActorWeaponType = 0;

			DobjWeaponType = 0;
		}
	}
}
