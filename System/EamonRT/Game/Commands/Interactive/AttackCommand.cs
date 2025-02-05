
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
using static EamonRT.Game.Plugin.Globals;

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

		public override void ExecuteForPlayer()
		{
			RetCode rc;

			Debug.Assert(DobjArtifact != null || DobjMonster != null);

			if (!BlastSpell && ActorMonster.Weapon < 0)
			{
				PrintMustFirstReadyWeapon();

				NextState = gEngine.CreateInstance<IStartState>();

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

						NextState = gEngine.CreateInstance<IStartState>();

						goto Cleanup;
					}

					if (DobjMonster.ShouldCheckToAttackNonEnemy())
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

				CombatComponent = gEngine.CreateInstance<ICombatComponent>(x =>
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

				NextState = gEngine.CreateInstance<IStartState>();

				goto Cleanup;
			}

			Debug.Assert(DobjArtAc != null);

			CombatComponent = gEngine.CreateInstance<ICombatComponent>(x =>
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
				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
		}

		public override void ExecuteForMonster()
		{
			Debug.Assert(DobjMonster != null);

			CombatComponent = gEngine.CreateInstance<ICombatComponent>(x =>
			{
				x.SetNextStateFunc = s => NextState = s;

				x.ActorMonster = ActorMonster;

				x.ActorRoom = ActorRoom;

				x.Dobj = DobjMonster;

				x.MemberNumber = MemberNumber;

				x.AttackNumber = AttackNumber;
			});

			CombatComponent.ExecuteAttack();

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IErrorState>(x =>
				{
					x.ErrorMessage = string.Format("{0}: NextState == null", Name);
				});
			}
		}

		public AttackCommand()
		{
			Synonyms = new string[] { "kill" };

			SortOrder = 250;

			IsMonsterEnabled = true;

			Name = "AttackCommand";

			Verb = "attack";

			Type = CommandType.Interactive;

			MemberNumber = 1;

			AttackNumber = 1;
		}
	}
}
