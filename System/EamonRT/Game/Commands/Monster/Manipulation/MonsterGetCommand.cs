
// MonsterGetCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
using System.Diagnostics;
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
	public class MonsterGetCommand : Command, IMonsterGetCommand
	{
		/// <summary></summary>
		public virtual ArtifactType[] ArtTypes { get; set; }

		/// <summary></summary>
		public virtual IList<IMonster> FumbleMonsterList { get; set; }

		/// <summary></summary>
		public virtual IArtifactCategory DobjArtAc { get; set; }

		/// <summary></summary>
		public virtual bool OmitWeightCheck { get; set; }

		public override void Execute()
		{
			Debug.Assert(DobjArtifact != null);

			ArtTypes = new ArtifactType[] { ArtifactType.DisguisedMonster, ArtifactType.DeadBody, ArtifactType.BoundMonster, ArtifactType.Weapon, ArtifactType.MagicWeapon };

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
