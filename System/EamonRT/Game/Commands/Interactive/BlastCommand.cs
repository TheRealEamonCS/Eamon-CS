
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
using static EamonRT.Game.Plugin.Globals;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class BlastCommand : Command, IBlastCommand
	{
		public IArtifactCategory _dobjArtAc;

		public virtual bool CastSpell { get; set; }

		public virtual bool CheckAttack { get; set; }

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

		public override void ExecuteForPlayer()
		{
			RetCode rc;

			Debug.Assert(DobjArtifact != null || DobjMonster != null);

			if (gGameState.GetSa(Spell.Blast) <= 0 && gCharacter.GetSpellAbility(Spell.Blast) <= 0)
			{
				PrintNothingHappens();

				if (DobjMonster != null)
				{
					gEngine.PauseCombat();
				}

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

					NextState = gEngine.CreateInstance<IStartState>();

					goto Cleanup;
				}

				if (DobjMonster.Reaction != Friendliness.Enemy)
				{
					PrintAttackNonEnemy();

					gEngine.Buf.Clear();

					rc = gEngine.In.ReadField(gEngine.Buf, gEngine.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, null);

					Debug.Assert(gEngine.IsSuccess(rc));

					if (gEngine.Buf.Length == 0 || gEngine.Buf[0] == 'N')
					{
						NextState = gEngine.CreateInstance<IStartState>();

						goto Cleanup;
					}
				}

				ProcessEvents(EventType.AfterAttackNonEnemyCheck);

				if (GotoCleanup)
				{
					goto Cleanup;
				}

				MagicComponent = gEngine.CreateInstance<IMagicComponent>(x =>
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

				if (NextState is IAttackCommand)
				{
					gEngine.ActionListCounter++;
				}

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

				NextState = gEngine.CreateInstance<IStartState>();

				goto Cleanup;
			}

			Debug.Assert(DobjArtAc != null);

			MagicComponent = gEngine.CreateInstance<IMagicComponent>(x =>
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

			if (NextState is IAttackCommand)
			{
				gEngine.ActionListCounter++;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
		}

		public override void ExecuteForMonster()
		{

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IErrorState>(x =>
				{
					x.ErrorMessage = string.Format("{0}: NextState == null", Name);
				});
			}
		}

		public override bool ShouldAllowSkillGains()
		{
			return DobjMonster != null || DobjArtifact.ShouldAllowBlastSkillGains();
		}

		public BlastCommand()
		{
			SortOrder = 260;

			IsMonsterEnabled = true;

			Name = "BlastCommand";

			Verb = "blast";

			Type = CommandType.Interactive;

			CastSpell = true;
		}
	}
}
