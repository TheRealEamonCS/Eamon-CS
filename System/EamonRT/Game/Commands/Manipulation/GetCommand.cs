
// GetCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

		public override void ExecuteForPlayer()
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

			NewlineFlag = false;

			foreach (var artifact in TakenArtifactList)
			{
				DobjArtAc = artifact.GetArtifactCategory(ArtTypes, false);

				if (DobjArtAc == null)
				{
					DobjArtAc = artifact.GetCategory(0);
				}

				Debug.Assert(DobjArtAc != null);

				OmitWeightCheck = artifact.IsCarriedByMonster(ActorMonster, true);

				ProcessArtifact(artifact, DobjArtAc, ref _newlineFlag);

				if (artifact.IsCarriedByMonster(ActorMonster))
				{
					// when a weapon is picked up all monster affinities to that weapon are broken

					FumbleMonsterList = gEngine.GetMonsterList(m => m.Weapon == -artifact.Uid - 1 && m != ActorMonster);

					foreach (var monster in FumbleMonsterList)
					{
						monster.Weapon = -1;
					}

					WeaponArtifactAc = artifact.GeneralWeapon;

					if (artifact.IsReadyableByMonster(ActorMonster) && (WeaponArtifact == null || gEngine.WeaponPowerCompare(artifact, WeaponArtifact) > 0) && (!GetAll || TakenArtifactList.Count == 1 || gGameState.Sh < 1 || WeaponArtifactAc.Field5 < 2))
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
				if (!gEngine.AutoDisplayUnseenArtifactDescs && !GetAll && DobjArtifact.IsCarriedByMonster(ActorMonster) && !DobjArtifact.Seen)
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

		public override void ExecuteForMonster()
		{
			Debug.Assert(DobjArtifact != null);

			DobjArtAc = DobjArtifact.GetArtifactCategory(ArtTypes, false);

			if (DobjArtAc == null)
			{
				DobjArtAc = DobjArtifact.GetCategory(0);
			}

			if (DobjArtAc != null && DobjArtAc.Type != ArtifactType.DisguisedMonster && DobjArtifact.Weight <= 900 && !DobjArtifact.IsUnmovable01() && (DobjArtAc.Type != ArtifactType.DeadBody || DobjArtAc.Field1 == 1) && DobjArtAc.Type != ArtifactType.BoundMonster)
			{
				OmitWeightCheck = DobjArtifact.IsCarriedByMonster(ActorMonster, true);

				if (!gEngine.EnforceMonsterWeightLimits || OmitWeightCheck || ActorMonster.CanCarryArtifactWeight(DobjArtifact))
				{
					DobjArtifact.SetCarriedByMonster(ActorMonster);

					Debug.Assert(gCharMonster != null);

					if (gCharMonster.IsInRoom(ActorRoom))
					{
						if (ActorRoom.IsLit())
						{
							PrintActorPicksUpObj(ActorMonster, DobjArtifact);
						}
						else
						{
							PrintActorPicksUpWeapon(ActorMonster);
						}
					}

					// when a weapon is picked up all monster affinities to that weapon are broken

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

		public virtual void ProcessArtifact(IArtifact artifact, IArtifactCategory ac, ref bool nlFlag)
		{
			Debug.Assert(artifact != null);

			Debug.Assert(ac != null);

			if (ac.Type == ArtifactType.DisguisedMonster)
			{
				ProcessAction(1, () => gEngine.RevealDisguisedMonster(ActorRoom, artifact), ref nlFlag);
			}
			else if (artifact.Weight > 900)
			{
				ProcessAction(2, () => PrintDontBeAbsurd(), ref nlFlag);
			}
			else if (artifact.IsUnmovable01())
			{
				ProcessAction(3, () => PrintCantVerbThat(artifact), ref nlFlag);
			}
			else if (ac.Type == ArtifactType.DeadBody && ac.Field1 != 1)
			{
				ProcessAction(4, () => PrintBestLeftAlone(artifact), ref nlFlag);
			}
			else if (!OmitWeightCheck && !ActorMonster.CanCarryArtifactWeight(artifact))
			{
				ProcessAction(5, () => PrintTooHeavy(artifact, GetAll), ref nlFlag);
			}
			else if (ac.Type == ArtifactType.BoundMonster)
			{
				ProcessAction(6, () => PrintMustBeFreed(artifact), ref nlFlag);
			}
			else
			{
				WeaponAffinityMonster = gEngine.GetMonsterList(m => m.IsInRoom(ActorRoom) && m.Weapon == -artifact.Uid - 1 && m != ActorMonster).FirstOrDefault();

				if (WeaponAffinityMonster != null)
				{
					ProcessAction(7, () => PrintObjBelongsToActor(artifact, WeaponAffinityMonster), ref nlFlag);
				}
				else
				{
					ProcessArtifact01(artifact, ac, ref nlFlag);
				}
			}
		}

		public virtual void ProcessArtifact01(IArtifact artifact, IArtifactCategory ac, ref bool nlFlag)
		{
			IsCarriedByContainer = artifact.IsCarriedByContainer();

			artifact.SetCarriedByMonster(ActorMonster);

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
		
		public virtual void ProcessAction(long actionType, Action action, ref bool nlFlag)
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

			IsMonsterEnabled = true;

			Name = "GetCommand";

			Verb = "get";

			Type = CommandType.Manipulation;
			
			ArtTypes = new ArtifactType[] { ArtifactType.DisguisedMonster, ArtifactType.DeadBody, ArtifactType.BoundMonster, ArtifactType.Weapon, ArtifactType.MagicWeapon };
		}
	}
}
