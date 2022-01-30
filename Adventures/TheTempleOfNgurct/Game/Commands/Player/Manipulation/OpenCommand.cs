
// OpenCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static TheTempleOfNgurct.Game.Plugin.PluginContext;

namespace TheTempleOfNgurct.Game.Commands
{
	[ClassMappings]
	public class OpenCommand : EamonRT.Game.Commands.OpenCommand, IOpenCommand
	{
		public override void PrintOpened(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			// Chest

			if (artifact.Uid == 54)
			{
				gOut.Print("{0} is open!", artifact.GetTheName(true));
			}

			// Oak door

			else if (artifact.Uid == 85)
			{
				gOut.Print("{0} swings open to your gentle touch.", artifact.GetTheName(true));
			}

			// Cell doors

			else if (artifact.Uid >= 86 && artifact.Uid <= 88)
			{
				gOut.Print("{0} squeaks open.", artifact.GetTheName(true));
			}
			else
			{
				base.PrintOpened(artifact);
			}
		}

		public override void PrintLocked(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			// Chest

			if (artifact.Uid == 54)
			{
				gOut.Print("{0} is locked -- what do you think that padlock is... chopped liver?",	artifact.GetTheName(true));
			}

			// Oak door

			else if (artifact.Uid == 85)
			{
				gOut.Print("{0} is locked shut!", artifact.GetTheName(true));
			}

			// Cell doors

			else if (artifact.Uid >= 86 && artifact.Uid <= 88)
			{
				gOut.Print("{0} is locked!", artifact.GetTheName(true));
			}
			else
			{
				base.PrintLocked(artifact);
			}
		}

		public override void PrintOpenObjWithKey(IArtifact artifact, IArtifact key)
		{
			Debug.Assert(artifact != null && key != null);

			// Chest/oak door/cell doors

			if (artifact.Uid == 54 || artifact.Uid == 85 || (artifact.Uid >= 86 && artifact.Uid <= 88))
			{
				gOut.Print("You unlock {0} with {1}.", artifact.EvalPlural("it", "them"), key.GetTheName());

				PrintOpened(artifact);
			}
			else
			{
				base.PrintOpenObjWithKey(artifact, key);
			}
		}

		public override void PrintCantVerbHere()
		{
			PrintEnemiesNearby();
		}

		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			// If chest opened reveal cobra

			if (eventType == EventType.AfterOpenArtifact && DobjArtifact.Uid == 54 && !gGameState.CobraAppeared)
			{
				var cobraMonster = gMDB[52];

				Debug.Assert(cobraMonster != null);

				cobraMonster.SetInRoom(ActorRoom);

				gGameState.CobraAppeared = true;

				NextState = Globals.CreateInstance<IStartState>();

				GotoCleanup = true;
			}
		}

		public override bool IsAllowedInRoom()
		{
			return gGameState.GetNBTL(Friendliness.Enemy) <= 0;
		}

		public OpenCommand()
		{
			IsPlayerEnabled = true;
		}
	}
}
