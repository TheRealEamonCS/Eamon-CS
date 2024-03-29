﻿
// ExamineCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Eamon.Framework;
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
	public class ExamineCommand : Command, IExamineCommand
	{
		/// <summary></summary>
		public virtual ArtifactType[] ArtTypes { get; set; }

		/// <summary></summary>
		public virtual IList<IArtifact> ContainerArtifactList { get; set; }

		/// <summary></summary>
		public virtual IArtifactCategory DobjArtAc { get; set; }

		/// <summary></summary>
		public virtual IArtifactCategory DobjArtContainerAc { get; set; }

		/// <summary></summary>
		public virtual ArtifactType ContainerArtType { get; set; }

		/// <summary></summary>
		public virtual ICommand RedirectCommand { get; set; }

		/// <summary></summary>
		public virtual ICommand RedirectCommand01 { get; set; }

		/// <summary></summary>
		public virtual bool ShowCharOwned { get; set; }

		/// <summary></summary>
		public virtual bool IsUninjuredGroupMonster { get; set; }

		/// <summary></summary>
		public virtual bool RevealedEmbeddedArtifact { get; set; }

		/// <summary></summary>
		public virtual bool CheckContainerTypeInDobjArtName { get; set; }

		public override void ExecuteForPlayer()
		{
			Debug.Assert(DobjArtifact != null || DobjMonster != null);

			if (DobjMonster != null)
			{
				PrintFullDesc(DobjMonster, false, false);

				DobjMonster.Seen = true;

				if (DobjMonster.Reaction == Friendliness.Friend && DobjMonster.ShouldShowContentsWhenExamined())
				{
					RedirectCommand01 = gEngine.CreateInstance<IInventoryCommand>();

					CopyCommandData(RedirectCommand01);

					NextState = RedirectCommand01;

					goto Cleanup;
				}

				ProcessEvents(EventType.BeforeExamineMonsterHealthStatus);

				if (GotoCleanup)
				{
					goto Cleanup;
				}

				if (DobjMonster.ShouldShowHealthStatusWhenExamined())
				{
					PrintHealthStatus(DobjMonster, true);
				}

				ProcessEvents(EventType.AfterExamineMonsterHealthStatus);

				if (GotoCleanup)
				{
					goto Cleanup;
				}

				goto Cleanup;
			}

			DobjArtAc = DobjArtifact.GetArtifactCategory(ArtTypes, false);

			if (DobjArtAc == null)
			{
				DobjArtAc = DobjArtifact.GetCategory(0);
			}

			Debug.Assert(DobjArtAc != null);

			if (DobjArtifact.IsEmbeddedInRoom(ActorRoom))
			{
				DobjArtifact.SetInRoom(ActorRoom);

				RevealedEmbeddedArtifact = true;
			}

			if (DobjArtAc.Type == ArtifactType.DisguisedMonster)
			{
				if (RevealedEmbeddedArtifact)
				{
					PrintFullDesc(DobjArtifact, false, false);

					DobjArtifact.Seen = true;
				}

				gEngine.RevealDisguisedMonster(ActorRoom, DobjArtifact);

				NextState = gEngine.CreateInstance<IStartState>();

				goto Cleanup;
			}

			if (DobjArtAc.Type == ArtifactType.DoorGate)
			{
				DobjArtAc.Field4 = 0;
			}

			if (!Enum.IsDefined(typeof(ContainerType), ContainerType) || (CheckContainerTypeInDobjArtName && string.Format(" {0} ", DobjArtifact.Name).IndexOf(string.Format(" {0} ", ContainerType.ToString()), StringComparison.OrdinalIgnoreCase) >= 0 && DobjArtifact.GeneralContainer == null) || DobjArtifact.IsWornByMonster(ActorMonster))
			{
				PrintFullDesc(DobjArtifact, false, false);

				DobjArtifact.Seen = true;

				ProcessEvents(EventType.AfterPrintArtifactFullDesc);

				if (GotoCleanup)
				{
					goto Cleanup;
				}

				if ((DobjArtAc.Type == ArtifactType.Drinkable || DobjArtAc.Type == ArtifactType.Edible) && DobjArtAc.Field2 != gEngine.InfiniteDrinkableEdible)
				{
					PrintObjAmountLeft(DobjArtifact, DobjArtAc.Field2, DobjArtAc.Type == ArtifactType.Edible);
				}

				if (((DobjArtAc.Type == ArtifactType.InContainer && (DobjArtAc.IsOpen() || DobjArtifact.ShouldExposeInContentsWhenClosed())) || DobjArtAc.Type == ArtifactType.OnContainer || DobjArtAc.Type == ArtifactType.UnderContainer || DobjArtAc.Type == ArtifactType.BehindContainer) && DobjArtifact.ShouldShowContentsWhenExamined())
				{
					RedirectCommand = gEngine.CreateInstance<IInventoryCommand>(x =>
					{
						x.AllowExtendedContainers = true;
					});

					CopyCommandData(RedirectCommand);

					NextState = RedirectCommand;

					goto Cleanup;
				}

				goto Cleanup;
			}

			ContainerArtType = gEngine.EvalContainerType(ContainerType, ArtifactType.InContainer, ArtifactType.OnContainer, ArtifactType.UnderContainer, ArtifactType.BehindContainer);

			DobjArtContainerAc = DobjArtifact.GetArtifactCategory(ContainerArtType);

			if (DobjArtContainerAc == null)
			{
				PrintYouSeeNothingSpecial();

				goto Cleanup;
			}

			if (DobjArtContainerAc == DobjArtifact.InContainer && !DobjArtContainerAc.IsOpen() && !DobjArtifact.ShouldExposeInContentsWhenClosed())
			{
				PrintMustFirstOpen(DobjArtifact);

				NextState = gEngine.CreateInstance<IStartState>();

				goto Cleanup;
			}

			ContainerArtifactList = DobjArtifact.GetContainedList(containerType: ContainerType);
					
			ShowCharOwned = !DobjArtifact.IsCarriedByMonster(ActorMonster) /* && !DobjArtifact.IsWornByMonster(ActorMonster) */;

			if (ContainerArtifactList.Count > 0)
			{
				PrintPrepContainerYouSee(DobjArtifact, ContainerArtifactList, ContainerType, ShowCharOwned);
			}
			else
			{
				PrintNothingPrepContainer(DobjArtifact, ContainerType, ShowCharOwned);
			}

			ProcessEvents(EventType.AfterPrintArtifactContents);

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

		/*
		public override bool IsPrepEnabled(IPrep prep)
		{
			Debug.Assert(prep != null);

			PrepNames = new string[] { "in", "into", "on", "onto", "under", "behind" };

			return PrepNames.FirstOrDefault(pn => prep.Name.Equals(pn, StringComparison.OrdinalIgnoreCase)) != null;
		}
		*/

		public ExamineCommand()
		{
			SortOrder = 150;

			IsDobjPrepEnabled = true;

			Name = "ExamineCommand";

			Verb = "examine";

			Type = CommandType.Manipulation;

			ArtTypes = new ArtifactType[] { ArtifactType.DisguisedMonster, ArtifactType.DoorGate, ArtifactType.Drinkable, ArtifactType.Edible, ArtifactType.InContainer, ArtifactType.OnContainer, ArtifactType.UnderContainer, ArtifactType.BehindContainer };

			CheckContainerTypeInDobjArtName = true;
		}
	}
}
