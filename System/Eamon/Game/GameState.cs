
// GameState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Utilities;
using static Eamon.Game.Plugin.PluginContext;

namespace Eamon.Game
{
	[ClassMappings]
	public class GameState : GameBase, IGameState
	{
		#region Public Properties

		#region Interface IGameState

		public virtual long Ar { get; set; }

		public virtual long Cm { get; set; }

		public virtual long Ls { get; set; }

		public virtual long Ro { get; set; }

		public virtual long R2 { get; set; }

		public virtual long R3 { get; set; }

		public virtual long Sh { get; set; }

		public virtual long Af { get; set; }

		public virtual long Die { get; set; }

		public virtual long Speed { get; set; }

		public virtual bool Vr { get; set; }

		public virtual bool Vm { get; set; }

		public virtual bool Va { get; set; }

		public virtual bool MatureContent { get; set; }

		public virtual bool EnhancedParser { get; set; }

		public virtual bool ShowPronounChanges { get; set; }

		public virtual bool ShowFulfillMessages { get; set; }

		public virtual long CurrTurn { get; set; }

		public virtual long PauseCombatMs { get; set; }

		public virtual long ImportedArtUidsIdx { get; set; }

		public virtual long UsedWpnIdx { get; set; }

		public virtual long[] Sa { get; set; }

		public virtual long[] ImportedArtUids { get; set; }

		public virtual long[] HeldWpnUids { get; set; }

		public virtual EventHeap BeforePrintPlayerRoomEventHeap { get; set; }

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
				Globals.Database.FreeGameStateUid(Uid);

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

		public virtual long GetNBTL(long index)
		{
			var nbtl = 0L;

			var monsterList = gEngine != null ? gEngine.GetMonsterList(m => m.Location == Ro && m.Reaction == (Friendliness)index) : new List<IMonster>();

			foreach (var monster in monsterList)
			{
				nbtl += (monster.Hardiness * monster.CurrGroupCount);
			}

			Debug.Assert(nbtl >= 0);

			return nbtl;
		}

		public virtual long GetNBTL(Friendliness friendliness)
		{
			return GetNBTL((long)friendliness);
		}

		public virtual long GetDTTL(long index)
		{
			var dttl = 0L;

			var monsterList = Globals.IsRulesetVersion(5) && gEngine != null ? gEngine.GetMonsterList(m => m.Location == Ro && m.Reaction == (Friendliness)index) : new List<IMonster>();

			foreach (var monster in monsterList)
			{
				dttl += monster.DmgTaken;
			}

			Debug.Assert(dttl >= 0);

			return dttl;
		}

		public virtual long GetDTTL(Friendliness friendliness)
		{
			return GetDTTL((long)friendliness);
		}

		public virtual long GetSa(long index)
		{
			return Sa[index];
		}

		public virtual long GetSa(Spell spell)
		{
			return GetSa((long)spell);
		}

		public virtual long GetImportedArtUids(long index)
		{
			return ImportedArtUids[index];
		}

		public virtual long GetHeldWpnUids(long index)
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

		public virtual void SetImportedArtUids(long index, long value)
		{
			ImportedArtUids[index] = value;
		}

		public virtual void SetHeldWpnUids(long index, long value)
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
			var character = Globals.CreateInstance<ICharacter>();

			Debug.Assert(character != null);

			EnhancedParser = !Globals.IsRulesetVersion(5);

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
