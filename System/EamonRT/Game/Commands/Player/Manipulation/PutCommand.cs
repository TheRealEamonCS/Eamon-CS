
// PutCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using System.Diagnostics;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class PutCommand : Command, IPutCommand
	{
		public long _iobjArtifactCount;

		public long _iobjArtifactWeight;

		/// <summary></summary>
		public virtual IList<IArtifact> DobjAllContainerArtifactList { get; set; }

		/// <summary></summary>
		public virtual IArtifactCategory IobjArtAc { get; set; }

		/// <summary></summary>
		public virtual long IobjArtifactCount
		{
			get
			{
				return _iobjArtifactCount;
			}

			set
			{
				_iobjArtifactCount = value;
			}
		}

		/// <summary></summary>
		public virtual long IobjArtifactWeight
		{
			get
			{
				return _iobjArtifactWeight;
			}

			set
			{
				_iobjArtifactWeight = value;
			}
		}

		/// <summary></summary>
		public virtual bool MaxItemsReached { get; set; }

		public override void Execute()
		{
			RetCode rc;

			Debug.Assert(DobjArtifact != null && IobjArtifact != null);

			if (!DobjArtifact.IsCarriedByCharacter())
			{
				if (!GetCommandCalled)
				{
					RedirectToGetCommand<IPutCommand>(DobjArtifact);
				}
				else if (DobjArtifact.DisguisedMonster == null)
				{
					NextState = Globals.CreateInstance<IStartState>();
				}

				goto Cleanup;
			}

			IobjArtAc = gEngine.EvalContainerType(ContainerType, IobjArtifact.InContainer, IobjArtifact.OnContainer, IobjArtifact.UnderContainer, IobjArtifact.BehindContainer);

			DobjAllContainerArtifactList = DobjArtifact.GetContainedList(containerType: (ContainerType)(-1), recurse: true);

			DobjAllContainerArtifactList.Add(DobjArtifact);

			if (DobjAllContainerArtifactList.Contains(IobjArtifact) || IobjArtAc == null)
			{
				PrintDontFollowYou();

				NextState = Globals.CreateInstance<IStartState>();

				goto Cleanup;
			}

			if ((IobjArtifact.IsCarriedByCharacter() && !IobjArtifact.ShouldAddContentsWhenCarried(ContainerType)) || (IobjArtifact.IsWornByCharacter() && !IobjArtifact.ShouldAddContentsWhenWorn(ContainerType)))
			{
				if (IobjArtifact.IsCarriedByCharacter())
				{
					PrintNotWhileCarryingObj(IobjArtifact);
				}
				else
				{
					PrintNotWhileWearingObj(IobjArtifact);
				}

				NextState = Globals.CreateInstance<IStartState>();

				goto Cleanup;
			}

			if (IobjArtAc == IobjArtifact.InContainer && !IobjArtAc.IsOpen() && !IobjArtifact.ShouldExposeInContentsWhenClosed())
			{
				PrintMustFirstOpen(IobjArtifact);

				NextState = Globals.CreateInstance<IStartState>();

				goto Cleanup;
			}

			if ((IobjArtAc == IobjArtifact.InContainer && IobjArtAc.GetKeyUid() == -2) || (IobjArtAc == IobjArtifact.OnContainer && IobjArtifact.InContainer != null && IobjArtifact.InContainer.GetKeyUid() == -2 && IobjArtifact.IsInContainerOpenedFromTop()))
			{
				PrintBrokeIt(IobjArtifact);

				goto Cleanup;
			}

			if (IobjArtAc == IobjArtifact.OnContainer && IobjArtifact.InContainer != null && IobjArtifact.InContainer.IsOpen() && IobjArtifact.IsInContainerOpenedFromTop())
			{
				PrintMustFirstClose(IobjArtifact);

				NextState = Globals.CreateInstance<IStartState>();

				goto Cleanup;
			}

			IobjArtifactCount = 0;

			IobjArtifactWeight = 0;

			rc = IobjArtifact.GetContainerInfo(ref _iobjArtifactCount, ref _iobjArtifactWeight, ContainerType, false);

			Debug.Assert(gEngine.IsSuccess(rc));

			if (IobjArtAc.Field3 < 1 || IobjArtAc.Field4 < 1)
			{
				PrintDontNeedTo();

				NextState = Globals.CreateInstance<IStartState>();

				goto Cleanup;
			}

			MaxItemsReached = IobjArtifactCount >= IobjArtAc.Field4;

			if ((!MaxItemsReached && IobjArtifactWeight + DobjArtifact.Weight > IobjArtAc.Field3) || DobjArtifact.Weight > IobjArtAc.Field3 || !IobjArtifact.ShouldAddContents(DobjArtifact, ContainerType))
			{
				PrintWontFit(DobjArtifact);

				goto Cleanup;
			}

			if (MaxItemsReached || IobjArtifactWeight + DobjArtifact.Weight > IobjArtAc.Field3)
			{
				if (IobjArtAc == IobjArtifact.InContainer)
				{
					PrintFull(IobjArtifact);
				}
				else
				{
					PrintOutOfSpace(IobjArtifact);
				}

				goto Cleanup;
			}

			DobjArtifact.SetCarriedByContainer(IobjArtifact, ContainerType);

			if (gGameState.Ls == DobjArtifact.Uid)
			{
				Debug.Assert(DobjArtifact.LightSource != null);

				gEngine.LightOut(DobjArtifact);
			}

			if (ActorMonster.Weapon == DobjArtifact.Uid)
			{
				Debug.Assert(DobjArtifact.GeneralWeapon != null);

				rc = DobjArtifact.RemoveStateDesc(DobjArtifact.GetReadyWeaponDesc());

				Debug.Assert(gEngine.IsSuccess(rc));

				ActorMonster.Weapon = -1;
			}

			PrintPutObjPrepContainer(DobjArtifact, IobjArtifact, ContainerType);

			ProcessEvents(EventType.AfterPutArtifact);

			if (GotoCleanup)
			{
				goto Cleanup;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		/*
		public override bool IsPrepEnabled(IPrep prep)
		{
			Debug.Assert(prep != null);

			PrepNames = new string[] { "in", "into", "on", "onto", "under", "behind" };

			return PrepNames.FirstOrDefault(pn => prep.Name.Equals(pn, StringComparison.OrdinalIgnoreCase)) != null;
		}
		*/

		public PutCommand()
		{
			SortOrder = 190;

			IsIobjEnabled = true;

			if (Globals.IsRulesetVersion(5))
			{
				IsPlayerEnabled = false;
			}

			Uid = 50;

			Name = "PutCommand";

			Verb = "put";

			Type = CommandType.Manipulation;
		}
	}
}
