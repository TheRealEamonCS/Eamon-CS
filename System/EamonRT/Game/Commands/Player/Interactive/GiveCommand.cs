
// GiveCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class GiveCommand : Command, IGiveCommand
	{
		public long _dobjArtifactCount;

		public long _dobjArtifactWeight;

		public long _iobjMonsterInventoryWeight;

		public virtual long GoldAmount { get; set; }

		/// <summary></summary>
		public virtual IArtifactCategory DobjArtAc { get; set; }

		/// <summary></summary>
		public virtual string IobjMonsterName { get; set; }

		/// <summary></summary>
		public virtual long DobjArtifactCount
		{
			get
			{
				return _dobjArtifactCount;
			}

			set
			{
				_dobjArtifactCount = value;
			}
		}

		/// <summary></summary>
		public virtual long DobjArtifactWeight
		{
			get
			{
				return _dobjArtifactWeight;
			}

			set
			{
				_dobjArtifactWeight = value;
			}
		}

		/// <summary></summary>
		public virtual long IobjMonsterInventoryWeight
		{
			get
			{
				return _iobjMonsterInventoryWeight;
			}

			set
			{
				_iobjMonsterInventoryWeight = value;
			}
		}

		public override void Execute()
		{
			RetCode rc;

			Debug.Assert(GoldAmount > 0 || DobjArtifact != null);

			Debug.Assert(IobjMonster != null);

			if (GoldAmount > 0)
			{
				gOut.Print("Give {0} gold piece{1} to {2}.",
					gEngine.GetStringFromNumber(GoldAmount, false, Globals.Buf),
					GoldAmount > 1 ? "s" : "",
					IobjMonster.GetTheName(buf: Globals.Buf01));

				gOut.Write("{0}Are you sure (Y/N): ", Environment.NewLine);

				Globals.Buf.Clear();

				rc = Globals.In.ReadField(Globals.Buf, Constants.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				if (Globals.Buf.Length == 0 || Globals.Buf[0] == 'N')
				{
					NextState = Globals.CreateInstance<IStartState>();

					goto Cleanup;
				}

				if (gCharacter.HeldGold < GoldAmount)
				{
					PrintNotEnoughGold();

					NextState = Globals.CreateInstance<IStartState>();

					goto Cleanup;
				}

				ProcessEvents(EventType.BeforeMonsterTakesGold);

				if (GotoCleanup)
				{
					goto Cleanup;
				}

				gOut.Print("{0} take{1} the money.",
					IobjMonster.GetTheName(true),
					IobjMonster.EvalPlural("s", ""));

				gCharacter.HeldGold -= GoldAmount;

				if (Globals.IsRulesetVersion(5))
				{
					IobjMonster.CalculateGiftFriendliness(GoldAmount, false);

					IobjMonster.ResolveReaction(gCharacter);
				}
				else
				{
					if (IobjMonster.Reaction == Friendliness.Neutral && GoldAmount > 4999)
					{
						IobjMonster.Friendliness = (Friendliness)200;

						IobjMonster.Reaction = Friendliness.Friend;

						gEngine.MonsterEmotes(IobjMonster);

						gOut.WriteLine();
					}
				}

				goto Cleanup;
			}

			if (DobjArtifact.IsWornByCharacter())
			{
				PrintWearingRemoveFirst(DobjArtifact);

				NextState = Globals.CreateInstance<IStartState>();

				goto Cleanup;
			}

			if (!DobjArtifact.IsCarriedByCharacter())
			{
				if (!GetCommandCalled)
				{
					RedirectToGetCommand<IGiveCommand>(DobjArtifact);
				}
				else if (DobjArtifact.DisguisedMonster == null)
				{
					NextState = Globals.CreateInstance<IStartState>();
				}

				goto Cleanup;
			}

			if (IobjMonster.ShouldRefuseToAcceptGift(DobjArtifact))
			{
				gEngine.MonsterEmotes(IobjMonster);

				gOut.WriteLine();

				goto Cleanup;
			}

			DobjArtifactCount = 0;

			DobjArtifactWeight = DobjArtifact.Weight;

			if (DobjArtifact.GeneralContainer != null)
			{
				rc = DobjArtifact.GetContainerInfo(ref _dobjArtifactCount, ref _dobjArtifactWeight, ContainerType.In, true);

				Debug.Assert(gEngine.IsSuccess(rc));

				rc = DobjArtifact.GetContainerInfo(ref _dobjArtifactCount, ref _dobjArtifactWeight, ContainerType.On, true);

				Debug.Assert(gEngine.IsSuccess(rc));
			}

			if (gEngine.EnforceMonsterWeightLimits)
			{
				IobjMonsterInventoryWeight = 0;

				rc = IobjMonster.GetFullInventoryWeight(ref _iobjMonsterInventoryWeight, recurse: true);

				Debug.Assert(gEngine.IsSuccess(rc));

				if (DobjArtifactWeight > IobjMonster.GetWeightCarryableGronds() || DobjArtifactWeight + IobjMonsterInventoryWeight > IobjMonster.GetWeightCarryableGronds() * IobjMonster.CurrGroupCount)
				{
					PrintTooHeavy(DobjArtifact);

					goto Cleanup;
				}
			}

			ProcessEvents(EventType.AfterEnforceMonsterWeightLimitsCheck);

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			if (DobjArtifact.DeadBody != null && IobjMonster.ShouldRefuseToAcceptDeadBody(DobjArtifact))
			{
				PrintPolitelyRefuses(IobjMonster);

				goto Cleanup;
			}

			if (gGameState.Ls == DobjArtifact.Uid)
			{
				Debug.Assert(DobjArtifact.LightSource != null);

				gEngine.LightOut(DobjArtifact);
			}

			if (ActorMonster.Weapon == DobjArtifact.Uid)
			{
				Debug.Assert(DobjArtifact.GeneralWeapon != null);

				rc = DobjArtifact.RemoveStateDesc(DobjArtifact.GetReadyWeaponDesc());

				Debug.Assert(gEngine.IsSuccess(rc));

				ActorMonster.Weapon = -1;
			}

			ProcessEvents(EventType.AfterPlayerGivesReadiedWeaponCheck);

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			PrintGiveObjToActor(DobjArtifact, IobjMonster);

			DobjArtAc = DobjArtifact.GetArtifactCategory(new ArtifactType[] { ArtifactType.Drinkable, ArtifactType.Edible });

			if (Globals.IsRulesetVersion(5) || DobjArtAc == null || DobjArtAc.Field2 <= 0)
			{
				DobjArtifact.SetCarriedByMonster(IobjMonster);

				if (Globals.IsRulesetVersion(5))
				{
					IobjMonster.CalculateGiftFriendliness(DobjArtifact.Value, true);

					IobjMonster.ResolveReaction(gCharacter);
				}
				else
				{
					if (IobjMonster.Reaction == Friendliness.Neutral)
					{
						IobjMonster.Friendliness = (Friendliness)200;

						IobjMonster.Reaction = Friendliness.Friend;

						gEngine.MonsterEmotes(IobjMonster);

						gOut.WriteLine();
					}
				}

				goto Cleanup;
			}

			IobjMonsterName = IobjMonster.EvalPlural(IobjMonster.GetTheName(true), IobjMonster.GetArticleName(true, true, false, true, Globals.Buf01));

			Globals.Buf01.Clear();

			if (!DobjArtAc.IsOpen())
			{
				Globals.Buf01.SetFormat(" opens {0}", DobjArtifact.GetTheName());

				DobjArtAc.SetOpen(true);
			}

			if (DobjArtAc.Field2 != Constants.InfiniteDrinkableEdible)
			{
				DobjArtAc.Field2--;
			}

			if (DobjArtAc.Field2 > 0)
			{
				Globals.Buf.SetPrint("{0}{1}{2} takes a {3} and hands {4} back.",
					IobjMonsterName,
					Globals.Buf01,
					Globals.Buf01.Length > 0 ? "," : "",
					DobjArtAc.Type == ArtifactType.Edible ? "bite" : "drink",
					DobjArtifact.EvalPlural("it", "them"));
			}
			else
			{
				DobjArtifact.Value = 0;

				if (DobjArtAc.Type == ArtifactType.Edible)
				{
					DobjArtifact.SetInLimbo();

					Globals.Buf.SetPrint("{0}{1}{2} eats {3} all.",
						IobjMonsterName,
						Globals.Buf01,
						Globals.Buf01.Length > 0 ? " and" : "",
						DobjArtifact.EvalPlural("it", "them"));
				}
				else
				{
					rc = DobjArtifact.AddStateDesc(DobjArtifact.GetEmptyDesc());

					Debug.Assert(gEngine.IsSuccess(rc));

					Globals.Buf.SetPrint("{0}{1}{2} drinks {3} all and hands {4} back.",
						IobjMonsterName,
						Globals.Buf01,
						Globals.Buf01.Length > 0 ? "," : "",
						DobjArtifact.EvalPlural("it", "them"),
						DobjArtifact.EvalPlural("it", "them"));
				}
			}

			gOut.Write("{0}", Globals.Buf);

			if (DobjArtAc.Field1 == 0)
			{
				goto Cleanup;
			}

			IobjMonster.DmgTaken -= DobjArtAc.Field1;

			if (IobjMonster.DmgTaken < 0)
			{
				IobjMonster.DmgTaken = 0;
			}

			Globals.Buf.SetFormat("{0}{1} is ",
				Environment.NewLine,
				IobjMonster.GetTheName(true, true, false, true, Globals.Buf01));

			IobjMonster.AddHealthStatus(Globals.Buf);

			gOut.Write("{0}", Globals.Buf);

			if (IobjMonster.IsDead())
			{
				gEngine.MonsterDies(ActorMonster, IobjMonster);
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		/*
		public override bool IsPrepEnabled(IPrep prep)
		{
			Debug.Assert(prep != null);

			PrepNames = new string[] { "to" };

			return PrepNames.FirstOrDefault(pn => prep.Name.Equals(pn, StringComparison.OrdinalIgnoreCase)) != null;
		}
		*/

		public GiveCommand()
		{
			SortOrder = 280;

			IsIobjEnabled = true;

			Uid = 37;

			Name = "GiveCommand";

			Verb = "give";

			Type = CommandType.Interactive;
		}
	}
}
