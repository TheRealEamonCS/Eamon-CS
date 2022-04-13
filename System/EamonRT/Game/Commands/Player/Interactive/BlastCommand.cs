
// BlastCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Components;
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

		/// <summary></summary>
		public virtual IMagicComponent MagicComponent { get; set; }

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

				MagicComponent = Globals.CreateInstance<IMagicComponent>(x =>
				{
					x.SetNextStateFunc = s => NextState = s;

					x.CopyCommandDataFunc = c => CopyCommandData(c);

					x.ActorMonster = ActorMonster;

					x.ActorRoom = ActorRoom;

					x.Dobj = DobjMonster;

					x.OmitSkillGains = !ShouldAllowSkillGains();

					x.CastSpell = CastSpell;
				});

				MagicComponent.ExecuteBlastSpell();

				goto Cleanup;
			}

			ProcessEvents(EventType.BeforeAttackArtifact);

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			DobjArtAc = null;

			if (!DobjArtifact.IsAttackable(ref _dobjArtAc))
			{
				PrintWhyAttack(DobjArtifact);

				NextState = Globals.CreateInstance<IStartState>();

				goto Cleanup;
			}

			Debug.Assert(DobjArtAc != null);

			MagicComponent = Globals.CreateInstance<IMagicComponent>(x =>
			{
				x.SetNextStateFunc = s => NextState = s;

				x.CopyCommandDataFunc = c => CopyCommandData(c);

				x.ActorMonster = ActorMonster;

				x.ActorRoom = ActorRoom;

				x.Dobj = DobjArtifact;

				x.DobjArtAc = DobjArtAc;

				x.OmitSkillGains = !ShouldAllowSkillGains();

				x.CastSpell = CastSpell;
			});

			MagicComponent.ExecuteBlastSpell();

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		public override bool ShouldAllowSkillGains()
		{
			return DobjMonster != null || DobjArtifact.ShouldAllowBlastSkillGains();
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
