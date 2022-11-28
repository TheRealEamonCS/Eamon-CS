
// GetCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static ThePyramidOfAnharos.Game.Plugin.Globals;

namespace ThePyramidOfAnharos.Game.Commands
{
	[ClassMappings]
	public class GetCommand : EamonRT.Game.Commands.GetCommand, IGetCommand
	{
		public override void Execute()
		{
			Debug.Assert(DobjArtifact != null);

			// Water

			if (DobjArtifact.Uid == 78)
			{
				var waterBagArtifact = gADB[12];

				Debug.Assert(waterBagArtifact != null);

				var caravanArtifact = gADB[47];

				Debug.Assert(caravanArtifact != null);

				if (waterBagArtifact.IsCarriedByCharacter())
				{
					if (ActorRoom.Uid == 26 || ActorRoom.Uid == 27)
					{
						gEngine.PrintEffectDesc(16);

						waterBagArtifact.SetInLimbo();

						if (gGameState.KW > 5)
						{
							gGameState.KW = 5;
						}
					}
					else if (ActorRoom.Uid == 38 || ActorRoom.Uid == 40 || ActorRoom.Uid == 55)
					{
						gOut.Print("You fill your water bag.");

						if (gGameState.KW < 150)
						{
							gGameState.KW = 150;
						}
					}
					else if (ActorRoom.Uid == 47 && caravanArtifact.IsInRoom(ActorRoom))
					{
						if (gCharacter.HeldGold >= 100)
						{
							gOut.Print("The caravan sells you some water.");

							gCharacter.HeldGold -= 100;

							if (gGameState.KW < 100)
							{
								gGameState.KW = 100;
							}
						}
						else
						{
							gOut.Print("The caravan won't sell you water on credit.");
						}
					}
					else
					{
						gOut.Print("There is no water here.");
					}
				}
				else
				{
					gOut.Print("You have no water bag.");

					if (ActorRoom.Uid == 38 || ActorRoom.Uid == 40 || ActorRoom.Uid == 55)
					{
						gOut.Print("You only get a drink.");

						gGameState.KW = 5;
					}
					else if (ActorRoom.Uid == 47 && caravanArtifact.IsInRoom(ActorRoom))
					{
						gOut.Print("The caravan gives you a drink.");

						gGameState.KW = 5;
					}
					else
					{
						gOut.Print("There is no water here.");
					}
				}

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
			else
			{
				base.Execute();
			}
		}

		public override void ProcessArtifact(IArtifact artifact, IArtifactCategory ac, ref bool nlFlag)
		{
			Debug.Assert(artifact != null);

			base.ProcessArtifact(artifact, ac, ref nlFlag);

			// Rope

			if (artifact.Uid == 13 && artifact.IsCarriedByCharacter())
			{
				gGameState.KG = 0;
			}
		}
	}
}
