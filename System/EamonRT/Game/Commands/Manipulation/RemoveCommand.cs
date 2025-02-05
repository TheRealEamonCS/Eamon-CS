
// RemoveCommand.cs

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
	public class RemoveCommand : Command, IRemoveCommand
	{
		/// <summary></summary>
		public virtual ArtifactType[] ArtTypes { get; set; }

		/// <summary></summary>
		public virtual IList<IMonster> FumbleMonsterList { get; set; }

		/// <summary></summary>
		public virtual IArtifactCategory ArmorArtifactAc { get; set; }

		/// <summary></summary>
		public virtual IArtifactCategory ShieldArtifactAc { get; set; }

		/// <summary></summary>
		public virtual IArtifactCategory DobjArtAc { get; set; }

		/// <summary></summary>
		public virtual IArtifact ArmorArtifact { get; set; }

		/// <summary></summary>
		public virtual IArtifact ShieldArtifact { get; set; }

		/// <summary></summary>
		public virtual string MonsterName { get; set; }

		/// <summary></summary>
		public virtual bool OmitWeightCheck { get; set; }

		public override void ExecuteForPlayer()
		{
			Debug.Assert(DobjArtifact != null);

			if (IobjArtifact != null)
			{
				if (!GetCommandCalled)
				{
					RedirectToGetCommand<IRemoveCommand>(DobjArtifact, false);

					goto Cleanup;
				}

				if (!DobjArtifact.IsCarriedByMonster(ActorMonster))
				{
					if (DobjArtifact.DisguisedMonster == null)
					{
						NextState = gEngine.CreateInstance<IStartState>();
					}

					goto Cleanup;
				}

				if (ActorMonster.Weapon <= 0 && DobjArtifact.IsReadyableByMonster(ActorMonster) && NextState == null)
				{
					NextState = gEngine.CreateInstance<IReadyCommand>();

					CopyCommandData(NextState as ICommand, false);

					goto Cleanup;
				}

				goto Cleanup;
			}

			ArmorArtifact = gADB[gGameState.Ar];

			ShieldArtifact = gADB[gGameState.Sh];

			ArmorArtifactAc = ArmorArtifact != null ? ArmorArtifact.Wearable : null;

			ShieldArtifactAc = ShieldArtifact != null ? ShieldArtifact.Wearable : null;

			ProcessEvents(EventType.BeforeRemoveWornArtifact);

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			if (DobjArtifact.Uid == gGameState.Sh)
			{
				ActorMonster.Armor = ArmorArtifactAc != null ? (ArmorArtifactAc.Field1 / 2) + ((ArmorArtifactAc.Field1 / 2) >= 3 ? 2 : 0) : 0;

				gGameState.Sh = 0;
			}

			if (DobjArtifact.Uid == gGameState.Ar)
			{
				ActorMonster.Armor = ShieldArtifactAc != null ? ShieldArtifactAc.Field1 : 0;

				gGameState.Ar = 0;
			}

			DobjArtifact.SetCarriedByMonster(ActorMonster);

			PrintRemoved(DobjArtifact);

			ProcessEvents(EventType.AfterRemoveWornArtifact);

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

		public override void ExecuteForMonster()
		{
			Debug.Assert(DobjArtifact != null && IobjArtifact != null && Prep != null && Enum.IsDefined(typeof(ContainerType), Prep.ContainerType));

			Debug.Assert(DobjArtifact.IsCarriedByContainer(IobjArtifact) && DobjArtifact.GetCarriedByContainerContainerType() == Prep.ContainerType);

			DobjArtAc = DobjArtifact.GetArtifactCategory(ArtTypes, false);

			if (DobjArtAc == null)
			{
				DobjArtAc = DobjArtifact.GetCategory(0);
			}

			if (DobjArtAc != null && DobjArtAc.Type != ArtifactType.DisguisedMonster && !DobjArtifact.IsUnmovable() && (DobjArtAc.Type != ArtifactType.DeadBody || DobjArtAc.Field1 == 1) && DobjArtAc.Type != ArtifactType.BoundMonster)
			{
				OmitWeightCheck = DobjArtifact.IsCarriedByMonster(ActorMonster, true);

				if (!gEngine.EnforceMonsterWeightLimits || OmitWeightCheck || ActorMonster.CanCarryArtifactWeight(DobjArtifact))
				{
					DobjArtifact.SetCarriedByMonster(ActorMonster);

					Debug.Assert(gCharMonster != null);

					if (gCharMonster.IsInRoom(ActorRoom))
					{
						if (ActorRoom.IsViewable())
						{
							PrintActorRemovesObjPrepContainer(ActorMonster, DobjArtifact, IobjArtifact, Prep.ContainerType, OmitWeightCheck);
						}
						else
						{
							PrintActorRemovesObjPrepContainer01(ActorMonster, DobjArtifact, IobjArtifact, Prep.ContainerType, OmitWeightCheck);
						}
					}

					// When a weapon is picked up all monster affinities to that weapon are broken

					FumbleMonsterList = gEngine.GetMonsterList(m => m.Weapon == -DobjArtifact.Uid - 1 && m != ActorMonster);

					foreach (var monster in FumbleMonsterList)
					{
						monster.Weapon = -1;
					}
				}
			}

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IErrorState>(x =>
				{
					x.ErrorMessage = string.Format("{0}: NextState == null", Name);
				});
			}
		}

		/*
		public override bool IsPrepEnabled(IPrep prep)
		{
			Debug.Assert(prep != null);

			PrepNames = new string[] { "in", "fromin", "on", "fromon", "under", "fromunder", "behind", "frombehind", "from" };

			return PrepNames.FirstOrDefault(pn => prep.Name.Equals(pn, StringComparison.OrdinalIgnoreCase)) != null;
		}
		*/

		public RemoveCommand()
		{
			SortOrder = 220;

			IsIobjEnabled = true;

			if (gEngine.IsRulesetVersion(5, 62))
			{
				IsPlayerEnabled = false;
			}

			if (!gEngine.IsRulesetVersion(5, 62))
			{
				IsMonsterEnabled = true;
			}

			Name = "RemoveCommand";

			Verb = "remove";

			Type = CommandType.Manipulation;

			ArtTypes = new ArtifactType[] { ArtifactType.DisguisedMonster, ArtifactType.DeadBody, ArtifactType.BoundMonster, ArtifactType.Weapon, ArtifactType.MagicWeapon };
		}
	}
}
