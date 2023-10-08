
// PutCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static TheWayfarersInn.Game.Plugin.Globals;

namespace TheWayfarersInn.Game.Commands
{
	[ClassMappings]
	public class PutCommand : EamonRT.Game.Commands.PutCommand, IPutCommand
	{
		public override void PrintDontNeedTo()
		{
			Debug.Assert(IobjArtifact != null);

			// Registration desk

			if (IobjArtifact.Uid == 26 && ContainerType == ContainerType.In)
			{
				gOut.Print("You can see the floor through the rotted-out bottom of the drawer.");
			}
			else
			{
				base.PrintDontNeedTo();
			}
		}

		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.AfterPutArtifact)
			{
				// Put fine clothing on mannequin

				if (DobjArtifact.Uid == 185 && IobjArtifact.Uid == 183 && !gGameState.FineClothingEnchanted)
				{
					gEngine.PrintEffectDesc(113);

					gGameState.FineClothingEnchanted = true;
				}

				// Put Arcane Cookbook under stone hearth

				else if (IobjArtifact.Uid == 173 && gGameState.KitchenRiddleState == 3)
				{
					var stoneHearthArtifact = gADB[170];

					Debug.Assert(stoneHearthArtifact != null);

					var arcaneCookbookArtifact = gADB[171];

					Debug.Assert(arcaneCookbookArtifact != null);

					var ac = stoneHearthArtifact.GetArtifactCategory(ArtifactType.Treasure);

					Debug.Assert(ac != null);

					ac.Type = ArtifactType.UnderContainer;

					arcaneCookbookArtifact.SetCarriedByContainer(stoneHearthArtifact, ContainerType.Under);

					gGameState.KitchenRiddleState++;
				}
			}
		}

		public override void ExecuteForPlayer()
		{
			Debug.Assert(DobjArtifact != null && IobjArtifact != null);

			// Put kerosene in rusty oil lantern

			if (DobjArtifact.Uid == 6 && IobjArtifact.Uid == 11 && ContainerType == ContainerType.In)
			{
				if (IobjArtifact.Field1 < 200)
				{
					if (DobjArtifact.Field1 > 0)
					{
						gOut.Print("You put some kerosene in {0}.", IobjArtifact.GetTheName());

						var amount = Math.Min(200 - IobjArtifact.Field1, DobjArtifact.Field1);

						IobjArtifact.Field1 += amount;

						DobjArtifact.Field1 -= amount;

						if (DobjArtifact.Field1 <= 0)
						{
							DobjArtifact.AddStateDesc(DobjArtifact.GetEmptyDesc());

							DobjArtifact.Value = 1;

							DobjArtifact.Weight = 1;
						}
					}
					else
					{
						PrintNoneLeft(DobjArtifact);
					}
				}
				else
				{
					PrintFull(IobjArtifact);
				}

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}

			// Put water in purification pool

			else if (DobjArtifact.Uid == 60 && IobjArtifact.Uid == 24 && ContainerType == ContainerType.In)
			{
				var bucketArtifact = gADB[47];

				Debug.Assert(bucketArtifact != null);

				var command = gEngine.CreateInstance<IUseCommand>();

				CopyCommandData(command);

				command.Dobj = bucketArtifact;

				NextState = command;
			}

			// Put bucket into water well

			else if (DobjArtifact.Uid == 47 && IobjArtifact.Uid == 17 && ContainerType == ContainerType.In)
			{
				var command = gEngine.CreateInstance<IUseCommand>();

				CopyCommandData(command);

				NextState = command;
			}

			// Put anything carried into water well / courtyard / gorge / river

			else if ((IobjArtifact.Uid == 17 || IobjArtifact.Uid == 97 || IobjArtifact.Uid == 145 || IobjArtifact.Uid == 146) && ContainerType == ContainerType.In)      // TODO: possibly disallow for certain Artifacts ???
			{
				if (DobjArtifact.IsCarriedByMonster(gCharMonster))
				{
					gOut.EnableOutput = false;

					gEngine.CurrState = gEngine.CreateInstance<IDropCommand>(x =>
					{
						x.ActorMonster = ActorMonster;

						x.ActorRoom = ActorRoom;

						x.Dobj = DobjArtifact;
					});

					gEngine.CurrCommand.Execute();

					gEngine.CurrState = this; 
					
					gOut.EnableOutput = true;

					gOut.Print(IobjArtifact.Uid == 17 ? 
						"You drop {0} into the water well. {1} into the inky blackness and a few moments later you hear a faint splash." :
						IobjArtifact.Uid == 97 ?
						"You drop {0} over the guard rail. {1} through the air, hitting the ground below with a thud." :
						"You absent-mindedly throw {0} into the gorge. {1} into the raging river, swept downstream beyond any hope of recovery.",
						DobjArtifact.GetTheName(), DobjArtifact.EvalPlural("It tumbles", "They tumble"));

					if (IobjArtifact.Uid == 97)
					{
						DobjArtifact.SetInRoomUid(ActorRoom.Uid == 50 ? 30 : 31);
					}
					else
					{
						DobjArtifact.SetInLimbo();
					}
				}
				else if (!DobjArtifact.IsUnmovable())
				{
					gEngine.PushRulesetVersion(5);

					PrintDontHaveIt02(DobjArtifact);

					gEngine.PopRulesetVersion();
				}
				else
				{
					PrintDontBeAbsurd();
				}

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
			else
			{
				base.ExecuteForPlayer();
			}
		}
	}
}
