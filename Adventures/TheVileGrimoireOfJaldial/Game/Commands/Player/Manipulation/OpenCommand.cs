
// OpenCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using System.Linq;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static TheVileGrimoireOfJaldial.Game.Plugin.Globals;

namespace TheVileGrimoireOfJaldial.Game.Commands
{
	[ClassMappings]
	public class OpenCommand : EamonRT.Game.Commands.OpenCommand, IOpenCommand
	{
		public override void PrintWontOpen(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			if (artifact.Uid == 3 || artifact.Uid == 4 || artifact.Uid == 5 || artifact.Uid == 13 || artifact.Uid == 35)
			{
				gOut.Print("You don't have the right tools for that.");
			}
			else
			{
				base.PrintWontOpen(artifact);
			}
		}

		public override void PrintLocked(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			if (artifact.Uid == 3 || artifact.Uid == 4 || artifact.Uid == 5)
			{
				gOut.Print("You can't seem to get enough leverage to pry open {0}'s door.", artifact.GetTheName());

				artifact.DoorGate.SetKeyUid(-1);
			}
			else if (artifact.Uid == 13)
			{
				gOut.Print("You work for a while on {0}'s lock but can't break it.", artifact.GetTheName());

				artifact.InContainer.SetKeyUid(-1);
			}
			else if (artifact.Uid == 35)
			{
				gOut.Print("You work for a while on {0}'s lid but can't pry it open.", artifact.GetTheName());

				artifact.InContainer.SetKeyUid(-1);
			}
			else
			{
				base.PrintLocked(artifact);
			}
		}

		public override void PrintOpenObjWithKey(IArtifact artifact, IArtifact key)
		{
			Debug.Assert(artifact != null && key != null);

			if (artifact.Uid == 3 || artifact.Uid == 4 || artifact.Uid == 5)
			{
				gOut.Print("With great effort, {0}'s door slides open.", artifact.GetTheName());
			}
			else if (artifact.Uid == 13)
			{
				gOut.Print("You manage to break open {0}, and find several items inside!", artifact.GetTheName());
			}
			else if (artifact.Uid == 35)
			{
				gOut.Print("You manage to pry open {0}'s lid, and find something inside!", artifact.GetTheName());
			}
			else
			{
				base.PrintOpenObjWithKey(artifact, key);
			}
		}

		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			// If wine cask opened reveal crimson amoeba

			if (eventType == EventType.AfterOpenArtifact && DobjArtifact.Uid == 17 && !gGameState.AmoebaAppeared)
			{
				var amoebaMonster = gMDB[25];

				Debug.Assert(amoebaMonster != null);

				amoebaMonster.SetInRoom(ActorRoom);

				gEngine.PrintEffectDesc(102);

				gGameState.AmoebaAppeared = true;

				NextState = gEngine.CreateInstance<IStartState>();

				GotoCleanup = true;
			}
		}

		public override void Execute()
		{
			Debug.Assert(DobjArtifact != null);

			// Grimoire can't be opened

			if (DobjArtifact.Uid == 27)
			{
				gEngine.PrintEffectDesc(90);

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}

			// DoorGates and InContainers sometimes need to be pryed open

			else if (((DobjArtifact.Uid == 3 || DobjArtifact.Uid == 4 || DobjArtifact.Uid == 5) && !DobjArtifact.DoorGate.IsOpen()) || ((DobjArtifact.Uid == 13 || DobjArtifact.Uid == 35) && !DobjArtifact.InContainer.IsOpen() && DobjArtifact.InContainer.GetKeyUid() == -1))
			{
				// Reset the Key Uid to the available Artifact with the best leverage (if any)

				var keyList = gADB.Records.Cast<Framework.IArtifact>().Where(a => (a.IsInRoom(ActorRoom) || a.IsCarriedByMonster(ActorMonster)) && a.GetLeverageBonus() > 0).OrderByDescending(a01 => a01.GetLeverageBonus()).ToList();

				var key = keyList.Count > 0 ? keyList[0] : null;

				if (key != null)
				{
					if (DobjArtifact.DoorGate != null)
					{
						DobjArtifact.DoorGate.SetKeyUid(key.Uid);
					}
					else
					{
						DobjArtifact.InContainer.SetKeyUid(key.Uid);
					}

					gOut.Print("[Using {0} for leverage.]", key.GetTheName());

					// Failed save throw versus Hardiness means door/container still stuck (should try again)

					if (!gEngine.SaveThrow(Stat.Hardiness, key.GetLeverageBonus()))
					{
						var keyLocation = key.Location;

						key.SetInLimbo();

						base.Execute();

						key.Location = keyLocation;
					}
					else
					{
						base.Execute();
					}
				}
				else
				{
					base.Execute();
				}
			}
			else
			{
				base.Execute();
			}
		}
	}
}
