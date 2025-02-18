﻿
// Monster.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using Enums = Eamon.Framework.Primitive.Enums;
using static Eamon.Game.Plugin.Globals;

namespace Eamon.Game
{
	[ClassMappings]
	public class Monster : GameBase, IMonster
	{
		#region Public Fields

		public long _courage;

		public long _location;

		#endregion

		#region Public Properties

		#region Interface IMonster

		[FieldName(320)]
		public virtual string StateDesc { get; set; }

		[FieldName(520)]
		public virtual bool IsListed { get; set; }

		[FieldName(540)]
		public virtual PluralType PluralType { get; set; }

		[FieldName(620)]
		public virtual long Hardiness { get; set; }

		[FieldName(640)]
		public virtual long Agility { get; set; }

		[FieldName(660)]
		public virtual long GroupCount { get; set; }

		[FieldName(680)]
		public virtual long AttackCount { get; set; }

		[FieldName(700)]
		public virtual long Courage
		{
			get
			{
				return gEngine.EnableMutateProperties && gEngine.IsRulesetVersion(5, 62) && IsWeaponless(false) && _courage < 200 ? _courage / 2 : _courage;
			}

			set
			{
				_courage = value;
			}
		}

		[FieldName(720)]
		public virtual long Location 
		{ 
			get
			{
				return _location;
			}

			set
			{
				if (gEngine.EnableMutateProperties && _location != value && HasMoved(_location, value))
				{
					Moved = true;
				}

				_location = value;
			}
		}

		[FieldName(740)]
		public virtual CombatCode CombatCode { get; set; }

		[FieldName(760)]
		public virtual long Armor { get; set; }

		[FieldName(780)]
		public virtual long Weapon { get; set; }

		[FieldName(800)]
		public virtual long NwDice { get; set; }

		[FieldName(820)]
		public virtual long NwSides { get; set; }

		[FieldName(840)]
		public virtual long DeadBody { get; set; }

		[FieldName(860)]
		public virtual Friendliness Friendliness { get; set; }

		[FieldName(880)]
		public virtual Gender Gender { get; set; }

		[FieldName(900)]
		public virtual long InitGroupCount { get; set; }

		[FieldName(920)]
		public virtual long CurrGroupCount { get; set; }

		[FieldName(940)]
		public virtual Friendliness Reaction { get; set; }

		[FieldName(960)]
		public virtual long DmgTaken { get; set; }

		[FieldName(980)]
		public virtual long Field1 { get; set; }

		[FieldName(1000)]
		public virtual long Field2 { get; set; }

		[FieldName(1020)]
		public virtual IMonsterSpell[] Spells { get; set; }

		#endregion

		#endregion

		#region Public Methods

		#region Interface IDisposable

		public override void Dispose(bool disposing)
		{
			if (disposing)
			{
				// Get rid of managed resources
			}

			if (IsUidRecycled && Uid > 0)
			{
				gDatabase.FreeMonsterUid(Uid);

				Uid = 0;
			}
		}

		#endregion

		#region Interface IGameBase

		public override void SetParentReferences()
		{
			if (Spells != null)
			{
				foreach (var s in Spells)
				{
					if (s != null)
					{
						s.Parent = this;
					}
				}
			}
		}

		public override string GetPluralName(string fieldName)
		{
			IEffect effect;
			long effectUid;
			string result;

			if (string.IsNullOrWhiteSpace(fieldName))
			{
				result = null;

				// PrintError

				goto Cleanup;
			}

			Debug.Assert(fieldName == "Name");

			var buf = new StringBuilder(gEngine.BufSize);

			effectUid = gEngine.GetPluralTypeEffectUid(PluralType);

			effect = gEDB[effectUid];

			if (effect != null)
			{
				buf.Append(effect.Desc.Substring(0, Math.Min(gEngine.MonNameLen, effect.Desc.Length)).Trim());
			}
			else
			{
				buf.Append(Name);

				if (buf.Length > 0 && PluralType == PluralType.YIes)
				{
					buf.Length--;
				}

				buf.Append(PluralType == PluralType.None ? "" :
						PluralType == PluralType.Es ? "es" :
						PluralType == PluralType.YIes ? "ies" :
						"s");
			}

			result = buf.ToString();

		Cleanup:

			return result;
		}

		public override string GetDecoratedName(string fieldName, ArticleType articleType, bool upshift = false, bool showCharOwned = true, bool showStateDesc = false, bool showContents = false, bool groupCountOne = false)
		{
			string result;
			long gc;

			if (string.IsNullOrWhiteSpace(fieldName))
			{
				result = null;

				// PrintError

				goto Cleanup;
			}

			Debug.Assert(fieldName == "Name");

			var buf = new StringBuilder(gEngine.BufSize);

			var buf01 = new StringBuilder(gEngine.BufSize);

			gc = groupCountOne ? 1 : CurrGroupCount;

			switch (articleType)
			{
				case ArticleType.None:

					if (gc > 10)
					{
						buf01.AppendFormat("{0} ", gc);
					}
					else
					{
						buf01.Append(gc > 1 ? gEngine.GetStringFromNumber(gc, true, new StringBuilder(gEngine.BufSize)) : "");
					}

					buf.AppendFormat
					(
						"{0}{1}{2}",
						buf01.ToString(),
						gc > 1 ? GetPluralName(fieldName) :
						Name,
						showStateDesc ? StateDesc : ""
					);

					break;

				case ArticleType.The:

					if (gc > 10)
					{
						buf01.AppendFormat("{0}{1} ", "the ", gc);
					}
					else
					{
						buf01.AppendFormat
						(
							"{0}{1}",
							gc > 1 ? "the " :
							ArticleType == ArticleType.None ? "" :
							"the ",
							gc > 1 ? gEngine.GetStringFromNumber(gc, true, new StringBuilder(gEngine.BufSize)) : ""
						);
					}

					buf.AppendFormat
					(
						"{0}{1}{2}",
						buf01.ToString(),
						gc > 1 ? GetPluralName(fieldName) :
						Name,
						showStateDesc ? StateDesc : ""
					);

					break;

				default:

					if (gc > 10)
					{
						buf01.AppendFormat("{0} ", gc);
					}
					else
					{
						buf01.Append
						(
							gc > 1 ? gEngine.GetStringFromNumber(gc, true, new StringBuilder(gEngine.BufSize)) :
							ArticleType == ArticleType.None ? "" :
							ArticleType == ArticleType.The ? "the " :
							ArticleType == ArticleType.Some ? "some " :
							ArticleType == ArticleType.An ? "an " :
							"a "
						);
					}

					buf.AppendFormat
					(
						"{0}{1}{2}",
						buf01.ToString(),
						gc > 1 ? GetPluralName(fieldName) :
						Name,
						showStateDesc ? StateDesc : ""
					);

					break;
			}

			if (buf.Length > 0 && upshift)
			{
				buf[0] = Char.ToUpper(buf[0]);
			}

			result = buf.ToString();

		Cleanup:

			return result;
		}

		public override RetCode BuildPrintedFullDesc(StringBuilder buf, bool showName, bool showVerboseName)
		{
			RetCode rc;

			if (buf == null)
			{
				rc = RetCode.InvalidArg;

				// PrintError

				goto Cleanup;
			}

			rc = RetCode.Success;

			if (showName || showVerboseName)
			{
				buf.AppendFormat("{0}[{1}]",
					Environment.NewLine,
					GetArticleName
					(
						true, 
						true,
						showVerboseName && !string.IsNullOrWhiteSpace(StateDesc) && ShouldShowVerboseNameStateDesc(),
						false,
						false
					)
				);
			}

			if (!string.IsNullOrWhiteSpace(Desc))
			{
				buf.AppendFormat("{0}{1}", Environment.NewLine, Desc);
			}

			if (showName || showVerboseName || !string.IsNullOrWhiteSpace(Desc))
			{
				buf.Append(Environment.NewLine);
			}

		Cleanup:

			return rc;
		}

		#endregion

		#region Interface IComparable

		public virtual int CompareTo(IMonster monster)
		{
			return this.Uid.CompareTo(monster.Uid);
		}

		#endregion

		#region Interface IMonster

		public virtual bool IsDead()
		{
			return DmgTaken >= Hardiness;
		}

		public virtual bool IsCarryingWeapon()
		{
			return Weapon > 0 && Weapon < 1001;
		}

		public virtual bool IsWeaponless(bool includeWeaponFumble)
		{
			return includeWeaponFumble ? Weapon < 0 : Weapon == -1;
		}

		public virtual bool HasDeadBody()
		{
			return DeadBody > 0 && DeadBody < 1001;
		}

		public virtual bool HasWornInventory()
		{
			return true;
		}

		public virtual bool HasCarriedInventory()
		{
			return true;
		}

		public virtual bool HasHumanNaturalAttackDescs()
		{
			return IsCharacterMonster();
		}

		public virtual bool HasMoved(long oldLocation, long newLocation)
		{
			return oldLocation != gEngine.LimboLocation && newLocation != gEngine.LimboLocation;
		}

		public virtual bool IsInRoom()
		{
			return Location > 0 && Location < 1001;
		}

		public virtual bool IsInLimbo()
		{
			return Location == gEngine.LimboLocation;
		}

		public virtual bool IsInRoomUid(long roomUid)
		{
			return Location == roomUid;
		}

		public virtual bool IsInRoom(IRoom room)
		{
			Debug.Assert(room != null);

			return IsInRoomUid(room.Uid);
		}

		public virtual bool IsAttackable(IMonster monster)
		{
			Debug.Assert(monster != null);

			return true;
		}

		public virtual bool CanMoveToRoomUid(long roomUid, bool fleeing)
		{
			Debug.Assert(roomUid >= 0 || roomUid < 0);		// Just for clarity

			return true;
		}

		public virtual bool CanMoveToRoom(IRoom room, bool fleeing)
		{
			return CanMoveToRoomUid(room != null ? room.Uid : 0, fleeing);
		}

		public virtual bool CanMoveInDirection(Direction dir, bool fleeing)
		{
			Debug.Assert(Enum.IsDefined(typeof(Direction), dir));

			return true;
		}

		public virtual bool CanAttackWithMultipleWeapons()
		{
			return false;
		}

		public virtual bool CanCarryArtifactWeight(IArtifact artifact)
		{
			RetCode rc;

			Debug.Assert(artifact != null);

			var result = false;

			var c = 0L;

			var w = artifact.Weight;

			if (artifact.GeneralContainer != null)
			{
				rc = artifact.GetContainerInfo(ref c, ref w, ContainerType.In, true);

				Debug.Assert(gEngine.IsSuccess(rc));

				rc = artifact.GetContainerInfo(ref c, ref w, ContainerType.On, true);

				Debug.Assert(gEngine.IsSuccess(rc));
			}

			var monWeight = 0L;

			rc = GetFullInventoryWeight(ref monWeight, recurse: true);

			Debug.Assert(gEngine.IsSuccess(rc));

			if (IsCharacterMonster())
			{
				result = w + monWeight <= GetWeightCarryableGronds();
			}
			else
			{
				result = w <= GetWeightCarryableGronds() && w + monWeight <= GetWeightCarryableGronds() * CurrGroupCount;
			}

			return result;
		}

		public virtual long GetCarryingWeaponUid()
		{
			return IsCarryingWeapon() ? Weapon : 0;
		}

		public virtual long GetDeadBodyUid()
		{
			return HasDeadBody() ? DeadBody : 0;
		}

		public virtual long GetInRoomUid()
		{
			return IsInRoom() ? Location : 0;
		}

		public virtual IRoom GetInRoom()
		{
			var uid = GetInRoomUid();

			return gRDB[uid];
		}

		public virtual void SetInRoomUid(long roomUid)
		{
			Location = roomUid;

			var gameState = gEngine.GetGameState();

			if (IsCharacterMonster() && gameState != null)
			{
				gameState.Ro = roomUid;
			}
		}

		public virtual void SetInLimbo()
		{
			SetInRoomUid(gEngine.LimboLocation);
		}

		public virtual void SetInRoom(IRoom room)
		{
			Debug.Assert(room != null);

			SetInRoomUid(room.Uid);
		}

		public virtual bool IsInRoomLit()
		{
			var room = GetInRoom();

			return room != null && room.IsLit();
		}

		public virtual bool IsInRoomViewable()
		{
			var room = GetInRoom();

			return room != null && room.IsViewable();
		}

		public virtual bool ShouldFleeRoom()
		{
			return CheckNBTLHostility();
		}

		public virtual bool ShouldCastSpell(ref Enums.Spell spellCast, ref IGameBase spellTarget)
		{
			var result = false;

			if (Spells != null)
			{
				// If enemies are present

				if (CheckNBTLHostility())
				{

				}
				else
				{

				}
			}

			return result;
		}

		public virtual bool ShouldReadyWeapon()
		{
			return true;
		}

		public virtual bool ShouldShowContentsWhenExamined()
		{
			return false;
		}

		public virtual bool ShouldShowHealthStatusWhenExamined()
		{
			return true;
		}

		public virtual bool ShouldShowHealthStatusWhenInventoried()
		{
			return true;
		}

		public virtual bool ShouldShowVerboseNameStateDesc()
		{
			return true;
		}

		public virtual bool ShouldCheckToAttackNonEnemy()
		{
			return Reaction != Friendliness.Enemy;
		}

		public virtual bool ShouldProcessInGameLoop()
		{
			var gameState = gEngine.GetGameState();

			return gameState != null && Location == gameState.Ro && !IsCharacterMonster();
		}

		public virtual bool ShouldRefuseToAcceptGold()
		{
			return !HasCarriedInventory();
		}

		public virtual bool ShouldRefuseToAcceptGift(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			return !HasCarriedInventory() || (!gEngine.IsRulesetVersion(5, 62) && (Reaction == Friendliness.Enemy || (Reaction == Friendliness.Neutral && artifact.Value < 3000)));
		}

		public virtual bool ShouldRefuseToAcceptDeadBody(IArtifact artifact)
		{
			return true;
		}

		public virtual bool ShouldPreferNaturalWeaponsToWeakerWeapon(IArtifact artifact)
		{
			return true;
		}

		public virtual bool CheckNBTLHostility()
		{
			var gameState = gEngine.GetGameState();

			return gameState != null && Reaction != Friendliness.Neutral && gameState.GetNBTL(Reaction == Friendliness.Friend ? Friendliness.Enemy : Friendliness.Friend) > 0;
		}

		public virtual bool CheckCourage()
		{
			var result = false;

			var gameState = gEngine.GetGameState();

			if (gEngine.IsRulesetVersion(5, 62) && gameState != null)
			{
				var x = gEngine.RollDice(1, 41, -21);
				
				if (gEngine.IsRulesetVersion(62) && x == 0)
				{
					x++;
				}
				
				var rl = (long)Math.Round((double)gameState.GetDTTL(Reaction, Location) / (double)gameState.GetNBTL(Reaction, Location) * 100 + x);

				result = rl <= Courage;
			}
			else
			{
				var s = (DmgTaken > 0 || GroupCount > CurrGroupCount ? 1 : 0) + (DmgTaken + 4 >= Hardiness ? 1 : 0);

				var rl = gEngine.RollDice(1, 100, s * 5);

				result = rl <= Courage;           // Courage >= 100 ||
			}

			return result;
		}

		public virtual T EvalReaction<T>(T enemyValue, T neutralValue, T friendValue)
		{
			return gEngine.EvalFriendliness(Reaction, enemyValue, neutralValue, friendValue);
		}

		public virtual T EvalGender<T>(T maleValue, T femaleValue, T neutralValue)
		{
			return gEngine.EvalGender(Gender, maleValue, femaleValue, neutralValue);
		}

		public virtual T EvalPlural<T>(T singularValue, T pluralValue)
		{
			return gEngine.EvalPlural(CurrGroupCount > 1, singularValue, pluralValue);
		}

		public virtual T EvalInRoomLightLevel<T>(T darkValue, T lightValue)
		{
			return IsInRoomLit() ? lightValue : darkValue;
		}

		public virtual T EvalInRoomViewability<T>(T nonviewableValue, T viewableValue)
		{
			return IsInRoomViewable() ? viewableValue : nonviewableValue;
		}

		public virtual void ResolveReaction(long charisma)
		{
			if (gEngine.IsValidMonsterFriendlinessPct(Friendliness))
			{
				var f = (long)Friendliness - 100;

				if (f > 0 && f < 100)
				{
					f += gEngine.GetCharismaFactor(charisma);
				}

				var k = Friendliness.Enemy;

				var rl = gEngine.RollDice(1, 100, 0);

				if (f >= rl)
				{
					k++;

					rl = gEngine.RollDice(1, 100, 0);

					if (f >= rl)
					{
						k++;
					}
				}

				Reaction = k;
			}
			else
			{
				Debug.Assert(Enum.IsDefined(typeof(Friendliness), Friendliness));

				Reaction = Friendliness;
			}
		}

		public virtual void ResolveReaction(ICharacter character)
		{
			if (character != null)
			{
				ResolveReaction(character.GetStat(Stat.Charisma));
			}
		}

		public virtual void CalculateGiftFriendliness(long value, bool isArtifactValue)
		{
			Debug.Assert(gEngine.IsRulesetVersion(5, 62));

			if (isArtifactValue)       // Scaled from EDX to original Eamon values
			{
				value *= 3;
			}

			long f = !Enum.IsDefined(typeof(Friendliness), Friendliness) ? (long)(Friendliness - 100) :
				Friendliness == Friendliness.Friend ? 100 :
				Friendliness == Friendliness.Neutral ? 50 :
				0;

			f = (long)((double)f * (1 + (double)value / 100.0));

			if (f < 0)
			{
				f = 0;
			}
			else if (f > 100)
			{
				f = 100;
			}

			if (!Enum.IsDefined(typeof(Friendliness), Friendliness))
			{
				Friendliness = (Friendliness)(f + 100);
			}
			else
			{
				Friendliness = f > 66 ? Friendliness.Friend :
					f > 33 ? Friendliness.Neutral :
					Friendliness.Enemy;
			}
		}

		public virtual bool IsCharacterMonster()
		{
			var gameState = gEngine.GetGameState();

			return gameState != null && gameState.Cm == Uid;
		}

		public virtual bool IsStateDescSideNotes()
		{
			if (!string.IsNullOrWhiteSpace(StateDesc))
			{
				var regex = new Regex(@"\(.+\)");

				return regex.IsMatch(StateDesc);
			}
			else
			{
				return false;
			}
		}

		public virtual long GetWeightCarryableGronds()
		{
			return gEngine.GetWeightCarryableGronds(Hardiness);
		}

		public virtual long GetFleeingMemberCount()
		{
			return gEngine.RollDice(1, CurrGroupCount, 0);
		}

		public virtual long GetMaxMemberActionCount()
		{
			return 5;
		}

		public virtual long GetMaxMemberAttackCount()
		{
			return 25;
		}

		public virtual IMonsterSpell GetMonsterSpell(Enums.Spell spell)
		{
			return Spells != null && Enum.IsDefined(typeof(Enums.Spell), spell) ? Spells.FirstOrDefault(ms => ms != null && ms.Spell == spell) : null;
		}

		public virtual IList<IArtifact> GetCarriedList(Func<IArtifact, bool> monsterFindFunc = null, Func<IArtifact, bool> artifactFindFunc = null, bool recurse = false)
		{
			if (monsterFindFunc == null)
			{
				monsterFindFunc = a => a.IsCarriedByMonster(this);
			}

			var artifactList = gEngine.GetArtifactList(a => monsterFindFunc(a));

			if (recurse && artifactList.Count > 0)
			{
				var artifactList01 = new List<IArtifact>();

				foreach (var a in artifactList)
				{
					if (a.GeneralContainer != null)
					{
						artifactList01.AddRange(a.GetContainedList(artifactFindFunc, (ContainerType)(-1), recurse));
					}
				}

				artifactList.AddRange(artifactList01);
			}

			return artifactList;
		}

		public virtual IList<IArtifact> GetWornList(Func<IArtifact, bool> monsterFindFunc = null, Func<IArtifact, bool> artifactFindFunc = null, bool recurse = false)
		{
			if (monsterFindFunc == null)
			{
				monsterFindFunc = a => a.IsWornByMonster(this);
			}

			var artifactList = gEngine.GetArtifactList(a => monsterFindFunc(a));

			if (recurse && artifactList.Count > 0)
			{
				var artifactList01 = new List<IArtifact>();

				foreach (var a in artifactList)
				{
					if (a.GeneralContainer != null)
					{
						artifactList01.AddRange(a.GetContainedList(artifactFindFunc, (ContainerType)(-1), recurse));
					}
				}

				artifactList.AddRange(artifactList01);
			}

			return artifactList;
		}

		public virtual IList<IArtifact> GetContainedList(Func<IArtifact, bool> monsterFindFunc = null, Func<IArtifact, bool> artifactFindFunc = null, bool recurse = false)
		{
			if (monsterFindFunc == null)
			{
				monsterFindFunc = a => a.IsCarriedByMonster(this) || a.IsWornByMonster(this);
			}

			var artifactList = gEngine.GetArtifactList(a => monsterFindFunc(a));

			if (recurse && artifactList.Count > 0)
			{
				var artifactList01 = new List<IArtifact>();

				foreach (var a in artifactList)
				{
					if (a.GeneralContainer != null)
					{
						artifactList01.AddRange(a.GetContainedList(artifactFindFunc, (ContainerType)(-1), recurse));
					}
				}

				artifactList.AddRange(artifactList01);
			}

			return artifactList;
		}

		public virtual RetCode EnforceFullInventoryWeightLimits(Func<IArtifact, bool> monsterFindFunc = null, Func<IArtifact, bool> artifactFindFunc = null, bool recurse = false)
		{
			RetCode rc;

			long c, w, mwt;

			rc = RetCode.Success;

			try
			{
				gEngine.RevealContentCounter--;

				mwt = 0;

				var artifactList = GetContainedList(monsterFindFunc, artifactFindFunc).OrderBy(a => recurse ? a.RecursiveWeight : a.Weight).ToList();

				foreach (var a in artifactList)
				{
					c = 0;

					w = a.Weight;

					Debug.Assert(!gEngine.IsUnmovable01(w));

					if (recurse && a.GeneralContainer != null)
					{
						rc = a.GetContainerInfo(ref c, ref w, (ContainerType)(-1), recurse);

						if (gEngine.IsFailure(rc))
						{
							// PrintError

							goto Cleanup;
						}
					}

					if (w <= 10 * Hardiness && mwt + w <= 10 * Hardiness * CurrGroupCount)
					{
						mwt += w;
					}
					else
					{
						a.Location = Location >= 0 ? Location : 0;

						if (Weapon == a.Uid)
						{
							a.RemoveStateDesc(a.GetReadyWeaponDesc());

							Weapon = -1;
						}
					}
				}

			Cleanup:

				;
			}
			finally
			{
				gEngine.RevealContentCounter++;
			}

			return rc;
		}

		public virtual RetCode GetFullInventoryWeight(ref long weight, Func<IArtifact, bool> monsterFindFunc = null, Func<IArtifact, bool> artifactFindFunc = null, bool recurse = false)
		{
			RetCode rc;

			rc = RetCode.Success;

			var artifactList = GetContainedList(monsterFindFunc, artifactFindFunc, recurse);

			foreach (var a in artifactList)
			{
				if (!a.IsUnmovable01())
				{
					weight += a.Weight;
				}
			}

			return rc;
		}

		public virtual void AddHealthStatus(StringBuilder buf, bool appendNewLine = true)
		{
			string result = null;

			if (buf == null)
			{
				// PrintError

				goto Cleanup;
			}

			var index = gEngine.GetMonsterHealthStatusIndex(Hardiness, DmgTaken);

			if (index > 5)
			{
				result = "dead!";
			}
			else if (index == 5)
			{
				result = "at death's door, knocking loudly.";
			}
			else if (index == 4)
			{
				result = (gEngine.IsRulesetVersion(5, 62) ? "very " : "") + "badly injured.";
			}
			else if (index == 3)
			{
				result = "in pain.";
			}
			else if (index == 2)
			{
				result = "hurting.";
			}
			else if (index == 1)
			{
				result = "still in good shape.";
			}
			else if (index < 1)
			{
				result = "in perfect health.";
			}

			Debug.Assert(result != null);

			buf.AppendFormat("{0}{1}", result, appendNewLine ? Environment.NewLine : "");

		Cleanup:

			;
		}

		public virtual string[] GetWeaponAttackDescs(IArtifact artifact)
		{
			Debug.Assert(artifact != null && artifact.GeneralWeapon != null && Enum.IsDefined(typeof(Enums.Weapon), artifact.GeneralWeapon.Field2));

			string[] attackDescs = null;

			switch ((Enums.Weapon)artifact.GeneralWeapon.Field2)
			{
				case Enums.Weapon.Axe:

					attackDescs = new string[] { "swing{0} at", "chop{0} at", "swing{0} at" };

					break;

				case Enums.Weapon.Bow:

					attackDescs = new string[] { "shoot{0} at", "shoot{0} at", "shoot{0} at" };

					break;

				case Enums.Weapon.Club:

					attackDescs = new string[] { "swing{0} at", "swing{0} at", "swing{0} at" };

					break;

				case Enums.Weapon.Spear:

					attackDescs = new string[] { "stab{0} at", "lunge{0} at", "jab{0} at" };

					break;

				case Enums.Weapon.Sword:

					attackDescs = new string[] { "swing{0} at", "chop{0} at", "stab{0} at" };

					break;
			}

			return attackDescs;
		}

		public virtual string[] GetHumanAttackDescs()
		{
			var attackDescs = new string[] { "charge{0} at", "punche{0} at", "kick{0} at" };

			return attackDescs;
		}

		public virtual string[] GetNaturalAttackDescs()
		{
			var attackDescs = new string[] { "lunge{0} at", "tear{0} at", "claw{0} at" };

			return attackDescs;
		}

		public virtual string GetAttackDescString(IRoom room, IArtifact artifact)
		{
			Debug.Assert(room != null);

			var attackDesc = "attack{0}";

			if (IsCharacterMonster() || (room.IsViewable() && CombatCode != CombatCode.Attacks && CombatCode != CombatCode.NaturalAttacks))
			{
				var attackDescs = artifact != null ? GetWeaponAttackDescs(artifact) : HasHumanNaturalAttackDescs() ? GetHumanAttackDescs() : GetNaturalAttackDescs();

				Debug.Assert(attackDescs != null && attackDescs.Length > 0);

				var rl = gEngine.RollDice(1, attackDescs.Length, -1);

				attackDesc = attackDescs[rl];
			}

			attackDesc = string.Format(attackDesc, IsCharacterMonster() ? "" : "s");

			return attackDesc;
		}

		public virtual string[] GetWeaponMissDescs(IArtifact artifact)
		{
			Debug.Assert(artifact != null && artifact.GeneralWeapon != null && Enum.IsDefined(typeof(Enums.Weapon), artifact.GeneralWeapon.Field2));

			string[] missDescs = null;

			switch ((Enums.Weapon)artifact.GeneralWeapon.Field2)
			{
				case Enums.Weapon.Axe:
				case Enums.Weapon.Club:
				case Enums.Weapon.Spear:

					missDescs = new string[] { "Dodged", "Missed" };

					break;

				case Enums.Weapon.Bow:

					missDescs = new string[] { "Missed", "Missed" };

					break;

				case Enums.Weapon.Sword:

					missDescs = new string[] { "Parried", "Missed" };

					break;
			}

			return missDescs;
		}

		public virtual string[] GetNaturalMissDescs()
		{
			var missDescs = new string[] { "Missed", "Missed" };

			return missDescs;
		}

		public virtual string GetMissDescString(IArtifact artifact)
		{
			var missDescs = artifact != null ? GetWeaponMissDescs(artifact) : GetNaturalMissDescs();

			Debug.Assert(missDescs != null && missDescs.Length > 0);

			var rl = gEngine.RollDice(1, missDescs.Length, -1);

			return missDescs[rl];
		}

		public virtual string GetArmorDescString()
		{
			return "armor";
		}

		public virtual string GetCantFindExitDescString(IRoom room, string monsterName, bool isPlural, bool fleeing)
		{
			string result;

			Debug.Assert(room != null && !string.IsNullOrWhiteSpace(monsterName));

			if (fleeing)
			{
				result = string.Format("{0} {1} to flee, but can't find {2}!", monsterName, isPlural ? "try" : "tries", room.EvalRoomType("an exit", "a path"));
			}
			else
			{
				result = string.Format("{0} {1} to leave, but can't find {2}.", monsterName, isPlural ? "try" : "tries", room.EvalRoomType("an exit", "a path"));
			}

			return result;
		}

		public virtual string GetMembersExitRoomDescString(IRoom room, string monsterName, bool isPlural, bool fleeing)
		{
			string result;

			Debug.Assert(room != null && !string.IsNullOrWhiteSpace(monsterName));

			if (fleeing)
			{
				result = string.Format("{0} {1}!", monsterName, isPlural ? "flee" : "flees");
			}
			else
			{
				result = string.Format("{0} {1}.", monsterName, isPlural ? "leave" : "leaves");
			}

			return result;
		}

		public virtual string GetExitRoomDescString(IRoom room, string monsterName, bool isPlural, bool fleeing, Direction exitDirection)
		{
			string result;

			Debug.Assert(room != null && !string.IsNullOrWhiteSpace(monsterName) && Enum.IsDefined(typeof(Direction), exitDirection));

			if (fleeing)
			{
				result = string.Format("{0} {1}{2}!", monsterName, isPlural ? "flee" : "flees", exitDirection == Direction.Up ? " upward" : exitDirection == Direction.Down ? " downward" : exitDirection == Direction.In ? " inside" : exitDirection == Direction.Out ? " outside" : string.Format(" to the {0}", exitDirection.ToString().ToLower()));
			}
			else
			{
				result = string.Format("{0} {1}{2}.", monsterName, isPlural ? "go" : "goes", exitDirection == Direction.Up ? " upward" : exitDirection == Direction.Down ? " downward" : exitDirection == Direction.In ? " inside" : exitDirection == Direction.Out ? " outside" : string.Format(" to the {0}", exitDirection.ToString().ToLower()));
			}

			return result;
		}

		public virtual string GetEnterRoomDescString(IRoom room, string monsterName, bool isPlural, bool fleeing, Direction enterDirection)
		{
			string result;

			Debug.Assert(room != null && !string.IsNullOrWhiteSpace(monsterName) && Enum.IsDefined(typeof(Direction), enterDirection));

			if (fleeing)
			{
				result = string.Format("{0} {1} {2} from{3}!", monsterName, isPlural ? "flee" : "flees", enterDirection == Direction.In ? "out" : "in", enterDirection == Direction.Up ? " above" : enterDirection == Direction.Down ? " below" : enterDirection == Direction.In ? " inside" : enterDirection == Direction.Out ? " outside" : string.Format(" the {0}", enterDirection.ToString().ToLower()));
			}
			else
			{
				result = string.Format("{0} {1} from{2}.", monsterName, isPlural ? "arrive" : "arrives", enterDirection == Direction.Up ? " above" : enterDirection == Direction.Down ? " below" : enterDirection == Direction.In ? " inside" : enterDirection == Direction.Out ? " outside" : string.Format(" the {0}", enterDirection.ToString().ToLower()));
			}

			return result;
		}

		public virtual string GetPovString(string youString, string maleString, string femaleString, string neutralString, string groupString)
		{
			Debug.Assert(youString != null && maleString != null && femaleString != null && neutralString != null);

			return IsCharacterMonster() ? youString : CurrGroupCount > 1 && groupString != null ? groupString : EvalGender(maleString, femaleString, neutralString);
		}

		#endregion

		#region Class Monster

		public Monster()
		{
			StateDesc = "";
		}

		#endregion

		#endregion
	}
}
