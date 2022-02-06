
// HealCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class HealCommand : Command, IHealCommand
	{
		public virtual bool CastSpell { get; set; }

		/// <summary></summary>
		public virtual bool IsCharMonster { get; set; }

		/// <summary></summary>
		public virtual long DamageHealed { get; set; }

		public override void Execute()
		{
			Debug.Assert(DobjMonster != null);

			if (CastSpell && !gEngine.CheckPlayerSpellCast(Spell.Heal, ShouldAllowSkillGains()))
			{
				goto Cleanup;
			}

			IsCharMonster = DobjMonster.IsCharacterMonster();

			if (DobjMonster.DmgTaken > 0)
			{
				PrintHealthImproves(DobjMonster);

				DamageHealed = gEngine.RollDice(1, Globals.IsRulesetVersion(5, 15, 25) ? 10 : 12, 0);

				DobjMonster.DmgTaken -= DamageHealed;
			}

			if (DobjMonster.DmgTaken < 0)
			{
				DobjMonster.DmgTaken = 0;
			}

			PrintHealthStatus(DobjMonster, false);

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		public HealCommand()
		{
			SortOrder = 290;

			Uid = 38;

			Name = "HealCommand";

			Verb = "heal";

			Type = CommandType.Interactive;

			CastSpell = true;
		}
	}
}
