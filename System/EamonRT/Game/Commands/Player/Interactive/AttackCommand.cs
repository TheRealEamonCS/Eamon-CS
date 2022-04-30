
// AttackCommand.cs

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
	public class AttackCommand : Command, IAttackCommand
	{
		public IArtifactCategory _dobjArtAc;

		public virtual bool BlastSpell { get; set; }

		public virtual long MemberNumber { get; set; }

		public virtual long AttackNumber { get; set; }

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
		public virtual ICombatComponent CombatComponent { get; set; }

		public override void Execute()
		{
			RetCode rc;

			Debug.Assert(DobjArtifact != null || DobjMonster != null);

			if (!BlastSpell && ActorMonster.Weapon < 0)
			{
				PrintMustFirstReadyWeapon();

				NextState = Globals.CreateInstance<IStartState>();

				goto Cleanup;
			}

			if (DobjMonster != null)
			{
				if (!BlastSpell)
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

					if (DobjMonster.Reaction != Friendliness.Enemy)
					{
						gEngine.MonsterGetsAggravated(DobjMonster);
					}

					ProcessEvents(EventType.AfterAggravateMonster);

					if (GotoCleanup)
					{
						goto Cleanup;
					}
				}

				CombatComponent = Globals.CreateInstance<ICombatComponent>(x =>
				{
					x.SetNextStateFunc = s => NextState = s;

					x.ActorMonster = ActorMonster;

					x.ActorRoom = ActorRoom;

					x.Dobj = DobjMonster;

					x.MemberNumber = MemberNumber;

					x.AttackNumber = AttackNumber;

					x.BlastSpell = BlastSpell;

					x.OmitSkillGains = BlastSpell || ActorMonster.Weapon == 0 || !ShouldAllowSkillGains();
				});

				CombatComponent.ExecuteAttack();

				goto Cleanup;
			}

			if (!BlastSpell)
			{
				ProcessEvents(EventType.BeforeAttackArtifact);

				if (GotoCleanup)
				{
					goto Cleanup;
				}
			}

			DobjArtAc = null;

			if (!DobjArtifact.IsAttackable(ref _dobjArtAc))
			{
				PrintWhyAttack(DobjArtifact);

				NextState = Globals.CreateInstance<IStartState>();

				goto Cleanup;
			}

			Debug.Assert(DobjArtAc != null);

			CombatComponent = Globals.CreateInstance<ICombatComponent>(x =>
			{
				x.SetNextStateFunc = s => NextState = s;

				x.CopyCommandDataFunc = c => CopyCommandData(c);

				x.ActorMonster = ActorMonster;

				x.ActorRoom = ActorRoom;

				x.Dobj = DobjArtifact;

				x.DobjArtAc = DobjArtAc;

				x.MemberNumber = MemberNumber;

				x.AttackNumber = AttackNumber;

				x.BlastSpell = BlastSpell;

				x.OmitSkillGains = true;
			});

			CombatComponent.ExecuteAttack();

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		public AttackCommand()
		{
			Synonyms = new string[] { "kill" };

			SortOrder = 250;

			Uid = 34;

			Name = "AttackCommand";

			Verb = "attack";

			Type = CommandType.Interactive;

			MemberNumber = 1;

			AttackNumber = 1;
		}
	}
}
