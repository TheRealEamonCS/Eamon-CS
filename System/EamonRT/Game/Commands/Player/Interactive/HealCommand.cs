
// HealCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
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
				if (Globals.IsRulesetVersion(5, 15, 25))
				{
					Globals.Buf.SetFormat("{0}Some of {1}", 
						Environment.NewLine,
						IsCharMonster ? "your" :
						DobjMonster.EvalPlural(DobjMonster.GetTheName(buf: Globals.Buf01),
														DobjMonster.GetArticleName(false, true, false, true, Globals.Buf02)));
				}
				else
				{
					Globals.Buf.SetFormat("{0}{1}",
						Environment.NewLine,
						IsCharMonster ? "Your" :
						DobjMonster.EvalPlural(DobjMonster.GetTheName(true, buf: Globals.Buf01),
														DobjMonster.GetArticleName(true, true, false, true, Globals.Buf02)));
				}

				if (!IsCharMonster)
				{
					gEngine.GetPossessiveName(Globals.Buf);
				}

				if (Globals.IsRulesetVersion(5, 15, 25))
				{
					Globals.Buf.AppendFormat(" wounds seem to clear up.{0}", Environment.NewLine);
				}
				else
				{
					Globals.Buf.AppendFormat(" health improves!{0}", Environment.NewLine);
				}

				gOut.Write("{0}", Globals.Buf);

				DamageHealed = gEngine.RollDice(1, Globals.IsRulesetVersion(5, 15, 25) ? 10 : 12, 0);

				DobjMonster.DmgTaken -= DamageHealed;
			}

			if (DobjMonster.DmgTaken < 0)
			{
				DobjMonster.DmgTaken = 0;
			}

			Globals.Buf.SetFormat("{0}{1} {2} ",
				Environment.NewLine,
				IsCharMonster ? "You" : DobjMonster.GetTheName(true, true, false, true, Globals.Buf01),
				IsCharMonster ? "are" : "is");

			DobjMonster.AddHealthStatus(Globals.Buf);

			gOut.Write("{0}", Globals.Buf);

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
