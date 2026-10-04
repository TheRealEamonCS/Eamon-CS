
// GiveCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Linq;
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
					var disabledStates = new long[] { 2, 3 };

					var eventState = gGameState.GetEventState(EventState.ChildsApparition);

					// Disable giving

					if (disabledStates.Contains(eventState))
					{
						gEngine.PrintMonsterEmotes(IobjMonster);

						gOut.WriteLine();

						GotoCleanup = true;
					}

					// Portrait

					else if (DobjArtifact.Uid == 30)
					{
						if (!gGameState.CharlottePortraitGiven)
						{
							gOut.Print("{0} smiles. \"That's my Mommy, and Daddy and me!\" She reaches for the image of her parents, but her hand passes through the canvas. Her smile fades as tears well in her eyes.", IobjMonster.GetTheName(true));

							gGameState.SetEventState(EventState.ChildsApparition, 2);

							gGameState.CharlottePortraitGiven = true;
						}
						else
						{
							gOut.Print("{0} refuses to go near it.", IobjMonster.GetTheName(true));
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
							gOut.Print("{0} refuses to go near it.", IobjMonster.GetTheName(true));
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
						gOut.Print("{0} gets a mischievous glint in her eye. {1}She looks at the {2}.", IobjMonster.GetTheName(true), gGameState.CharlotteArtisansStory ? "\"One of the others with hammers and saws...\" " : "", ActorRoom.EvalRoomType("floor", "ground"));

						GotoCleanup = true;
					}

					// Disable giving

					else
					{
						gEngine.PrintMonsterEmotes(IobjMonster);

						gOut.WriteLine();

						GotoCleanup = true;
					}
				}
			}
		}
	}
}
