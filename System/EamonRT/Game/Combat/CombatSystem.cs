
// CombatSystem.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Linq;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Combat;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Combat
{
	[ClassMappings]
	public class CombatSystem : ICombatSystem
	{
		/// <summary></summary>
		public long _odds = 0;

		/// <summary></summary>
		public long _rl = 0;

		/// <summary></summary>
		public long _d2 = 0;

		public virtual Action<IState> SetNextStateFunc { get; set; }

		public virtual IMonster OfMonster { get; set; }

		public virtual IMonster DfMonster { get; set; }

		public virtual long MemberNumber { get; set; }

		public virtual long AttackNumber { get; set; }

		public virtual bool BlastSpell { get; set; }

		public virtual bool UseAttacks { get; set; }

		public virtual bool MaxDamage { get; set; }

		public virtual bool OmitArmor { get; set; }

		public virtual bool OmitSkillGains { get; set; }

		public virtual bool OmitMonsterStatus { get; set; }

		public virtual bool OmitFinalNewLine { get; set; }

		public virtual AttackResult FixedResult { get; set; }

		public virtual WeaponRevealType WeaponRevealType { get; set; }

		/// <summary></summary>
		public virtual CombatState CombatState { get; set; }

		/// <summary></summary>
		public virtual IArtifactCategory OfAc { get; set; }

		/// <summary></summary>
		public virtual IArtifactCategory DfAc { get; set; }

		/// <summary></summary>
		public virtual IArtifactCategory ArAc { get; set; }

		/// <summary></summary>
		public virtual IArtifact WpnArtifact { get; set; }

		/// <summary></summary>
		public virtual IArtifact OfWeapon { get; set; }

		/// <summary></summary>
		public virtual IArtifact DfWeapon { get; set; }

		/// <summary></summary>
		public virtual IArtifact DfArmor { get; set; }

		/// <summary></summary>
		public virtual Weapon OfWeaponType { get; set; }

		/// <summary></summary>
		public virtual Weapon DfWeaponType { get; set; }

		/// <summary></summary>
		public virtual string OfMonsterName { get; set; }

		/// <summary></summary>
		public virtual string DfMonsterName { get; set; }

		/// <summary></summary>
		public virtual string AttackDesc { get; set; }

		/// <summary></summary>
		public virtual string AttackDesc01 { get; set; }

		/// <summary></summary>
		public virtual string MissDesc { get; set; }

		/// <summary></summary>
		public virtual bool UseFractionalStrength { get; set; }

		/// <summary></summary>
		public virtual bool OmitBboaPadding { get; set; }

		/// <summary></summary>
		public virtual bool LightOut { get; set; }

		/// <summary></summary>
		public virtual long OfWeaponUid { get; set; }

		/// <summary></summary>
		public virtual long DfWeaponUid { get; set; }

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
			if (BlastSpell)
			{
				PrintBlast();

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

		/// <summary></summary>
		public virtual void SetAttackDesc()
		{
			AttackDesc = "attack{0}";

			if (!UseAttacks)
			{
				if (OfMonster.IsCharacterMonster() || (OfMonster.IsInRoomLit() && OfMonster.CombatCode != CombatCode.Attacks))
				{
					AttackDesc = OfMonster.GetAttackDescString(OfWeapon);
				}
			}
		}

		/// <summary></summary>
		public virtual void PrintAttack()
		{
			SetAttackDesc();

			AttackDesc01 = string.Format(AttackDesc, OfMonster.IsCharacterMonster() ? "" : "s");

			OfMonsterName = OfMonster.IsCharacterMonster() ? "You" :
					OfMonster.EvalInRoomLightLevel(AttackNumber == 1 ? "An unseen offender" : "The unseen offender",
						OfMonster.InitGroupCount > 1 && AttackNumber == 1 ? OfMonster.GetArticleName(true, true, false, true) : OfMonster.GetTheName(true, true, false, true));

			DfMonsterName = DfMonster.IsCharacterMonster() ? "you" :
					DfMonster.EvalInRoomLightLevel("an unseen defender",
					DfMonster.InitGroupCount > 1 ? DfMonster.GetArticleName(groupCountOne: true) : DfMonster.GetTheName(groupCountOne: true));

			gOut.Write("{0}{1} {2} {3}{4}.",
				Environment.NewLine,
				OfMonsterName,
				AttackDesc01,
				DfMonsterName,
					OfWeapon != null &&
					(WeaponRevealType == WeaponRevealType.Always ||
					(WeaponRevealType == WeaponRevealType.OnlyIfSeen && OfWeapon.Seen)) ?
						" with " + OfWeapon.GetArticleName() :
						"");
		}

		/// <summary></summary>
		public virtual void PrintMiss()
		{
			MissDesc = DfMonster.GetMissDescString(DfWeapon);

			gOut.Write("{0} --- {1}!", Environment.NewLine, MissDesc);
		}

		/// <summary></summary>
		public virtual void PrintFumble()
		{
			gOut.Write("{0} ... A fumble!", Environment.NewLine);
		}

		/// <summary></summary>
		public virtual void PrintRecovered()
		{
			gOut.Write("{0}  Recovered.", Environment.NewLine);
		}

		/// <summary></summary>
		public virtual void PrintWeaponDropped()
		{
			gOut.Write("{0}  {1} {2} {3}!",
				Environment.NewLine,
				OfMonster.IsCharacterMonster() ? "You" :
				OfMonster.EvalInRoomLightLevel("The offender", OfMonster.GetTheName(true, true, false, true)),
				OfMonster.IsCharacterMonster() ? "drop" : "drops",
				OfMonster.IsCharacterMonster() || OfMonster.IsInRoomLit() ?
					(
						(WeaponRevealType == WeaponRevealType.Never || 
						(WeaponRevealType == WeaponRevealType.OnlyIfSeen && !OfWeapon.Seen)) ? 
							OfWeapon.GetArticleName(buf: Globals.Buf01) :
							OfWeapon.GetTheName(buf: Globals.Buf01)
					) : 
					"a weapon");
		}

		/// <summary></summary>
		public virtual void PrintWeaponHitsUser()
		{
			gOut.Write("{0}  Weapon hits user!", Environment.NewLine);
		}

		/// <summary></summary>
		public virtual void PrintSparksFly()
		{
			gOut.Write("{0}  Sparks fly from {1}!",
				Environment.NewLine,
				OfMonster.IsCharacterMonster() || OfMonster.IsInRoomLit() ? 
					(
						(WeaponRevealType == WeaponRevealType.Never ||
						(WeaponRevealType == WeaponRevealType.OnlyIfSeen && !OfWeapon.Seen)) ?
							OfWeapon.GetArticleName() :
							OfWeapon.GetTheName()
					) : 
					"a weapon");
		}

		/// <summary></summary>
		public virtual void PrintWeaponDamaged()
		{
			gOut.Write("{0}  Weapon damaged!", Environment.NewLine);
		}

		/// <summary></summary>
		public virtual void PrintWeaponBroken()
		{
			gOut.Write("{0}  Weapon broken!", Environment.NewLine);
		}

		/// <summary></summary>
		public virtual void PrintBrokenWeaponHitsUser()
		{
			gOut.Write("{0}  Broken weapon hits user!", Environment.NewLine);
		}

		/// <summary></summary>
		public virtual void PrintStarPlus()
		{
			gOut.Write("{0} {1} ", Environment.NewLine, DfMonster.IsCharacterMonster() ? "***" : "+++");
		}

		/// <summary></summary>
		public virtual void PrintHit()
		{
			gOut.Write("A hit!");
		}

		/// <summary></summary>
		public virtual void PrintCriticalHit()
		{
			gOut.Write("A critical hit!");
		}

		/// <summary></summary>
		public virtual void PrintBlowTurned()
		{
			if (DfMonster.Armor < 1)
			{
				gOut.Write("{0}{1}Blow turned!", Environment.NewLine, OmitBboaPadding ? "" : "  ");
			}
			else
			{
				var armorDesc = DfMonster.GetArmorDescString();

				gOut.Write("{0}{1}Blow bounces off {2}!", Environment.NewLine, OmitBboaPadding ? "" : "  ", armorDesc);
			}
		}

		/// <summary></summary>
		public virtual void PrintHealthStatus()
		{
			DfMonsterName = DfMonster.IsCharacterMonster() ? "You" :
				BlastSpell && DfMonster.InitGroupCount > 1 ? DfMonster.EvalInRoomLightLevel(DfMonster == OfMonster ? "An offender" : "A defender", DfMonster.GetArticleName(true, true, false, true)) :
				DfMonster.EvalInRoomLightLevel(DfMonster == OfMonster ? "The offender" : "The defender", DfMonster.GetTheName(true, true, false, true, Globals.Buf01));

			Globals.Buf.SetFormat("{0}{1} {2} ",
				Environment.NewLine,
				DfMonsterName,
				DfMonster.IsCharacterMonster() ? "are" : "is");

			DfMonster.AddHealthStatus(Globals.Buf, false);

			gOut.Write("{0}", Globals.Buf);
		}

		/// <summary></summary>
		public virtual void PrintBlast()
		{
			gOut.Print("{0}", gEngine.GetBlastDesc());
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
			Debug.Assert(OfMonster != null && OfMonster.CombatCode != CombatCode.NeverFights);

			Debug.Assert(DfMonster != null);

			Debug.Assert(MemberNumber > 0);

			OfWeaponUid = OfMonster.Weapon;

			if (OfWeaponUid > 0 && OfMonster.GroupCount == 1 && AttackNumber > 1 && OfMonster.CanAttackWithMultipleWeapons())
			{
				var weaponList = OfMonster.GetCarriedList().Where(x => x.IsReadyableByMonster(OfMonster)).ToList();

				var weaponCount = weaponList.Count;

				if ((AttackNumber - 1) % weaponCount != 0)
				{
					OfWeapon = gEngine.GetNthArtifact(weaponList, (AttackNumber - 1) % weaponCount, x => x.Uid != OfWeaponUid);

					OfWeaponUid = OfWeapon.Uid;
				}
				else
				{
					OfWeapon = gADB[OfWeaponUid];
				}
			}
			else if (OfWeaponUid > 0 && OfMonster.CurrGroupCount > 1 && MemberNumber > 1)
			{
				OfWeapon = gEngine.GetNthArtifact(OfMonster.GetCarriedList(), MemberNumber - 1, x => x.IsReadyableByMonster(OfMonster) && x.Uid != OfWeaponUid);

				OfWeaponUid = OfWeapon != null ? OfWeapon.Uid : OfMonster.NwDice > 0 && OfMonster.NwSides > 0 ? 0 : -1;

				if (OfWeaponUid < 0)
				{
					CombatState = CombatState.EndAttack;

					goto Cleanup;
				}
			}
			else
			{
				OfWeapon = OfWeaponUid > 0 ? gADB[OfWeaponUid] : null;
			}

			Debug.Assert(OfWeaponUid == 0 || (OfWeapon != null && OfWeapon.GeneralWeapon != null));

			OfAc = OfWeapon != null ? OfWeapon.GeneralWeapon : null;

			Af = gEngine.GetArmorFactor(gGameState.Ar, gGameState.Sh);

			gEngine.GetOddsToHit(OfMonster, DfMonster, OfAc, Af, ref _odds);

			RollToHitOrMiss();

			if (OfMonster.IsCharacterMonster() && _rl < 97 && (_rl < 5 || _rl <= _odds) && !OmitSkillGains)
			{
				gEngine.CheckPlayerSkillGains(OfAc, Af);
			}

			OfWeaponType = (Weapon)(OfAc != null ? OfAc.Field2 : 0);

			PrintAttack();

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
			DfWeaponUid = DfMonster.Weapon;

			DfWeapon = DfWeaponUid > 0 ? gADB[DfWeaponUid] : null;

			DfAc = DfWeapon != null ? DfWeapon.GeneralWeapon : null;

			DfWeaponType = (Weapon)(DfAc != null ? DfAc.Field2 : 0);

			if (_rl < 97 || OfWeaponUid == 0)
			{
				PrintMiss();

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
				if (gGameState.Ls > 0 && gGameState.Ls == OfWeaponUid)
				{
					LightOut = true;
				}

				OfWeapon.SetInRoom(OfMonster.GetInRoom());

				WpnArtifact = gADB[OfMonster.Weapon];

				Debug.Assert(WpnArtifact != null);

				rc = WpnArtifact.RemoveStateDesc(WpnArtifact.GetReadyWeaponDesc());

				Debug.Assert(gEngine.IsSuccess(rc));

				OfMonster.Weapon = !OfMonster.IsCharacterMonster() ? -OfWeaponUid - 1 : -1;

				PrintWeaponDropped();

				CombatState = CombatState.EndAttack;

				goto Cleanup;
			}

			if (_rl > 95)
			{
				PrintWeaponHitsUser();

				DfMonster = OfMonster;

				CombatState = CombatState.AttackHit;

				goto Cleanup;
			}

			if (OfAc.Type == ArtifactType.MagicWeapon)
			{
				PrintSparksFly();

				CombatState = CombatState.EndAttack;

				goto Cleanup;
			}

			if (_rl < 91)
			{
				OfAc.Field4--;

				if (OfAc.Field4 > 0)
				{
					PrintWeaponDamaged();

					CombatState = CombatState.EndAttack;

					goto Cleanup;
				}
			}

			PrintWeaponBroken();

			if (gGameState.Ls > 0 && gGameState.Ls == OfWeaponUid)
			{
				LightOut = true;
			}

			OfWeapon.SetInLimbo();

			WpnArtifact = gADB[OfMonster.Weapon];

			Debug.Assert(WpnArtifact != null);

			rc = WpnArtifact.RemoveStateDesc(WpnArtifact.GetReadyWeaponDesc());

			Debug.Assert(gEngine.IsSuccess(rc));

			OfMonster.Weapon = -1;

			_rl = gEngine.RollDice(1, 100, 0);

			if (_rl > 50 || OfAc.Field4 <= 0)
			{
				CombatState = CombatState.EndAttack;

				goto Cleanup;
			}

			PrintBrokenWeaponHitsUser();

			DfMonster = OfMonster;

			_rl = gEngine.RollDice(1, 5, 95);

			CombatState = CombatState.AttackHit;

		Cleanup:

			;
		}

		/// <summary></summary>
		public virtual void AttackHit()
		{
			if (OfAc != null)
			{
				D = OfAc.Field3;

				S = OfAc.Field4;
			}
			else
			{
				D = OfMonster.NwDice;

				S = OfMonster.NwSides;
			}

			A = OmitArmor ? 0 : 1;

			PrintStarPlus();

			if ((OfMonster != DfMonster && _rl >= 5) || (OfMonster == DfMonster && _rl < 100))
			{
				PrintHit();

				CombatState = CombatState.CalculateDamage;

				goto Cleanup;
			}

			PrintCriticalHit();

			if (OfMonster != DfMonster || !Globals.IsRulesetVersion(5, 15, 25))
			{
				_rl = gEngine.RollDice(1, 100, 0);

				if (_rl == 100)
				{
					_d2 = DfMonster.Hardiness - DfMonster.DmgTaken - (Globals.IsRulesetVersion(5, 15, 25) ? 0 : 2);

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
			Debug.Assert(OfMonster != null && UseFractionalStrength);

			_d2 = 0;

			var xx = (double)(OfMonster.Hardiness - OfMonster.DmgTaken) / (double)OfMonster.Hardiness;      // Fractional strength

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
			Debug.Assert(OfMonster != null || !UseFractionalStrength);

			Debug.Assert(DfMonster != null);

			Debug.Assert(D > 0 && S > 0 && A >= 0 && A <= 1);

			if (UseFractionalStrength)
			{
				CalculateDamageForFractionalStrength();
			}
			else
			{
				_d2 = MaxDamage ? (D * S) + M : gEngine.RollDice(D, S, M);
			}

			_d2 -= (A * DfMonster.Armor);

			CombatState = CombatState.CheckArmor;
		}

		/// <summary></summary>
		public virtual void CheckArmor()
		{
			if (_d2 < 1)
			{
				PrintBlowTurned();

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
			Debug.Assert(DfMonster != null);

			Debug.Assert(_d2 >= 0);

			DfMonster.DmgTaken += _d2;

			if (!OmitMonsterStatus || OfMonster == DfMonster)
			{
				PrintHealthStatus();
			}

			if (DfMonster.IsDead())
			{
				if (DfMonster.IsCharacterMonster())
				{
					gGameState.Die = 1;

					if (SetNextStateFunc != null)
					{
						SetNextStateFunc(Globals.CreateInstance<IPlayerDeadState>(x =>
						{
							x.PrintLineSep = true;
						}));
					}
				}
				else
				{
					gEngine.MonsterDies(OfMonster, DfMonster);
				}
			}

			CombatState = CombatState.EndAttack;
		}

		/// <summary></summary>
		public virtual void ExecuteStateMachine()
		{
			Debug.Assert(CombatState == CombatState.BeginAttack || CombatState == CombatState.CalculateDamage || CombatState == CombatState.CheckMonsterStatus);

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

			if (LightOut && OfWeapon != null)
			{
				gEngine.LightOut(OfWeapon);
			}
		}

		public CombatSystem()
		{
			OfWeaponType = 0;

			DfWeaponType = 0;
		}
	}
}
