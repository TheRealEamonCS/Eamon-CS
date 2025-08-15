
// GameState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Game.Attributes;
using static Firestorm.Game.Plugin.Globals;

namespace Firestorm.Game
{
	[ClassMappings(typeof(IGameState))]
	public class GameState : Eamon.Game.GameState, Framework.IGameState
	{
		[FieldName(1121)]
		public virtual long[] OS { get; set; }

		[FieldName(1122)]
		public virtual long[] PG { get; set; }

		[FieldName(1123)]
		public virtual long MP { get; set; }

		[FieldName(1124)]
		public virtual long ST { get; set; }

		[FieldName(1125)]
		public virtual long GH { get; set; }

		[FieldName(1126)]
		public virtual long PZ { get; set; }

		[FieldName(1127)]
		public virtual long RZ { get; set; }

		[FieldName(1128)]
		public virtual long MY { get; set; }

		[FieldName(1129)]
		public virtual long NF { get; set; }

		public virtual bool MPEnabled { get; set; }

		public virtual bool DeathStory { get; set; }

		public virtual long GetOS(long index)
		{
			return OS[index];
		}

		public virtual void SetOS(long index, long value)
		{
			OS[index] = value;
		}

		public virtual long GetPG(long index)
		{
			return PG[index];
		}

		public virtual void SetPG(long index, long value)
		{
			PG[index] = value;
		}

		public GameState()
		{
			OS = new long[3];

			PG = new long[10];
		}
	}
}
