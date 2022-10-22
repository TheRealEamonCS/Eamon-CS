
// MonsterReadyCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.Globals;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class MonsterReadyCommand : Command, IMonsterReadyCommand
	{
		/// <summary></summary>
		public virtual IArtifact ActorWeapon { get; set; }

		public override void Execute()
		{
			RetCode rc;

			Debug.Assert(DobjArtifact != null);

			if (DobjArtifact.IsReadyableByMonster(ActorMonster) && DobjArtifact.IsCarriedByMonster(ActorMonster))
			{
				ActorWeapon = gADB[ActorMonster.Weapon];

				if (ActorWeapon != null)
				{
					rc = ActorWeapon.RemoveStateDesc(ActorWeapon.GetReadyWeaponDesc());

					Debug.Assert(gEngine.IsSuccess(rc));
				}

				ActorMonster.Weapon = DobjArtifact.Uid;

				rc = DobjArtifact.AddStateDesc(DobjArtifact.GetReadyWeaponDesc());

				Debug.Assert(gEngine.IsSuccess(rc));

				Debug.Assert(gCharMonster != null);

				if (gCharMonster.IsInRoom(ActorRoom))
				{
					if (ActorRoom.IsLit())
					{
						PrintActorReadiesObj(ActorMonster, DobjArtifact);
					}
					else
					{
						PrintActorReadiesWeapon(ActorMonster);
					}

					if (ActorMonster.CheckNBTLHostility())
					{
						gEngine.Thread.Sleep(gGameState.PauseCombatMs);
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

		public MonsterReadyCommand()
		{
			SortOrder = 820;

			IsPlayerEnabled = false;

			IsMonsterEnabled = true;

			Name = "MonsterReadyCommand";

			Verb = "ready";

			Type = CommandType.Manipulation;
		}
	}
}
