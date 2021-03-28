
// InventoryCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
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
			RetCode rc;

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
					Globals.Buf.SetFormat("{0}{1} {2} you see ",
						Environment.NewLine,
						gEngine.EvalContainerType(DobjArtContainerType, "Inside", "On", "Under", "Behind"),
						DobjArtifact.GetTheName(false, ShowCharOwned, false, false, Globals.Buf01));

					rc = gEngine.GetRecordNameList(DobjArtContainedArtifactList.Cast<IGameBase>().ToList(), ArticleType.A, ShowCharOwned, StateDescDisplayCode.None, false, false, Globals.Buf);

					Debug.Assert(gEngine.IsSuccess(rc));
				}
				else
				{
					Globals.Buf.SetFormat("{0}There's nothing {1} {2}",
						Environment.NewLine,
						gEngine.EvalContainerType(DobjArtContainerType, "inside", "on", "under", "behind"),
						DobjArtifact.GetTheName(false, ShowCharOwned, false, false, Globals.Buf01));
				}

				Globals.Buf.AppendFormat(".{0}", Environment.NewLine);

				gOut.Write("{0}", Globals.Buf);

				goto Cleanup;
			}

			IsCharMonster = DobjMonster.IsCharacterMonster();

			if (!IsCharMonster && DobjMonster.Reaction < Friendliness.Friend)
			{
				gEngine.MonsterEmotes(DobjMonster);

				gOut.WriteLine();

				goto Cleanup;
			}

			HasWornInventory = DobjMonster.HasWornInventory();

			if (HasWornInventory)
			{
				DobjMonsterWornArtifactList = DobjMonster.GetWornList();

				if (DobjMonsterWornArtifactList.Count > 0)
				{
					Globals.Buf.SetFormat("{0}{1} {2} {3}",
						Environment.NewLine,
						IsCharMonster ? "You" : DobjMonster.EvalPlural(DobjMonster.GetTheName(true, true, false, true, Globals.Buf01), "They"),
						IsCharMonster ? "are" : DobjMonster.EvalPlural("is", "are"),
						IsCharMonster ? "wearing " : DobjMonster.EvalPlural("wearing ", "wearing among them "));

					rc = gEngine.GetRecordNameList(DobjMonsterWornArtifactList.Cast<IGameBase>().ToList(), ArticleType.A, IsCharMonster ? false : true, IsCharMonster ? StateDescDisplayCode.AllStateDescs : StateDescDisplayCode.SideNotesOnly, IsCharMonster ? true : false, false, Globals.Buf);

					Debug.Assert(gEngine.IsSuccess(rc));

					Globals.Buf.AppendFormat(".{0}", Environment.NewLine);

					gOut.Write("{0}", Globals.Buf);
				}
			}

			HasCarriedInventory = DobjMonster.HasCarriedInventory();

			if (HasCarriedInventory)
			{
				DobjMonsterCarriedArtifactList = DobjMonster.GetCarriedList();

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

				Globals.Buf.SetFormat("{0}{1} {2} {3}",
					Environment.NewLine,
					IsCharMonster ? "You" : DobjMonster.EvalPlural(DobjMonster.GetTheName(true, true, false, true, Globals.Buf01), "They"),
					IsCharMonster ? "are" : DobjMonster.EvalPlural("is", "are"),
					DobjMonsterCarriedArtifactList.Count == 0 ? "" :
					IsCharMonster ? "carrying " : DobjMonster.EvalPlural("carrying ", "carrying among them "));

				if (DobjMonsterCarriedArtifactList.Count > 0)
				{
					rc = gEngine.GetRecordNameList(DobjMonsterCarriedArtifactList.Cast<IGameBase>().ToList(), ArticleType.A, IsCharMonster ? false : true, IsCharMonster ? StateDescDisplayCode.AllStateDescs : StateDescDisplayCode.SideNotesOnly, IsCharMonster ? true : false, false, Globals.Buf);

					Debug.Assert(gEngine.IsSuccess(rc));
				}
				else
				{
					Globals.Buf.Append("empty handed");
				}

				Globals.Buf.AppendFormat(".{0}", Environment.NewLine);

				gOut.Write("{0}", Globals.Buf);
			}

			ShouldShowHealthStatusWhenInventoried = DobjMonster.ShouldShowHealthStatusWhenInventoried();

			if (ShouldShowHealthStatusWhenInventoried)
			{
				IsUninjuredGroup = DobjMonster.CurrGroupCount > 1 && DobjMonster.DmgTaken == 0;

				Globals.Buf.SetFormat("{0}{1} {2} ",
					Environment.NewLine,
					IsCharMonster ? "You" :
					IsUninjuredGroup ? "They" :
					DobjMonster.GetTheName(true, true, false, true, Globals.Buf01),
					IsCharMonster || IsUninjuredGroup ? "are" : "is");

				DobjMonster.AddHealthStatus(Globals.Buf);

				gOut.Write("{0}", Globals.Buf);
			}

			if (GoldArtifact != null)
			{
				GoldArtifact.Dispose();

				GoldArtifact = null;
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

		public InventoryCommand()
		{
			SortOrder = 320;

			IsDarkEnabled = true;

			Uid = 58;

			Name = "InventoryCommand";

			Verb = "inventory";

			Type = CommandType.Miscellaneous;
		}
	}
}
