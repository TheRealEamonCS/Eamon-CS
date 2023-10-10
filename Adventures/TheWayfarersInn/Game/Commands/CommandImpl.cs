
// CommandImpl.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Linq;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using static TheWayfarersInn.Game.Plugin.Globals;

namespace TheWayfarersInn.Game.Commands
{
	[ClassMappings]
	public class CommandImpl : EamonRT.Game.Commands.CommandImpl, ICommandImpl
	{
		public override bool IsPlayerEnabled 
		{
			get
			{
				var result = base.IsPlayerEnabled;

				// Disable (nearly) all Commands if MatureContent setting is false; player must opt in

				if (!(Command is IQuitCommand || Command is IRestoreCommand || Command is ISettingsCommand) && !gGameState.MatureContent)
				{
					result = false;
				}

				return result;
			}

			set
			{
				base.IsPlayerEnabled = value;
			}
		}

		public override void BuildPrepContainerYouSeePrefix(IArtifact artifact, ContainerType containerType, bool showCharOwned)
		{
			Debug.Assert(artifact != null && Enum.IsDefined(typeof(ContainerType), containerType));

			// Burlap sack

			if (artifact.Uid == 34)
			{
				gEngine.Buf.SetFormat("{0}{1} the unique void fragments of {2}, you see ",
					Environment.NewLine,
					gEngine.EvalContainerType(containerType, "Inside", "On", "Under", "Behind"),
					artifact.GetTheName(showCharOwned: showCharOwned));
			}
			else
			{
				base.BuildPrepContainerYouSeePrefix(artifact, containerType, showCharOwned);
			}
		}

		public override void PrintDoYouMeanObj1OrObj2(IGameBase obj1, IGameBase obj2)
		{
			Debug.Assert(obj1 != null && obj2 != null);

			var artifactUids = new long[] { 7, 8, 9, 10 };

			// Toolshed / Small Barn / Stable / Kennel

			if (artifactUids.Contains(obj1.Uid) && artifactUids.Contains(obj2.Uid))
			{
				gOut.Print("Do you mean \"{0} door\" or \"{1} door\"?", obj1.GetNoneName(showCharOwned: false), obj2.GetNoneName(showCharOwned: false));
			}
			else
			{
				base.PrintDoYouMeanObj1OrObj2(obj1, obj2);
			}
		}

		public override void PrintOpened(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			var artifactUids = new long[] { 7, 8, 9, 10, 20 };

			// Toolshed / Small Barn / Stable / Kennel / Temple

			if (artifactUids.Contains(artifact.Uid))
			{
				gOut.Print("{0} door opened.", artifact.GetNoneName(true, false));
			}
			else
			{
				base.PrintOpened(artifact);
			}
		}

		public override void PrintClosed(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			var artifactUids = new long[] { 7, 8, 9, 10, 20 };

			// Toolshed / Small Barn / Stable / Kennel / Temple

			if (artifactUids.Contains(artifact.Uid))
			{
				gOut.Print("{0} door closed.", artifact.GetNoneName(true, false));
			}
			else
			{
				base.PrintClosed(artifact);
			}
		}

		public override void PrintNothingPrepContainer(IArtifact artifact, ContainerType containerType, bool showCharOwned)
		{
			Debug.Assert(artifact != null);

			// Registration desk

			if (artifact.Uid == 26 && containerType == ContainerType.In)
			{
				gOut.Print("You can see the floor through the rotted-out bottom of the drawer.");
			}
			else
			{
				base.PrintNothingPrepContainer(artifact, containerType, showCharOwned);
			}
		}

		public override void PrintPutObjPrepContainer(IArtifact artifact, IArtifact container, ContainerType containerType)
		{
			Debug.Assert(artifact != null && container != null && Enum.IsDefined(typeof(ContainerType), containerType));

			var shelfArtifactList = container.Uid == 173 ? gEngine.GetArtifactList(a => a.IsCarriedByContainerUid(173) && a.GetCarriedByContainerContainerType() == ContainerType.On) : null;

			// Put plates on kitchen shelf

			if (artifact.Uid == 174 && container.Uid == 173 && gGameState.KitchenRiddleState == 1 && shelfArtifactList.Count == 1)
			{
				artifact.Weight = -999;

				gEngine.PrintEffectDesc(120);

				gGameState.KitchenRiddleState++;
			}

			// Put utensils on kitchen shelf

			else if (artifact.Uid == 175 && container.Uid == 173 && gGameState.KitchenRiddleState == 2 && shelfArtifactList.Count == 2 && shelfArtifactList.FirstOrDefault(a => a.Uid == 174) != null)
			{
				artifact.Weight = -999;

				gEngine.PrintEffectDesc(121);

				gGameState.KitchenRiddleState++;
			}
			else
			{
				base.PrintPutObjPrepContainer(artifact, container, containerType);
			}
		}
	}
}
