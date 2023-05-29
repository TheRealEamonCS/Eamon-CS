
// GiveCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.Globals;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class GiveCommand : Command, IGiveCommand
	{
		public virtual long GoldAmount { get; set; }

		/// <summary></summary>
		public virtual ArtifactType[] ArtTypes { get; set; }
		
		/// <summary></summary>
		public virtual IArtifactCategory DobjArtAc { get; set; }

		/// <summary></summary>
		public virtual string IobjMonsterName { get; set; }

		/// <summary></summary>
		public virtual bool ObjOpened { get; set; }

		public override void ExecuteForPlayer()
		{
			RetCode rc;

			Debug.Assert(GoldAmount > 0 || DobjArtifact != null);

			Debug.Assert(IobjMonster != null);

			if (GoldAmount > 0)
			{
				PrintGiveGoldPiecesTo(IobjMonster, GoldAmount);

				PrintAreYouSure();

				gEngine.Buf.Clear();

				rc = gEngine.In.ReadField(gEngine.Buf, gEngine.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				if (gEngine.Buf.Length == 0 || gEngine.Buf[0] == 'N')
				{
					NextState = gEngine.CreateInstance<IStartState>();

					goto Cleanup;
				}

				if (gCharacter.HeldGold < GoldAmount)
				{
					PrintNotEnoughGold();

					NextState = gEngine.CreateInstance<IStartState>();

					goto Cleanup;
				}

				if (IobjMonster.ShouldRefuseToAcceptGold())
				{
					gEngine.PrintMonsterEmotes(IobjMonster);

					gOut.WriteLine();

					goto Cleanup;
				}

				ProcessEvents(EventType.BeforeTakePlayerGold);

				if (GotoCleanup)
				{
					goto Cleanup;
				}

				PrintTakesTheMoney(IobjMonster);

				gCharacter.HeldGold -= GoldAmount;

				if (gEngine.IsRulesetVersion(5, 62))
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

			if (DobjArtifact.IsWornByMonster(ActorMonster))
			{
				PrintWearingRemoveFirst(DobjArtifact);

				NextState = gEngine.CreateInstance<IStartState>();

				goto Cleanup;
			}

			if (!DobjArtifact.IsCarriedByMonster(ActorMonster))
			{
				if (!ShouldAllowRedirectToGetCommand())
				{
					PrintDontHaveIt02(DobjArtifact);

					NextState = gEngine.CreateInstance<IStartState>();
				}
				else if (!GetCommandCalled)
				{
					RedirectToGetCommand<IGiveCommand>(DobjArtifact);
				}
				else if (DobjArtifact.DisguisedMonster == null)
				{
					NextState = gEngine.CreateInstance<IStartState>();
				}

				goto Cleanup;
			}

			if (IobjMonster.ShouldRefuseToAcceptGift(DobjArtifact))
			{
				gEngine.PrintMonsterEmotes(IobjMonster);

				gOut.WriteLine();

				goto Cleanup;
			}

			if (gEngine.EnforceMonsterWeightLimits && !IobjMonster.CanCarryArtifactWeight(DobjArtifact))
			{
				PrintTooHeavy(DobjArtifact);

				goto Cleanup;
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

			ProcessEvents(EventType.AfterRefuseDeadBodyCheck);

			if (GotoCleanup)
			{
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

			DobjArtAc = DobjArtifact.GetArtifactCategory(ArtTypes);

			if (gEngine.IsRulesetVersion(5, 62) || DobjArtAc == null || DobjArtAc.Field2 <= 0)
			{
				DobjArtifact.SetCarriedByMonster(IobjMonster);

				if (gEngine.IsRulesetVersion(5, 62))
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

			if (DobjArtAc.Field2 != gEngine.InfiniteDrinkableEdible)
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
				NextState = gEngine.CreateInstance<IMonsterStartState>();
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

			Name = "GiveCommand";

			Verb = "give";

			Type = CommandType.Interactive;
			
			ArtTypes = new ArtifactType[] { ArtifactType.Drinkable, ArtifactType.Edible };
		}
	}
}
