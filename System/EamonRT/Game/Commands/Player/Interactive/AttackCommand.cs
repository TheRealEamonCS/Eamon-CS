
// AttackCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Combat;
using EamonRT.Framework.Commands;
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

		public virtual bool CheckAttack { get; set; }

		public virtual long MemberNumber { get; set; }

		public virtual long AttackNumber { get; set; }

		/// <summary></summary>
		public virtual IList<IArtifact> SpilledArtifactList { get; set; }

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
		public virtual IArtifactCategory ActorWeaponAc { get; set; }

		/// <summary></summary>
		public virtual IMonster DisguisedMonster { get; set; }

		/// <summary></summary>
		public virtual IArtifact ActorWeapon { get; set; }

		/// <summary></summary>
		public virtual ICommand RedirectCommand { get; set; }

		/// <summary></summary>
		public virtual ICombatSystem CombatSystem { get; set; }

		/// <summary></summary>
		public virtual long KeyArtifactUid { get; set; }

		/// <summary></summary>
		public virtual long BreakageStrength { get; set; }

		/// <summary></summary>
		public virtual long BreakageDice { get; set; }

		/// <summary></summary>
		public virtual long BreakageSides { get; set; }

		/// <summary></summary>
		public virtual long BreakageDamage { get; set; }

		public override void Execute()
		{
			RetCode rc;

			Debug.Assert(DobjArtifact != null || DobjMonster != null);

			if (!BlastSpell && ActorMonster.Weapon <= 0)
			{
				PrintMustFirstReadyWeapon();

				NextState = Globals.CreateInstance<IStartState>();

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

				if (!CheckAttack && DobjMonster.Reaction != Friendliness.Enemy)
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

					gEngine.MonsterGetsAggravated(DobjMonster);
				}

				CombatSystem = Globals.CreateInstance<ICombatSystem>(x =>
				{
					x.SetNextStateFunc = s => NextState = s;

					x.OfMonster = ActorMonster;

					x.DfMonster = DobjMonster;

					x.MemberNumber = MemberNumber;

					x.AttackNumber = AttackNumber;

					x.BlastSpell = BlastSpell;

					x.OmitSkillGains = !BlastSpell && !ShouldAllowSkillGains();
				});

				CombatSystem.ExecuteAttack();

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

			if (DobjArtAc.Type == ArtifactType.DeadBody)
			{
				if (BlastSpell)
				{
					gOut.Print("{0}", gEngine.GetBlastDesc());
				}

				DobjArtifact.SetInLimbo();

				PrintHackToBits();

				goto Cleanup;
			}

			if (DobjArtAc.Type == ArtifactType.DisguisedMonster)
			{
				gEngine.RevealDisguisedMonster(ActorRoom, DobjArtifact);

				DisguisedMonster = gMDB[DobjArtAc.Field1];

				Debug.Assert(DisguisedMonster != null);

				RedirectCommand = null;

				if (BlastSpell)
				{
					RedirectCommand = Globals.CreateInstance<IBlastCommand>(x =>
					{
						x.CastSpell = false;
					});
				}
				else
				{
					RedirectCommand = Globals.CreateInstance<IAttackCommand>();
				}

				CopyCommandData(RedirectCommand);

				RedirectCommand.Dobj = DisguisedMonster;

				NextState = RedirectCommand;

				goto Cleanup;
			}

			/*
				Damage it...
			*/

			KeyArtifactUid = DobjArtAc.GetKeyUid();

			if (KeyArtifactUid == -2)
			{
				PrintAlreadyBrokeIt(DobjArtifact);

				goto Cleanup;
			}

			BreakageStrength = DobjArtAc.GetBreakageStrength();

			if (BreakageStrength < 1000)
			{
				gOut.Print("Nothing happens.");

				goto Cleanup;
			}

			BreakageDice = 0;

			BreakageSides = 0;

			if (BlastSpell)
			{
				if (Globals.IsRulesetVersion(5, 15))
				{
					BreakageDice = 1;

					BreakageSides = 6;
				}
				else
				{
					BreakageDice = 2;

					BreakageSides = 5;
				}

				Globals.Buf.SetPrint("{0}", gEngine.GetBlastDesc());
			}
			else
			{
				ActorWeapon = gADB[ActorMonster.Weapon];

				Debug.Assert(ActorWeapon != null);

				ActorWeaponAc = ActorWeapon.GeneralWeapon;

				Debug.Assert(ActorWeaponAc != null);

				BreakageDice = ActorWeaponAc.Field3;

				BreakageSides = ActorWeaponAc.Field4;

				BuildWhamHitObj();
			}

			gOut.Write("{0}", Globals.Buf);

			BreakageDamage = gEngine.RollDice(BreakageDice, BreakageSides, 0);

			BreakageStrength -= BreakageDamage;

			if (BreakageStrength > 1000)
			{
				DobjArtAc.SetBreakageStrength(BreakageStrength);

				goto Cleanup;
			}

			/*
				Broken!
			*/

			DobjArtAc.SetOpen(true);

			DobjArtAc.SetKeyUid(-2);

			DobjArtAc.Field4 = 0;

			DobjArtifact.Value = 0;

			rc = DobjArtifact.AddStateDesc(DobjArtifact.GetBrokenDesc());

			Debug.Assert(gEngine.IsSuccess(rc));

			BuildSmashesToPieces();

			if (DobjArtAc.Type == ArtifactType.InContainer)
			{
				SpilledArtifactList = DobjArtifact.GetContainedList(containerType: ContainerType.In);

				if (DobjArtifact.OnContainer != null && DobjArtifact.IsInContainerOpenedFromTop())
				{
					SpilledArtifactList.AddRange(DobjArtifact.GetContainedList(containerType: ContainerType.On));
				}

				foreach (var artifact in SpilledArtifactList)
				{
					artifact.SetInRoom(ActorRoom);
				}

				if (SpilledArtifactList.Count > 0)
				{
					BuildContentsSpillToFloor();
				}

				DobjArtAc.Field3 = 0;
			}

			Globals.Buf.AppendFormat("!{0}", Environment.NewLine);

			gOut.Write("{0}", Globals.Buf);

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		public virtual void PrintHackToBits()
		{
			gOut.Print("You {0} {1} to bits!", BlastSpell ? "blast" : "hack", DobjArtifact.EvalPlural("it", "them"));
		}

		public virtual void BuildWhamHitObj()
		{
			Globals.Buf.SetPrint("Wham!  You hit {0}!", DobjArtifact.GetTheName(buf: Globals.Buf01));
		}

		public virtual void BuildSmashesToPieces()
		{
			Globals.Buf.SetFormat("{0}{1} {2} to pieces", Environment.NewLine, DobjArtifact.GetTheName(true, buf: Globals.Buf01), DobjArtifact.EvalPlural("smashes", "smash"));
		}

		public virtual void BuildContentsSpillToFloor()
		{
			Globals.Buf.AppendFormat("; {0} contents spill to the {1}", DobjArtifact.EvalPlural("its", "their"), ActorRoom.EvalRoomType("floor", "ground"));
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
