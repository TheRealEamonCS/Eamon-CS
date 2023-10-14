﻿
// GiveCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using TheWayfarersInn.Framework.Primitive.Enums;
using static TheWayfarersInn.Game.Plugin.Globals;

namespace TheWayfarersInn.Game.Commands
{
	[ClassMappings]
	public class GiveCommand : EamonRT.Game.Commands.GiveCommand, IGiveCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.AfterEnforceMonsterWeightLimitsCheck)
			{
				// Charlotte

				if (IobjMonster.Uid == 4)
				{
					// Portrait

					if (DobjArtifact.Uid == 30)
					{
						if (!gGameState.CharlottePortraitGiven)
						{
							gOut.Print("{0} smiles and says, \"That's my Mommy, and Daddy and me!\" She reaches out to touch the image of her parents, but as her hand passes through the canvas, the smile slowly fades. Tears well up in her eyes.", IobjMonster.GetTheName(true));

							gGameState.SetEventState(EventState.ChildsApparition, 2);

							gGameState.CharlottePortraitGiven = true;
						}
						else
						{
							gOut.Print("{0} refuses to go anywhere near it.", IobjMonster.GetTheName(true));
						}

						GotoCleanup = true;
					}

					// Child's skeleton

					else if (DobjArtifact.Uid == 54)
					{
						if (!gGameState.CharlotteBonesGiven)
						{
							gEngine.PrintEffectDesc(36);

							gGameState.SetEventState(EventState.ChildsApparition, 2);

							gGameState.CharlotteBonesGiven = true;
						}
						else
						{
							gOut.Print("{0} refuses to go anywhere near it.", IobjMonster.GetTheName(true));
						}

						GotoCleanup = true;
					}

					// Teddy bear

					else if (DobjArtifact.Uid == 55)
					{
						gEngine.PrintEffectDesc(145);

						GotoCleanup = true;
					}
		
					// Artisan body part

					else if (gDobjArtifact(this).IsArtisanBodyPartArtifact())
					{
						gOut.Print("{0} gets a mischievous glint in her eye and says, \"One of the others with hammers and saws...\" She then looks at the {1}.", IobjMonster.GetTheName(true), ActorRoom.EvalRoomType("floor", "ground"));

						GotoCleanup = true;
					}
				}
			}
		}
	}
}
