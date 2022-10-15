
// PutCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game.Commands
{
	[ClassMappings]
	public class PutCommand : EamonRT.Game.Commands.PutCommand, IPutCommand
	{
		public override void PrintBrokeIt(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			// Swallower shark

			if (artifact.Uid == 31)
			{
				gOut.Print("You mangled {0}!", artifact.EvalPlural("it", "them"));
			}
			else
			{
				base.PrintBrokeIt(artifact);
			}
		}

		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.AfterPutArtifact)
			{
				// Put anything in slime destroys it

				if (IobjArtifact.Uid == 24 || IobjArtifact.Uid == 25)
				{
					gOut.Print("{0} start{1} dissolving on contact with {2}!", DobjArtifact.GetTheName(true), DobjArtifact.EvalPlural("s", ""), IobjArtifact.GetTheName(buf: Globals.Buf01));

					gOut.Print("{0} {1} destroyed!", DobjArtifact.GetTheName(true), DobjArtifact.EvalPlural("is", "are"));

					DobjArtifact.SetInLimbo();
				}

				// Put orb in metal pedestal

				else if (DobjArtifact.Uid == 4 && IobjArtifact.Uid == 43)
				{
					gEngine.PrintEffectDesc(43);

					gEngine.PrintEffectDesc(44);

					var adjacentRoom = gRDB[45];

					Debug.Assert(adjacentRoom != null);

					var newRoom = gRDB[15];

					Debug.Assert(newRoom != null);

					adjacentRoom.SetDir(Direction.South, 15);

					IobjArtifact.IsListed = false;

					gEngine.TransportPlayerBetweenRooms(ActorRoom, newRoom, null);

					NextState = Globals.CreateInstance<IAfterPlayerMoveState>();
				}
			}
		}

		public override void Execute()
		{
			Debug.Assert(DobjArtifact != null && IobjArtifact != null);

			if (IobjArtifact.Uid == 24 || IobjArtifact.Uid == 25)
			{
				gEngine.ConvertTreasureToContainer(IobjArtifact);
			}

			base.Execute();

			if (IobjArtifact.Uid == 24 || IobjArtifact.Uid == 25)
			{
				gEngine.ConvertContainerToTreasure(IobjArtifact);
			}
		}
	}
}
