﻿
// StatusCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Args;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.Globals;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class StatusCommand : Command, IStatusCommand
	{
		public long _charInventoryWeight;

		/// <summary></summary>
		public virtual long[] ArmorArtifactUids { get; set; }

		/// <summary></summary>
		public virtual IStatDisplayArgs StatDisplayArgs { get; set; }

		/// <summary></summary>
		public virtual IArtifactCategory ArmorArtifactAc { get; set; }

		/// <summary></summary>
		public virtual IArtifact ArmorArtifact { get; set; }

		/// <summary></summary>
		public virtual IArmor CharArmor { get; set; }

		/// <summary></summary>
		public virtual Armor CharArmorClass { get; set; }

		/// <summary></summary>
		public virtual long CharInventoryWeight
		{
			get
			{
				return _charInventoryWeight;
			}

			set
			{
				_charInventoryWeight = value;
			}
		}

		public override void ExecuteForPlayer()
		{
			RetCode rc;
			
			gEngine.ShouldPreTurnProcess = false;

			ArmorArtifactUids = new long[] { gGameState.Ar, gGameState.Sh };

			CharArmorClass = Armor.SkinClothes;

			foreach (var armorArtifactUid in ArmorArtifactUids)
			{
				if (armorArtifactUid > 0)
				{
					ArmorArtifact = gADB[armorArtifactUid];

					Debug.Assert(ArmorArtifact != null);

					ArmorArtifactAc = ArmorArtifact.Wearable;

					Debug.Assert(ArmorArtifactAc != null);

					CharArmorClass += ArmorArtifactAc.Field1;
				}
			}

			CharArmor = gEngine.GetArmor(CharArmorClass);

			Debug.Assert(CharArmor != null);
			
			gEngine.Buf.SetFormat("{0}", CharArmor.Name);

			CharInventoryWeight = 0;

			rc = ActorMonster.GetFullInventoryWeight(ref _charInventoryWeight, recurse: true);

			Debug.Assert(gEngine.IsSuccess(rc));

			StatDisplayArgs = gEngine.CreateInstance<IStatDisplayArgs>(x =>
			{
				x.Character = gCharacter;
				x.Monster = ActorMonster;
				x.ArmorString = gEngine.Buf.ToString();
				x.SpellAbilities = gGameState.Sa;
				x.Speed = gGameState.Speed;
				x.CharmMon = gEngine.GetCharismaFactor(gCharacter.GetStat(Stat.Charisma));
				x.Weight = CharInventoryWeight;
			});

			rc = gEngine.StatDisplay(StatDisplayArgs);

			Debug.Assert(gEngine.IsSuccess(rc));

			ProcessEvents(EventType.AfterPrintPlayerStatus);

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IStartState>();
			}
		}

		public StatusCommand()
		{
			SortOrder = 370;

			IsSentenceParserEnabled = false;

			IsDarkEnabled = true;

			if (gEngine.IsRulesetVersion(5, 62))
			{
				IsPlayerEnabled = false;
			}

			Name = "StatusCommand";

			Verb = "status";

			Type = CommandType.Miscellaneous;
		}
	}
}
