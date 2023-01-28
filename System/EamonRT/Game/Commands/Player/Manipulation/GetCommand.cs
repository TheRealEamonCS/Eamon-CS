
// GetCommand.cs

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
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.Globals;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class GetCommand : Command, IGetCommand
	{
		public long _dobjArtifactCount;

		public long _dobjArtifactWeight;

		public long _actorMonsterInventoryWeight;

		public bool _newlineFlag;

		public virtual bool GetAll { get; set; }

		/// <summary></summary>
		public virtual ArtifactType[] ArtTypes { get; set; }

		/// <summary></summary>
		public virtual IList<IMonster> FumbleMonsterList { get; set; }

		/// <summary></summary>
		public virtual IList<IArtifact> TakenArtifactList { get; set; }

		/// <summary></summary>
		public virtual IArtifactCategory DobjArtAc { get; set; }

		/// <summary></summary>
		public virtual IArtifactCategory WeaponArtifactAc { get; set; }

		/// <summary></summary>
		public virtual IMonster WeaponAffinityMonster { get; set; }

		/// <summary></summary>
		public virtual IArtifact WeaponArtifact { get; set; }

		/// <summary></summary>
		public virtual ICommand RedirectCommand { get; set; }

		/// <summary></summary>
		public virtual long DobjArtifactCount
		{
			get
			{
				return _dobjArtifactCount;
			}

			set
			{
				_dobjArtifactCount = value;
			}
		}

		/// <summary></summary>
		public virtual long DobjArtifactWeight
		{
			get
			{
				return _dobjArtifactWeight;
			}

			set
			{
				_dobjArtifactWeight = value;
			}
		}

		/// <summary></summary>
		public virtual long ActorMonsterInventoryWeight
		{
			get
			{
				return _actorMonsterInventoryWeight;
			}

			set
			{
				_actorMonsterInventoryWeight = value;
			}
		}

		/// <summary></summary>
		public virtual bool OmitWeightCheck { get; set; }

		/// <summary></summary>
		public virtual bool IsCarriedByContainer { get; set; }

		/// <summary></summary>
		public virtual bool NewlineFlag
		{
			get
			{
				return _newlineFlag;
			}

			set
			{
				_newlineFlag = value;
			}
		}

		public override void Execute()
		{
			Debug.Assert(GetAll || DobjArtifact != null);

			if (GetAll)
			{
				// screen out all weapons in the room which have monsters present with affinities to those weapons

				TakenArtifactList = ActorRoom.GetTakeableList().Where(a => gEngine.GetMonsterList(m => m.IsInRoom(ActorRoom) && m.Weapon == -a.Uid - 1 && m != ActorMonster).FirstOrDefault() == null).ToList();
			}
			else
			{
				TakenArtifactList = new List<IArtifact>() { DobjArtifact };
			}

			if (TakenArtifactList.Count <= 0)
			{
				PrintNothingToGet();

				NextState = gEngine.CreateInstance<IStartState>();

				goto Cleanup;
			}

			ArtTypes = new ArtifactType[] { ArtifactType.DisguisedMonster, ArtifactType.DeadBody, ArtifactType.BoundMonster, ArtifactType.Weapon, ArtifactType.MagicWeapon };

			NewlineFlag = false;

			foreach (var artifact in TakenArtifactList)
			{
				DobjArtAc = artifact.GetArtifactCategory(ArtTypes, false);

				if (DobjArtAc == null)
				{
					DobjArtAc = artifact.GetCategory(0);
				}

				Debug.Assert(DobjArtAc != null);

				OmitWeightCheck = artifact.IsCarriedByCharacter(true);

				ProcessArtifact(artifact, DobjArtAc, ref _newlineFlag);

				if (artifact.IsCarriedByCharacter())
				{
					// when a weapon is picked up all monster affinities to that weapon are broken

					FumbleMonsterList = gEngine.GetMonsterList(m => m.Weapon == -artifact.Uid - 1 && m != ActorMonster);

					foreach (var monster in FumbleMonsterList)
					{
						monster.Weapon = -1;
					}

					WeaponArtifactAc = artifact.GeneralWeapon;

					if (artifact.IsReadyableByCharacter() && (WeaponArtifact == null || gEngine.WeaponPowerCompare(artifact, WeaponArtifact) > 0) && (!GetAll || TakenArtifactList.Count == 1 || gGameState.Sh < 1 || WeaponArtifactAc.Field5 < 2))
					{
						WeaponArtifact = artifact;
					}
				}
			}

			if (NewlineFlag)
			{
				gOut.WriteLine();

				NewlineFlag = false;
			}

			if (ActorRoom.IsLit())
			{
				if (!gEngine.AutoDisplayUnseenArtifactDescs && !GetAll && DobjArtifact.IsCarriedByCharacter() && !DobjArtifact.Seen)
				{
					PrintFullDesc(DobjArtifact, false, false);

					DobjArtifact.Seen = true;
				}
			}

			if (ActorMonster.Weapon <= 0 && WeaponArtifact != null && NextState == null)
			{
				RedirectCommand = gEngine.CreateInstance<IReadyCommand>();

				CopyCommandData(RedirectCommand);

				RedirectCommand.Dobj = WeaponArtifact;

				NextState = RedirectCommand;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
		}

		public virtual void ProcessArtifact(IArtifact artifact, IArtifactCategory ac, ref bool nlFlag)
		{
			RetCode rc;

			Debug.Assert(artifact != null);

			Debug.Assert(ac != null);

			if (ac.Type == ArtifactType.DisguisedMonster)
			{
				ProcessAction(() => gEngine.RevealDisguisedMonster(ActorRoom, artifact), ref nlFlag);
			}
			else if (artifact.Weight > 900)
			{
				ProcessAction(() => PrintDontBeAbsurd(), ref nlFlag);
			}
			else if (artifact.IsUnmovable01())
			{
				ProcessAction(() => PrintCantVerbThat(artifact), ref nlFlag);
			}
			else
			{
				DobjArtifactCount = 0;

				DobjArtifactWeight = artifact.Weight;

				if (artifact.GeneralContainer != null)
				{
					rc = artifact.GetContainerInfo(ref _dobjArtifactCount, ref _dobjArtifactWeight, ContainerType.In, true);

					Debug.Assert(gEngine.IsSuccess(rc));

					rc = artifact.GetContainerInfo(ref _dobjArtifactCount, ref _dobjArtifactWeight, ContainerType.On, true);

					Debug.Assert(gEngine.IsSuccess(rc));
				}

				ActorMonsterInventoryWeight = 0;

				rc = ActorMonster.GetFullInventoryWeight(ref _actorMonsterInventoryWeight, recurse: true);

				Debug.Assert(gEngine.IsSuccess(rc));

				if (ac.Type == ArtifactType.DeadBody && ac.Field1 != 1)
				{
					ProcessAction(() => PrintBestLeftAlone(artifact), ref nlFlag);
				}
				else if (!OmitWeightCheck && (DobjArtifactWeight + ActorMonsterInventoryWeight > ActorMonster.GetWeightCarryableGronds()))
				{
					ProcessAction(() => PrintTooHeavy(artifact, GetAll), ref nlFlag);
				}
				else if (ac.Type == ArtifactType.BoundMonster)
				{
					ProcessAction(() => PrintMustBeFreed(artifact), ref nlFlag);
				}
				else
				{
					WeaponAffinityMonster = gEngine.GetMonsterList(m => m.IsInRoom(ActorRoom) && m.Weapon == -artifact.Uid - 1 && m != ActorMonster).FirstOrDefault();

					if (WeaponAffinityMonster != null)
					{
						ProcessAction(() => PrintObjBelongsToActor(artifact, WeaponAffinityMonster), ref nlFlag);
					}
					else
					{
						IsCarriedByContainer = artifact.IsCarriedByContainer();

						artifact.SetCarriedByCharacter();

						if (NextState is IRequestCommand)
						{
							PrintReceived(artifact);
						}
						else if (NextState is IRemoveCommand || IsCarriedByContainer)
						{
							PrintRetrieved(artifact);
						}
						else
						{
							PrintTaken(artifact, GetAll);
						}

						nlFlag = true;
					}
				}
			}
		}

		public virtual void ProcessAction(Action action, ref bool nlFlag)
		{
			Debug.Assert(action != null);

			if (nlFlag)
			{
				gOut.WriteLine();

				nlFlag = false;
			}

			action();

			if (!PreserveNextState && NextState != null)
			{
				NextState = null;
			}
		}

		public GetCommand()
		{
			Synonyms = new string[] { "take" };

			SortOrder = 160;

			Name = "GetCommand";

			Verb = "get";

			Type = CommandType.Manipulation;
		}
	}
}
