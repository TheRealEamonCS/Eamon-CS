
// BlastCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon;
using Eamon.Framework.Primitive.Classes;
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
		public IArtifactCategory _dobjArtAc;

		public virtual bool CastSpell { get; set; }

		/// <summary></summary>
		public virtual IArtifactCategory DobjArtAc
		{
			get
			{
				return _dobjArtAc;
			}

			set
			{
				_dobjArtAc = value;
			}
		}

		public override void Execute()
		{
			RetCode rc;

			Debug.Assert(DobjArtifact != null || DobjMonster != null);

			if (gGameState.GetSa(Spell.Blast) <= 0 && gCharacter.GetSpellAbilities(Spell.Blast) <= 0)
			{
				PrintNothingHappens();

				goto Cleanup;
			}

			if (DobjMonster != null)
			{
				ProcessEvents(EventType.BeforeAttackMonster);

				if (GotoCleanup)
				{
					goto Cleanup;
				}

				if (!DobjMonster.IsAttackable(ActorMonster))
				{
					PrintWhyAttack(DobjMonster);

					NextState = Globals.CreateInstance<IStartState>();

					goto Cleanup;
				}

				if (DobjMonster.Reaction != Friendliness.Enemy)
				{
					PrintAttackNonEnemy();

					Globals.Buf.Clear();

					rc = Globals.In.ReadField(Globals.Buf, Constants.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, null);

					Debug.Assert(gEngine.IsSuccess(rc));

					if (Globals.Buf.Length == 0 || Globals.Buf[0] == 'N')
					{
						NextState = Globals.CreateInstance<IStartState>();

						goto Cleanup;
					}
				}

				ProcessEvents(EventType.AfterAttackNonEnemyCheck);

				if (GotoCleanup)
				{
					goto Cleanup;
				}

				if (CastSpell && !gEngine.CheckPlayerSpellCast(Spell.Blast, ShouldAllowSkillGains()))
				{
					goto Cleanup;
				}

				ProcessEvents(EventType.AfterCastSpellCheck);

				if (GotoCleanup)
				{
					goto Cleanup;
				}

				if (DobjMonster.Reaction != Friendliness.Enemy)
				{
					gEngine.MonsterGetsAggravated(DobjMonster);
				}

				ProcessEvents(EventType.AfterAggravateMonster);

				if (GotoCleanup)
				{
					goto Cleanup;
				}

				NextState = Globals.CreateInstance<IAttackCommand>(x =>
				{
					x.BlastSpell = true;
				});

				CopyCommandData(NextState as ICommand);

				goto Cleanup;
			}

			ProcessEvents(EventType.BeforeAttackArtifact);

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			DobjArtAc = null;

			if (!DobjArtifact.IsAttackable01(ref _dobjArtAc))
			{
				PrintWhyAttack(DobjArtifact);

				NextState = Globals.CreateInstance<IStartState>();

				goto Cleanup;
			}

			Debug.Assert(DobjArtAc != null);

			if (CastSpell && !gEngine.CheckPlayerSpellCast(Spell.Blast, ShouldAllowSkillGains()))
			{
				goto Cleanup;
			}

			ProcessEvents(EventType.AfterCastSpellCheck);

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			NextState = Globals.CreateInstance<IAttackCommand>(x =>
			{
				x.BlastSpell = true;
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
