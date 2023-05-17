
// CloseCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
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
	public class CloseCommand : Command, ICloseCommand
	{
		/// <summary></summary>
		public virtual IArtifactCategory InContainerAc { get; set; }

		/// <summary></summary>
		public virtual IArtifactCategory DoorGateAc { get; set; }

		/// <summary></summary>
		public virtual IArtifactCategory DrinkableAc { get; set; }

		/// <summary></summary>
		public virtual IArtifactCategory EdibleAc { get; set; }

		/// <summary></summary>
		public virtual IArtifactCategory ReadableAc { get; set; }

		/// <summary></summary>
		public virtual IArtifactCategory DobjArtAc { get; set; }

		public override void ExecuteForPlayer()
		{
			Debug.Assert(DobjArtifact != null);

			InContainerAc = DobjArtifact.InContainer;

			DoorGateAc = DobjArtifact.DoorGate;

			DrinkableAc = DobjArtifact.Drinkable;

			EdibleAc = DobjArtifact.Edible;

			ReadableAc = DobjArtifact.Readable;

			DobjArtAc =	InContainerAc != null ? InContainerAc :
						DoorGateAc != null ? DoorGateAc :
						DrinkableAc != null ? DrinkableAc :
						EdibleAc != null ? EdibleAc :
						ReadableAc;

			if (DobjArtAc == null)
			{
				PrintCantVerbIt(DobjArtifact);

				NextState = gEngine.CreateInstance<IStartState>();

				goto Cleanup;
			}

			if (DobjArtAc.Type == ArtifactType.Drinkable || DobjArtAc.Type == ArtifactType.Edible || DobjArtAc.Type == ArtifactType.Readable || DobjArtAc.GetKeyUid() == -1)
			{
				PrintDontNeedTo();

				NextState = gEngine.CreateInstance<IStartState>();

				goto Cleanup;
			}

			if (DobjArtAc.Type == ArtifactType.DoorGate)
			{
				if (DobjArtifact.Seen)
				{
					DobjArtAc.Field4 = 0;
				}

				if (DobjArtAc.Field4 == 1)
				{
					PrintDontFollowYou();

					NextState = gEngine.CreateInstance<IStartState>();

					goto Cleanup;
				}
			}

			if (DobjArtAc.GetKeyUid() == -2)
			{
				PrintBrokeIt(DobjArtifact);

				goto Cleanup;
			}

			if (!DobjArtAc.IsOpen())
			{
				PrintNotOpen(DobjArtifact);

				NextState = gEngine.CreateInstance<IStartState>();

				goto Cleanup;
			}

			PrintClosed(DobjArtifact);

			DobjArtAc.SetOpen(false);

			ProcessEvents(EventType.AfterCloseArtifact);

			if (GotoCleanup)
			{
				goto Cleanup;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
		}

		public CloseCommand()
		{
			SortOrder = 110;

			if (gEngine.IsRulesetVersion(5, 62))
			{
				IsPlayerEnabled = false;
			}

			Name = "CloseCommand";

			Verb = "close";

			Type = CommandType.Manipulation;
		}
	}
}
