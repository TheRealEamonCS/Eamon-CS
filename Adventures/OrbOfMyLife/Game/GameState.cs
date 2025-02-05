
// GameState.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Game.Attributes;
using Polenter.Serialization;
using static OrbOfMyLife.Game.Plugin.Globals;

namespace OrbOfMyLife.Game
{
	[ClassMappings(typeof(IGameState))]
	public class GameState : Eamon.Game.GameState, Framework.IGameState
	{
		[FieldName(1121)]
		public virtual string MW { get; set; }

		[FieldName(1122)]
		public virtual string TW { get; set; }

		[FieldName(1123)]
		public virtual string CW { get; set; }

		[FieldName(1124)]
		public virtual long GOL { get; set; }

		[FieldName(1125)]
		public virtual long SCR { get; set; }

		[FieldName(1126)]
		public virtual long IS { get; set; }

		[FieldName(1127)]
		public virtual long VC { get; set; }

		[FieldName(1128)]
		public virtual long FC { get; set; }

		[FieldName(1129)]
		public virtual long TC { get; set; }

		[FieldName(1130)]
		public virtual long SR { get; set; }

		public virtual bool ICV { get; set; }

		public virtual bool FL { get; set; }

		public virtual bool RC { get; set; }

		[ExcludeFromSerialization]
		public virtual bool IC
		{
			get
			{
				var result = false;

				if (gEngine.EnableMutateProperties)
				{
					var room = gRDB != null ? gRDB[Ro] : null;

					if (room != null && room.IsLit())
					{
						result = ICV;
					}
				}

				return result;
			}
		}

		[ExcludeFromSerialization]
		public virtual bool IV 
		{ 
			get
			{
				var result = false;

				if (gEngine.EnableMutateProperties)
				{
					var cloakOfDarknessArtifact = gADB != null ? gADB[11] : null;

					if (cloakOfDarknessArtifact != null && gCharMonster != null)
					{
						result = cloakOfDarknessArtifact.IsWornByMonster(gCharMonster);
					}
				}

				return result;
			}
		}

		public GameState()
		{
			var mwArray = new string[]
			{
				"EVANTKE",
				"ZABATA",
				"VORIKA",
				"HAXAM",
				"FARZAK"
			};

			var twArray = new string[]
			{
				"VORAKA MEDE",
				"HOBORO WAN",
				"VESHEBA KALA",
				"SHAMASA VORIK"
			};

			var cwArray = new string[]
			{
				"SHUMAT CODO",
				"HORAZIK VIN",
				"ERADA VALA",
				"GORIBI PANE"
			};

			// Select magic words

			MW = gEngine.GetRandomElement(mwArray);

			TW = gEngine.GetRandomElement(twArray);

			CW = gEngine.GetRandomElement(cwArray);
		}
	}
}
