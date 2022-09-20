
// MonsterGetCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using System.Diagnostics;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class MonsterGetCommand : Command, IMonsterGetCommand
	{
		public long _dobjArtifactCount;

		public long _dobjArtifactWeight;

		public long _actorMonsterInventoryWeight;

		/// <summary></summary>
		public virtual ArtifactType[] ArtTypes { get; set; }

		/// <summary></summary>
		public virtual IList<IMonster> FumbleMonsterList { get; set; }

		/// <summary></summary>
		public virtual IArtifactCategory DobjArtAc { get; set; }

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

		public override void Execute()
		{
			RetCode rc;

			Debug.Assert(DobjArtifact != null);

			ArtTypes = new ArtifactType[] { ArtifactType.DisguisedMonster, ArtifactType.DeadBody, ArtifactType.BoundMonster, ArtifactType.Weapon, ArtifactType.MagicWeapon };

			DobjArtAc = DobjArtifact.GetArtifactCategory(ArtTypes, false);

			if (DobjArtAc == null)
			{
				DobjArtAc = DobjArtifact.GetCategories(0);
			}

			if (DobjArtAc != null && DobjArtAc.Type != ArtifactType.DisguisedMonster && DobjArtifact.Weight <= 900 && !DobjArtifact.IsUnmovable01() && (DobjArtAc.Type != ArtifactType.DeadBody || DobjArtAc.Field1 == 1) && DobjArtAc.Type != ArtifactType.BoundMonster)
			{
				OmitWeightCheck = DobjArtifact.IsCarriedByMonster(ActorMonster, true);

				DobjArtifactCount = 0;

				DobjArtifactWeight = DobjArtifact.Weight;

				if (DobjArtifact.GeneralContainer != null)
				{
					rc = DobjArtifact.GetContainerInfo(ref _dobjArtifactCount, ref _dobjArtifactWeight, ContainerType.In, true);

					Debug.Assert(gEngine.IsSuccess(rc));

					rc = DobjArtifact.GetContainerInfo(ref _dobjArtifactCount, ref _dobjArtifactWeight, ContainerType.On, true);

					Debug.Assert(gEngine.IsSuccess(rc));
				}

				ActorMonsterInventoryWeight = 0;

				rc = ActorMonster.GetFullInventoryWeight(ref _actorMonsterInventoryWeight, recurse: true);

				Debug.Assert(gEngine.IsSuccess(rc));

				if (!gEngine.EnforceMonsterWeightLimits || OmitWeightCheck || (DobjArtifactWeight <= ActorMonster.GetWeightCarryableGronds() && DobjArtifactWeight + ActorMonsterInventoryWeight <= ActorMonster.GetWeightCarryableGronds() * ActorMonster.CurrGroupCount))
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
				NextState = Globals.CreateInstance<IErrorState>(x =>
				{
					x.ErrorMessage = string.Format("{0}: NextState == null", Name);
				});
			}
		}

		public MonsterGetCommand()
		{
			SortOrder = 810;

			IsPlayerEnabled = false;

			IsMonsterEnabled = true;

			Name = "MonsterGetCommand";

			Verb = "get";

			Type = CommandType.Manipulation;
		}
	}
}
