
// GameState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Game.Attributes;
using static ThePyramidOfAnharos.Game.Plugin.Globals;

namespace ThePyramidOfAnharos.Game
{
	[ClassMappings(typeof(IGameState))]
	public class GameState : Eamon.Game.GameState, Framework.IGameState
	{
		[FieldName(1121)]
		public virtual long GD { get; set; }

		[FieldName(1122)]
		public virtual long GU { get; set; }

		[FieldName(1123)]
		public virtual long KE { get; set; }

		[FieldName(1124)]
		public virtual long KF { get; set; }

		[FieldName(1125)]
		public virtual long KG { get; set; }

		[FieldName(1126)]
		public virtual long KH { get; set; }

		[FieldName(1127)]
		public virtual long KL { get; set; }

		[FieldName(1128)]
		public virtual long KN { get; set; }

		[FieldName(1129)]
		public virtual long KO { get; set; }

		[FieldName(1130)]
		public virtual long KP { get; set; }

		[FieldName(1131)]
		public virtual long KQ { get; set; }

		[FieldName(1132)]
		public virtual long KR { get; set; }

		[FieldName(1133)]
		public virtual long KS { get; set; }

		[FieldName(1134)]
		public virtual long KT { get; set; }

		[FieldName(1135)]
		public virtual long KU { get; set; }

		[FieldName(1136)]
		public virtual long KV { get; set; }

		[FieldName(1137)]
		public virtual long KW { get; set; }

		public GameState()
		{
			KW = 200;
		}
	}
}
