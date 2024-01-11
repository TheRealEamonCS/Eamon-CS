
// GameState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Utilities;
using static Eamon.Game.Plugin.Globals;

namespace Eamon.Game
{
	[ClassMappings]
	public class GameState : GameBase, IGameState
	{
		#region Public Properties

		#region Interface IGameState

		[FieldName(620)]
		public virtual long Ar { get; set; }

		[FieldName(640)]
		public virtual long Cm { get; set; }

		[FieldName(660)]
		public virtual long Ls { get; set; }

		[FieldName(680)]
		public virtual long Ro { get; set; }

		[FieldName(700)]
		public virtual long R2 { get; set; }

		[FieldName(720)]
		public virtual long R3 { get; set; }

		[FieldName(740)]
		public virtual long Sh { get; set; }

		[FieldName(760)]
		public virtual long Af { get; set; }

		[FieldName(780)]
		public virtual long Die { get; set; }

		[FieldName(800)]
		public virtual long Speed { get; set; }

		[FieldName(820)]
		public virtual bool Vr { get; set; }

		[FieldName(840)]
		public virtual bool Vm { get; set; }

		[FieldName(860)]
		public virtual bool Va { get; set; }

		[FieldName(870)]
		public virtual bool Vn { get; set; }

		[FieldName(880)]
		public virtual bool MatureContent { get; set; }

		[FieldName(890)]
		public virtual bool InteractiveFiction { get; set; }

		[FieldName(900)]
		public virtual bool EnhancedParser { get; set; }

		[FieldName(910)]
		public virtual bool IobjPronounAffinity { get; set; }

		[FieldName(920)]
		public virtual bool ShowPronounChanges { get; set; }

		[FieldName(940)]
		public virtual bool ShowFulfillMessages { get; set; }

		[FieldName(960)]
		public virtual long CurrTurn { get; set; }

		[FieldName(980)]
		public virtual long PauseCombatMs { get; set; }

		[FieldName(990)]
		public virtual long PauseCombatActions { get; set; }

		[FieldName(1000)]
		public virtual long ImportedArtUidsIdx { get; set; }

		[FieldName(1020)]
		public virtual long UsedWpnIdx { get; set; }

		[FieldName(1040)]
		public virtual long[] Sa { get; set; }

		[FieldName(1060)]
		public virtual long[] ImportedArtUids { get; set; }

		[FieldName(1080)]
		public virtual long[] HeldWpnUids { get; set; }

		[FieldName(1100)]
		public virtual EventHeap BeforePrintPlayerRoomEventHeap { get; set; }

		[FieldName(1120)]
		public virtual EventHeap AfterPrintPlayerRoomEventHeap { get; set; }

		#endregion

		#endregion

		#region Public Methods

		#region Interface IDisposable

		public override void Dispose(bool disposing)
		{
			if (disposing)
			{
				// get rid of managed resources
			}

			if (IsUidRecycled && Uid > 0)
			{
				gDatabase.FreeGameStateUid(Uid);

				Uid = 0;
			}
		}

		#endregion

		#region Interface IComparable

		public virtual int CompareTo(IGameState gameState)
		{
			return this.Uid.CompareTo(gameState.Uid);
		}

		#endregion

		#region Interface IGameState

		public virtual long GetNBTL(long index, long roomUid = 0)
		{
			var nbtl = 0L;

			var monsterList = gEngine.GetMonsterList(m => m.IsInRoomUid(roomUid > 0 ? roomUid : Ro) && m.Reaction == (Friendliness)index);

			foreach (var monster in monsterList)
			{
				nbtl += (monster.Hardiness * monster.CurrGroupCount);
			}

			Debug.Assert(nbtl >= 0);

			return nbtl;
		}

		public virtual long GetNBTL(Friendliness friendliness, long roomUid = 0)
		{
			return GetNBTL((long)friendliness, roomUid);
		}

		public virtual long GetDTTL(long index, long roomUid = 0)
		{
			var dttl = 0L;

			var monsterList = gEngine.IsRulesetVersion(5, 62) ? gEngine.GetMonsterList(m => m.IsInRoomUid(roomUid > 0 ? roomUid : Ro) && m.Reaction == (Friendliness)index) : new List<IMonster>();

			foreach (var monster in monsterList)
			{
				dttl += monster.DmgTaken;
			}

			Debug.Assert(dttl >= 0);

			return dttl;
		}

		public virtual long GetDTTL(Friendliness friendliness, long roomUid = 0)
		{
			return GetDTTL((long)friendliness, roomUid);
		}

		public virtual long GetSa(long index)
		{
			return Sa[index];
		}

		public virtual long GetSa(Spell spell)
		{
			return GetSa((long)spell);
		}

		public virtual long GetImportedArtUid(long index)
		{
			return ImportedArtUids[index];
		}

		public virtual long GetHeldWpnUid(long index)
		{
			return HeldWpnUids[index];
		}

		public virtual void SetSa(long index, long value)
		{
			Sa[index] = value;
		}

		public virtual void SetSa(Spell spell, long value)
		{
			SetSa((long)spell, value);
		}

		public virtual void SetImportedArtUid(long index, long value)
		{
			ImportedArtUids[index] = value;
		}

		public virtual void SetHeldWpnUid(long index, long value)
		{
			HeldWpnUids[index] = value;
		}

		public virtual void ModSa(long index, long value)
		{
			Sa[index] += value;
		}

		public virtual void ModSa(Spell spell, long value)
		{
			ModSa((long)spell, value);
		}

		#endregion

		#region Class GameState

		public GameState()
		{
			var character = gEngine.CreateInstance<ICharacter>();

			Debug.Assert(character != null);

			EnhancedParser = !gEngine.IsRulesetVersion(5);

			Sa = new long[character.SpellAbilities.Length];

			ImportedArtUids = new long[character.Weapons.Length + 2];

			HeldWpnUids = new long[character.Weapons.Length];

			BeforePrintPlayerRoomEventHeap = new EventHeap();

			AfterPrintPlayerRoomEventHeap = new EventHeap();
		}

		#endregion

		#endregion
	}
}
