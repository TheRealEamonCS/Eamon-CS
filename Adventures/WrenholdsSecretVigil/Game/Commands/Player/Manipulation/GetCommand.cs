
// GetCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game.Commands
{
	[ClassMappings]
	public class GetCommand : EamonRT.Game.Commands.GetCommand, IGetCommand
	{
		public override void PrintReceived(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			// Request medallion

			if (artifact.Uid == 10 && gGameState.MedallionCharges > 0)
			{
				base.PrintReceived(artifact);

				gOut.Write("{0}{0}Your hand feels relaxed, but strong.", Environment.NewLine);
			}
			else
			{
				base.PrintReceived(artifact);
			}
		}

		public override void PrintRetrieved(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			// Remove orb

			if (artifact.Uid == 4 && !gGameState.RemovedLifeOrb)
			{
				gEngine.PrintEffectDesc(33);

				gEngine.PrintEffectDesc(34);

				gGameState.RemovedLifeOrb = true;

				base.PrintRetrieved(artifact);
			}

			// Remove medallion

			else if (artifact.Uid == 10 && gGameState.MedallionCharges > 0)
			{
				base.PrintRetrieved(artifact);

				gOut.Write("{0}{0}Your hand feels relaxed, but strong.", Environment.NewLine);
			}
			else
			{
				base.PrintRetrieved(artifact);
			}
		}

		public override void PrintTaken(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			var bronzeKeyArtifact = gADB[27];

			Debug.Assert(bronzeKeyArtifact != null);

			var deviceArtifact1 = gADB[44];

			Debug.Assert(deviceArtifact1 != null);

			var deviceArtifact2 = gADB[49];

			Debug.Assert(deviceArtifact2 != null);

			var ac = artifact.Categories[0];

			// Get lever

			if (artifact.Uid == 48 && deviceArtifact1.IsInRoom(ActorRoom))
			{
				if (TakenArtifactList[0] != artifact)
				{
					gOut.WriteLine();
				}

				deviceArtifact1.SetInLimbo();

				deviceArtifact2.SetInRoom(ActorRoom);

				gEngine.PrintEffectDesc(30);

				base.PrintTaken(artifact);
			}

			// Get medallion

			else if (artifact.Uid == 10 && gGameState.MedallionCharges > 0)
			{
				base.PrintTaken(artifact);

				gOut.Write("{0}{0}Your hand feels relaxed, but strong.", Environment.NewLine);

				if (TakenArtifactList[TakenArtifactList.Count - 1] != artifact)
				{
					gOut.WriteLine();
				}
			}

			// Get straw mattress

			else if (artifact.Uid == 26 && bronzeKeyArtifact.IsInLimbo())
			{
				base.PrintTaken(artifact);

				bronzeKeyArtifact.SetInRoom(ActorRoom);

				gOut.Write("{0}{0}You found something under the mattress!", Environment.NewLine);

				if (TakenArtifactList[TakenArtifactList.Count - 1] != artifact)
				{
					gOut.WriteLine();
				}
			}

			// Get gold curtain

			else if (artifact.Uid == 40 && ac.Type == ArtifactType.DoorGate)
			{
				base.PrintTaken(artifact);

				ActorRoom.SetDirs(Direction.South, 68);

				ac.Type = ArtifactType.Treasure;

				ac.Field1 = 0;

				ac.Field2 = 0;

				ac.Field3 = 0;

				ac.Field4 = 0;

				ac.Field5 = 0;
			}
			else
			{
				base.PrintTaken(artifact);
			}
		}

		public override void ProcessArtifact(IArtifact artifact, IArtifactCategory ac, ref bool nlFlag)
		{
			Debug.Assert(artifact != null);

			Debug.Assert(ac != null);

			// Get slime

			if (artifact.Uid == 24 || artifact.Uid == 25)
			{
				ProcessAction(() => PrintCantGetSlime(), ref nlFlag);
			}

			// Get rope

			else if (artifact.Uid == 28)
			{
				gEngine.PrintEffectDesc(25);

				if (!gGameState.PulledRope)
				{
					var monsterList = gEngine.GetMonsterList(m => m.Uid >= 14 && m.Uid <= 16);

					foreach (var monster in monsterList)
					{
						monster.SetInRoomUid(48);
					}

					gGameState.PulledRope = true;
				}

				ProcessAction(() => PrintCantDetachRope(), ref nlFlag);
			}
			else
			{
				base.ProcessArtifact(artifact, ac, ref nlFlag);
			}
		}

		public virtual void PrintCantGetSlime()
		{
			gOut.Print("Corrosive slime is not something to get.");
		}

		public virtual void PrintCantDetachRope()
		{
			gOut.Print("You cannot detach the rope.");
		}
	}
}
