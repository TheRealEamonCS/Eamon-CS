
// OpenCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game.Commands
{
	[ClassMappings]
	public class OpenCommand : EamonRT.Game.Commands.OpenCommand, IOpenCommand
	{
		public override void PrintOpened(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			// Large green device

			if (artifact.Uid == 44)
			{
				gOut.Print("You try to open the glowing device.");
			}
			else
			{
				base.PrintOpened(artifact);
			}
		}

		public override void PrintLocked(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			// Swallower shark

			if (artifact.Uid == 31)
			{
				gOut.Print("The hide is too hard to cut!");

				NextState = Globals.CreateInstance<IStartState>();
			}
			else
			{
				base.PrintLocked(artifact);
			}
		}

		public override void PrintOpenObjWithKey(IArtifact artifact, IArtifact key)
		{
			Debug.Assert(artifact != null && key != null);

			// Large green device

			if (artifact.Uid == 44)
			{
				gOut.Print("You try to open the glowing device with {0}.", key.GetTheName());
			}
			else
			{
				base.PrintOpenObjWithKey(artifact, key);
			}
		}

		public override void ProcessEvents(EventType eventType)
		{
			// Try to open running device, all flee

			if (eventType == EventType.AfterArtifactOpenPrint && DobjArtifact.Uid == 44)
			{
				Globals.DeviceOpened = true;

				GotoCleanup = true;
			}
			else
			{
				base.ProcessEvents(eventType);
			}
		}

		public override void Execute()
		{
			Debug.Assert(DobjArtifact != null);

			// Large rock

			if (DobjArtifact.Uid == 17)
			{
				PrintCantVerbObj(DobjArtifact);

				NextState = Globals.CreateInstance<IStartState>();
			}
			else
			{
				base.Execute();
			}
		}
	}
}
