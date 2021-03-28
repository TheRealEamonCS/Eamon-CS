
// MonsterReadyCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class MonsterReadyCommand : Command, IMonsterReadyCommand
	{
		/// <summary></summary>
		public virtual IArtifact ActorWeapon { get; set; }

		/// <summary></summary>
		public virtual string MonsterName { get; set; }

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
						MonsterName = ActorMonster.EvalPlural(ActorMonster.GetTheName(true), ActorMonster.GetArticleName(true, true, false, true, Globals.Buf01));

						gOut.Print("{0} readies {1}.", MonsterName, DobjArtifact.GetArticleName());
					}
					else
					{
						MonsterName = string.Format("An unseen {0}", ActorMonster.CheckNBTLHostility() ? "offender" : "entity");

						gOut.Print("{0} readies {1}.", MonsterName, "a weapon");
					}

					if (ActorMonster.CheckNBTLHostility())
					{
						Globals.Thread.Sleep(gGameState.PauseCombatMs);
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

		public MonsterReadyCommand()
		{
			SortOrder = 820;

			IsPlayerEnabled = false;

			IsMonsterEnabled = true;

			Uid = 22;

			Name = "MonsterReadyCommand";

			Verb = "ready";

			Type = CommandType.Manipulation;
		}
	}
}
