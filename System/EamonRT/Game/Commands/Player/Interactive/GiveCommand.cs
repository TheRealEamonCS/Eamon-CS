
// GiveCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon;
using Eamon.Framework;
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

		/// <summary></summary>
		public virtual bool ObjOpened { get; set; }

		public override void Execute()
		{
			RetCode rc;

			Debug.Assert(GoldAmount > 0 || DobjArtifact != null);

			Debug.Assert(IobjMonster != null);

			if (GoldAmount > 0)
			{
				PrintGiveGoldPiecesTo(IobjMonster, GoldAmount);

				PrintAreYouSure();

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

				ProcessEvents(EventType.BeforeTakePlayerGold);

				if (GotoCleanup)
				{
					goto Cleanup;
				}

				PrintTakesTheMoney(IobjMonster);

				gCharacter.HeldGold -= GoldAmount;

				if (Globals.IsRulesetVersion(5, 25))
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

						gEngine.PrintMonsterEmotes(IobjMonster);

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
				gEngine.PrintMonsterEmotes(IobjMonster);

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

			ProcessEvents(EventType.AfterGiveReadiedWeaponCheck);

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			PrintGiveObjToActor(DobjArtifact, IobjMonster);

			DobjArtAc = DobjArtifact.GetArtifactCategory(new ArtifactType[] { ArtifactType.Drinkable, ArtifactType.Edible });

			if (Globals.IsRulesetVersion(5, 25) || DobjArtAc == null || DobjArtAc.Field2 <= 0)
			{
				DobjArtifact.SetCarriedByMonster(IobjMonster);

				if (Globals.IsRulesetVersion(5, 25))
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

						gEngine.PrintMonsterEmotes(IobjMonster);

						gOut.WriteLine();
					}
				}

				goto Cleanup;
			}

			ObjOpened = false;

			if (!DobjArtAc.IsOpen())
			{
				ObjOpened = true;

				DobjArtAc.SetOpen(true);
			}

			if (DobjArtAc.Field2 != Constants.InfiniteDrinkableEdible)
			{
				DobjArtAc.Field2--;
			}

			if (DobjArtAc.Field2 > 0)
			{
				PrintOpensConsumesAndHandsBack(DobjArtifact, IobjMonster, ObjOpened, DobjArtAc.Type == ArtifactType.Edible);
			}
			else
			{
				DobjArtifact.Value = 0;

				if (DobjArtAc.Type == ArtifactType.Edible)
				{
					DobjArtifact.SetInLimbo();

					PrintConsumesItAll(DobjArtifact, IobjMonster, ObjOpened);
				}
				else
				{
					rc = DobjArtifact.AddStateDesc(DobjArtifact.GetEmptyDesc());

					Debug.Assert(gEngine.IsSuccess(rc));

					PrintConsumesItAllHandsBack(DobjArtifact, IobjMonster, ObjOpened);
				}
			}

			if (DobjArtAc.Field1 == 0)
			{
				goto Cleanup;
			}

			IobjMonster.DmgTaken -= DobjArtAc.Field1;

			if (IobjMonster.DmgTaken < 0)
			{
				IobjMonster.DmgTaken = 0;
			}

			PrintHealthStatus(IobjMonster, false);

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
