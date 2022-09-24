
// InventoryCommand.cs

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
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class InventoryCommand : Command, IInventoryCommand
	{
		public virtual bool AllowExtendedContainers { get; set; }

		/// <summary></summary>
		public virtual IList<IArtifact> DobjArtContainedArtifactList { get; set; }

		/// <summary></summary>
		public virtual IList<IArtifact> DobjMonsterWornArtifactList { get; set; }

		/// <summary></summary>
		public virtual IList<IArtifact> DobjMonsterCarriedArtifactList { get; set; }

		/// <summary></summary>
		public virtual IArtifactCategory DobjArtAc { get; set; }

		/// <summary></summary>
		public virtual IArtifact GoldArtifact { get; set; }

		/// <summary></summary>
		public virtual ContainerType DobjArtContainerType { get; set; }

		/// <summary></summary>
		public virtual long TotalGold { get; set; }

		/// <summary></summary>
		public virtual bool ShowCharOwned { get; set; }

		/// <summary></summary>
		public virtual bool IsCharMonster { get; set; }

		/// <summary></summary>
		public virtual bool HasWornInventory { get; set; }

		/// <summary></summary>
		public virtual bool HasCarriedInventory { get; set; }

		/// <summary></summary>
		public virtual bool ShouldShowHealthStatusWhenInventoried { get; set; }

		/// <summary></summary>
		public virtual bool IsUninjuredGroup { get; set; }

		public override void Execute()
		{
			Debug.Assert(DobjArtifact != null || DobjMonster != null);

			if (!ActorRoom.IsLit())
			{
				Debug.Assert(DobjMonster != null && DobjMonster.IsCharacterMonster());
			}

			if (DobjArtifact != null)
			{
				DobjArtAc = DobjArtifact.InContainer;

				if (DobjArtAc == null)
				{
					DobjArtAc = DobjArtifact.OnContainer;
				}

				if (DobjArtAc == null && AllowExtendedContainers)
				{
					DobjArtAc = DobjArtifact.UnderContainer;
				}

				if (DobjArtAc == null && AllowExtendedContainers)
				{
					DobjArtAc = DobjArtifact.BehindContainer;
				}

				if (DobjArtAc == null)
				{
					PrintCantVerbObj(DobjArtifact);

					NextState = Globals.CreateInstance<IStartState>();

					goto Cleanup;
				}

				DobjArtContainerType = gEngine.GetContainerType(DobjArtAc.Type);

				if (DobjArtifact.IsEmbeddedInRoom(ActorRoom))
				{
					DobjArtifact.SetInRoom(ActorRoom);
				}

				if (DobjArtAc == DobjArtifact.InContainer && !DobjArtAc.IsOpen() && !DobjArtifact.ShouldExposeInContentsWhenClosed())
				{
					PrintMustFirstOpen(DobjArtifact);

					NextState = Globals.CreateInstance<IStartState>();

					goto Cleanup;
				}

				DobjArtContainedArtifactList = DobjArtifact.GetContainedList(containerType: DobjArtContainerType);

				ShowCharOwned = !DobjArtifact.IsCarriedByCharacter() && !DobjArtifact.IsWornByCharacter();

				if (DobjArtContainedArtifactList.Count > 0)
				{
					PrintPrepContainerYouSee(DobjArtifact, DobjArtContainedArtifactList, DobjArtContainerType, ShowCharOwned);
				}
				else
				{
					PrintNothingPrepContainer(DobjArtifact, DobjArtContainerType, ShowCharOwned);
				}

				goto Cleanup;
			}

			IsCharMonster = DobjMonster.IsCharacterMonster();

			if (!IsCharMonster && DobjMonster.Reaction < Friendliness.Friend)
			{
				gEngine.PrintMonsterEmotes(DobjMonster);

				gOut.WriteLine();

				goto Cleanup;
			}

			HasWornInventory = DobjMonster.HasWornInventory();

			if (HasWornInventory)
			{
				DobjMonsterWornArtifactList = DobjMonster.GetWornList();

				if (DobjMonsterWornArtifactList.Count > 0)
				{
					PrintActorIsWearing(DobjMonster, DobjMonsterWornArtifactList);
				}
			}

			HasCarriedInventory = DobjMonster.HasCarriedInventory();

			if (HasCarriedInventory)
			{
				DobjMonsterCarriedArtifactList = DobjMonster.GetCarriedList();

				CreateGoldArtifactIfNecessary();

				PrintActorIsCarrying(DobjMonster, DobjMonsterCarriedArtifactList);

				DestroyGoldArtifactIfNecessary();
			}

			ProcessEvents(EventType.BeforeInventoryMonsterHealthStatus);

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			ShouldShowHealthStatusWhenInventoried = DobjMonster.ShouldShowHealthStatusWhenInventoried();

			if (ShouldShowHealthStatusWhenInventoried)
			{
				PrintHealthStatus(DobjMonster, true);
			}

			ProcessEvents(EventType.AfterInventoryMonsterHealthStatus);

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			if (!HasWornInventory && !HasCarriedInventory && !ShouldShowHealthStatusWhenInventoried)
			{
				PrintCantVerbObj(DobjMonster);

				NextState = Globals.CreateInstance<IStartState>();

				goto Cleanup;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		public virtual void CreateGoldArtifactIfNecessary()
		{
			if (IsCharMonster)
			{
				// use total debt for characters with no assets; otherwise use HeldGold (which may be debt or asset)

				TotalGold = gCharacter.HeldGold < 0 && gCharacter.BankGold < 0 ? gCharacter.HeldGold + gCharacter.BankGold : gCharacter.HeldGold;

				if (TotalGold != 0)
				{
					GoldArtifact = Globals.CreateInstance<IArtifact>(x =>
					{
						x.Name = string.Format("{0}{1} gold piece{2}",
										TotalGold < 0 ? "a debt of " : "",
										gEngine.GetStringFromNumber(Math.Abs(TotalGold), false, Globals.Buf),
										Math.Abs(TotalGold) != 1 ? "s" : "");
					});

					DobjMonsterCarriedArtifactList.Add(GoldArtifact);
				}
			}
		}

		public virtual void DestroyGoldArtifactIfNecessary()
		{
			if (GoldArtifact != null)
			{
				DobjMonsterCarriedArtifactList.Remove(GoldArtifact);

				GoldArtifact.Dispose();

				GoldArtifact = null;
			}
		}

		public InventoryCommand()
		{
			SortOrder = 320;

			IsDarkEnabled = true;

			Name = "InventoryCommand";

			Verb = "inventory";

			Type = CommandType.Miscellaneous;
		}
	}
}
