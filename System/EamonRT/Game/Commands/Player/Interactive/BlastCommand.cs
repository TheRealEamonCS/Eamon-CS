
// BlastCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class BlastCommand : Command, IBlastCommand
	{
		public virtual bool CastSpell { get; set; }

		public virtual bool CheckAttack { get; set; }

		public override void Execute()
		{
			RetCode rc;

			Debug.Assert(DobjArtifact != null || DobjMonster != null);

			if (!CheckAttack && DobjMonster != null && DobjMonster.Reaction != Friendliness.Enemy)
			{
				gOut.Write("{0}Attack non-enemy (Y/N): ", Environment.NewLine);

				Globals.Buf.Clear();

				rc = Globals.In.ReadField(Globals.Buf, Constants.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				if (Globals.Buf.Length == 0 || Globals.Buf[0] == 'N')
				{
					NextState = Globals.CreateInstance<IStartState>();

					goto Cleanup;
				}

				CheckAttack = true;
			}

			if (CastSpell && !gEngine.CheckPlayerSpellCast(Spell.Blast, ShouldAllowSkillGains()))
			{
				goto Cleanup;
			}

			ProcessEvents(EventType.AfterPlayerSpellCastCheck);

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			if (DobjMonster != null && DobjMonster.Reaction != Friendliness.Enemy)
			{
				gEngine.MonsterGetsAggravated(DobjMonster);
			}

			ProcessEvents(EventType.AfterMonsterGetsAggravated);

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			NextState = Globals.CreateInstance<IAttackCommand>(x =>
			{
				x.BlastSpell = true;

				x.CheckAttack = CheckAttack;
			});

			CopyCommandData(NextState as ICommand);

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		public override bool ShouldAllowSkillGains()
		{
			return DobjMonster != null || DobjArtifact.IsAttackable();
		}

		public BlastCommand()
		{
			SortOrder = 260;

			Uid = 35;

			Name = "BlastCommand";

			Verb = "blast";

			Type = CommandType.Interactive;

			CastSpell = true;
		}
	}
}
