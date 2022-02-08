
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
using static EamonRT.Game.Plugin.PluginContext;

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
		public virtual IList<IArtifact> SpilledArtifactList { get; set; }

		/// <summary></summary>
		public virtual IMonster DisguisedMonster { get; set; }

		/// <summary></summary>
		public virtual ICommand RedirectCommand { get; set; }

		/// <summary></summary>
		public virtual IArtifactCategory ActorAc { get; set; }

		/// <summary></summary>
		public virtual IArtifactCategory DobjAc { get; set; }

		/// <summary></summary>
		public virtual IArtifact WpnArtifact { get; set; }

		/// <summary></summary>
		public virtual IArtifact ActorWeapon { get; set; }

		/// <summary></summary>
		public virtual IArtifact DobjWeapon { get; set; }

		/// <summary></summary>
		public virtual Weapon ActorWeaponType { get; set; }

		/// <summary></summary>
		public virtual Weapon DobjWeaponType { get; set; }

		/// <summary></summary>
		public virtual bool ContentsSpilled { get; set; }

		/// <summary></summary>
		public virtual bool UseFractionalStrength { get; set; }

		/// <summary></summary>
		public virtual bool OmitBboaPadding { get; set; }

		/// <summary></summary>
		public virtual bool LightOut { get; set; }

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

		public virtual void ExecuteCalculateDamage(long numDice, long numSides, long mod = 0)
		{
			CombatState = CombatState.CalculateDamage;

			D = numDice;

			S = numSides;

			M = mod;

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
					PrintZapDirectHit();

					if (Globals.IsRulesetVersion(5, 15, 25))
					{
						ExecuteCalculateDamage(1, 6);
					}
					else
					{
						ExecuteCalculateDamage(2, 5);
					}
				}
				else
				{
					CombatState = CombatState.BeginAttack;

					ExecuteStateMachine();
				}

				Globals.Thread.Sleep(gGameState.PauseCombatMs);
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

				if (rl > gCharacter.GetWeaponAbilities(s))
				{
					var weapon = gEngine.GetWeapons(s);

					Debug.Assert(weapon != null);

					Globals.WeaponSkillIncreaseFunc = () =>
					{
						if (!Globals.IsRulesetVersion(5, 15, 25))
						{
							PrintWeaponAbilityIncreased(s, weapon);
						}

						gCharacter.ModWeaponAbilities(s, 2);

						if (gCharacter.GetWeaponAbilities(s) > weapon.MaxValue)
						{
							gCharacter.SetWeaponAbilities(s, weapon.MaxValue);
						}
					};
				}

				var x = Math.Abs(af);

				if (x > 0)
				{
					rl = gEngine.RollDice(1, x, 0);

					rl += (long)Math.Round(((double)x / 100.0) * (double)gCharacter.GetIntellectBonusPct());

					if (rl > gCharacter.ArmorExpertise)
					{
						Globals.ArmorSkillIncreaseFunc = () =>
						{
							if (!Globals.IsRulesetVersion(5, 15, 25))
							{
								PrintArmorExpertiseIncreased();
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
						};
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

			if (ActorWeaponUid > 0 && ActorMonster.GroupCount == 1 && AttackNumber > 1 && ActorMonster.CanAttackWithMultipleWeapons())
			{
				var weaponList = ActorMonster.GetCarriedList().Where(x => x.IsReadyableByMonster(ActorMonster)).ToList();

				var weaponCount = weaponList.Count;

				if ((AttackNumber - 1) % weaponCount != 0)
				{
					ActorWeapon = gEngine.GetNthArtifact(weaponList, (AttackNumber - 1) % weaponCount, x => x.Uid != ActorWeaponUid);

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

			Debug.Assert(ActorWeaponUid == 0 || (ActorWeapon != null && ActorWeapon.GeneralWeapon != null));

			ActorAc = ActorWeapon != null ? ActorWeapon.GeneralWeapon : null;

			Af = gEngine.GetArmorFactor(gGameState.Ar, gGameState.Sh);

			gEngine.GetOddsToHit(ActorMonster, DobjMonster, ActorAc, Af, ref _odds);

			RollToHitOrMiss();

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
		public virtual void AttackMiss()
		{
			DobjWeaponUid = DobjMonster.Weapon;

			DobjWeapon = DobjWeaponUid > 0 ? gADB[DobjWeaponUid] : null;

			DobjAc = DobjWeapon != null ? DobjWeapon.GeneralWeapon : null;

			DobjWeaponType = (Weapon)(DobjAc != null ? DobjAc.Field2 : 0);

			if (_rl < 97 || ActorWeaponUid == 0)
			{
				PrintMiss(DobjMonster, DobjWeapon);

				CombatState = CombatState.EndAttack;

				goto Cleanup;
			}

			CombatState = CombatState.AttackFumble;

		Cleanup:

			;
		}

		/// <summary></summary>
		public virtual void AttackFumble()
		{
			RetCode rc;

			PrintFumble();

			_rl = gEngine.RollDice(1, 100, 0);

			if ((Globals.IsRulesetVersion(5, 15, 25) && _rl < 36) || (!Globals.IsRulesetVersion(5, 15, 25) && _rl < 41))
			{
				PrintRecovered();

				CombatState = CombatState.EndAttack;

				goto Cleanup;
			}

			if ((Globals.IsRulesetVersion(5, 15, 25) && _rl < 76) || (!Globals.IsRulesetVersion(5, 15, 25) && _rl < 81))
			{
				if (gGameState.Ls > 0 && gGameState.Ls == ActorWeaponUid)
				{
					LightOut = true;
				}

				ActorWeapon.SetInRoom(ActorRoom);

				WpnArtifact = gADB[ActorMonster.Weapon];

				Debug.Assert(WpnArtifact != null);

				rc = WpnArtifact.RemoveStateDesc(WpnArtifact.GetReadyWeaponDesc());

				Debug.Assert(gEngine.IsSuccess(rc));

				ActorMonster.Weapon = !ActorMonster.IsCharacterMonster() ? -ActorWeaponUid - 1 : -1;

				PrintWeaponDropped(ActorRoom, ActorMonster, ActorWeapon, WeaponRevealType);

				CombatState = CombatState.EndAttack;

				goto Cleanup;
			}

			if (_rl > 95)
			{
				PrintWeaponHitsUser();

				Dobj = ActorMonster;

				CombatState = CombatState.AttackHit;

				goto Cleanup;
			}

			if (ActorAc.Type == ArtifactType.MagicWeapon)
			{
				PrintSparksFly(ActorRoom, ActorMonster, ActorWeapon, WeaponRevealType);

				CombatState = CombatState.EndAttack;

				goto Cleanup;
			}

			if (_rl < 91)
			{
				ActorAc.Field4--;

				if (ActorAc.Field4 > 0)
				{
					PrintWeaponDamaged();

					CombatState = CombatState.EndAttack;

					goto Cleanup;
				}
			}

			PrintWeaponBroken();

			if (gGameState.Ls > 0 && gGameState.Ls == ActorWeaponUid)
			{
				LightOut = true;
			}

			ActorWeapon.SetInLimbo();

			WpnArtifact = gADB[ActorMonster.Weapon];

			Debug.Assert(WpnArtifact != null);

			rc = WpnArtifact.RemoveStateDesc(WpnArtifact.GetReadyWeaponDesc());

			Debug.Assert(gEngine.IsSuccess(rc));

			ActorMonster.Weapon = -1;

			_rl = gEngine.RollDice(1, 100, 0);

			if (_rl > 50 || ActorAc.Field4 <= 0)
			{
				CombatState = CombatState.EndAttack;

				goto Cleanup;
			}

			PrintBrokenWeaponHitsUser();

			Dobj = ActorMonster;

			_rl = gEngine.RollDice(1, 5, 95);

			CombatState = CombatState.AttackHit;

		Cleanup:

			;
		}

		/// <summary></summary>
		public virtual void AttackHit()
		{
			if (ActorAc != null)
			{
				D = ActorAc.Field3;

				S = ActorAc.Field4;
			}
			else
			{
				D = ActorMonster.NwDice;

				S = ActorMonster.NwSides;
			}

			A = OmitArmor ? 0 : 1;

			PrintStarPlus(DobjMonster);

			if ((ActorMonster != DobjMonster && _rl >= 5) || (ActorMonster == DobjMonster && _rl < 100))
			{
				PrintHit();

				CombatState = CombatState.CalculateDamage;

				goto Cleanup;
			}

			PrintCriticalHit();

			if (ActorMonster != DobjMonster || !Globals.IsRulesetVersion(5, 15, 25))
			{
				_rl = gEngine.RollDice(1, 100, 0);

				if (_rl == 100)
				{
					_d2 = DobjMonster.Hardiness - DobjMonster.DmgTaken - (Globals.IsRulesetVersion(5, 15, 25) ? 0 : 2);

					CombatState = CombatState.CheckArmor;

					goto Cleanup;
				}

				if (_rl < (Globals.IsRulesetVersion(5, 15, 25) ? 51 : 50))
				{
					A = 0;

					CombatState = CombatState.CalculateDamage;

					goto Cleanup;
				}

				if (_rl < 86 || !Globals.IsRulesetVersion(5, 15, 25))
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

			CombatState = CombatState.CheckArmor;
		}

		/// <summary></summary>
		public virtual void CheckArmor()
		{
			if (_d2 < 1)
			{
				PrintBlowTurned(DobjMonster, OmitBboaPadding);

				CombatState = CombatState.EndAttack;

				goto Cleanup;
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
				PrintHealthStatus(ActorRoom, ActorMonster, DobjMonster, BlastSpell);
			}

			if (DobjMonster.IsDead())
			{
				if (DobjMonster.IsCharacterMonster())
				{
					gGameState.Die = 1;

					SetNextStateFunc(Globals.CreateInstance<IPlayerDeadState>(x =>
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
					RedirectCommand = Globals.CreateInstance<IBlastCommand>(x =>
					{
						x.CastSpell = false;
					});
				}
				else
				{
					RedirectCommand = Globals.CreateInstance<IAttackCommand>();
				}

				CopyCommandDataFunc(RedirectCommand);

				RedirectCommand.Dobj = DisguisedMonster;

				SetNextStateFunc(RedirectCommand);

				CombatState = CombatState.EndAttack;

				goto Cleanup;
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
				if (BlastSpell)
				{
					PrintZapDirectHit();
				}

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
				if (Globals.IsRulesetVersion(5, 15, 25))
				{
					D = 1;

					S = 6;
				}
				else
				{
					D = 2;

					S = 5;
				}

				PrintZapDirectHit();
			}
			else
			{
				ActorWeaponUid = ActorMonster.Weapon;

				ActorWeapon = ActorWeaponUid > 0 ? gADB[ActorWeaponUid] : null;

				ActorAc = ActorWeapon != null ? ActorWeapon.GeneralWeapon : null;

				if (ActorAc != null)
				{
					D = ActorAc.Field3;

					S = ActorAc.Field4;
				}
				else
				{
					D = ActorMonster.NwDice;

					S = ActorMonster.NwSides;
				}

				PrintWhamHitObj(DobjArtifact);
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

			CombatState = CombatState.CheckContentsSpilled;

		Cleanup:

			;
		}

		/// <summary></summary>
		public virtual void CheckContentsSpilled()
		{
			Debug.Assert(DobjArtifact != null && DobjArtAc != null);

			ContentsSpilled = false;

			if (DobjArtAc.Type == ArtifactType.InContainer)
			{
				SpilledArtifactList = DobjArtifact.GetContainedList(containerType: ContainerType.In);

				if (DobjArtifact.OnContainer != null && DobjArtifact.IsInContainerOpenedFromTop())
				{
					SpilledArtifactList.AddRange(DobjArtifact.GetContainedList(containerType: ContainerType.On));
				}

				foreach (var artifact in SpilledArtifactList)
				{
					artifact.SetInRoom(ActorRoom);
				}

				if (SpilledArtifactList.Count > 0)
				{
					ContentsSpilled = true;
				}

				DobjArtAc.Field3 = 0;
			}

			PrintSmashesToPieces(ActorRoom, DobjArtifact, ContentsSpilled);

			CombatState = CombatState.EndAttack;
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

					case CombatState.CheckContentsSpilled:

						CheckContentsSpilled();

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
