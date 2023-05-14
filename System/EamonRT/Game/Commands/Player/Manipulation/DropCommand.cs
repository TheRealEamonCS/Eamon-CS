
// DropCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Collections.Generic;
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
	public class DropCommand : Command, IDropCommand
	{
		public virtual bool DropAll { get; set; }

		/// <summary></summary>
		public virtual IList<IArtifact> DroppedArtifactList { get; set; }

		/// <summary></summary>
		public virtual IArtifact LsArtifact { get; set; }

		public override void Execute()
		{
			Debug.Assert(DropAll || DobjArtifact != null);

			if (DobjArtifact != null)
			{
				if (DobjArtifact.IsWornByMonster(ActorMonster))
				{
					PrintWearingRemoveFirst(DobjArtifact);

					NextState = gEngine.CreateInstance<IStartState>();

					goto Cleanup;
				}

				if (!DobjArtifact.IsCarriedByMonster(ActorMonster))
				{
					if (!GetCommandCalled)
					{
						if (DobjArtifact.IsCarriedByMonster(ActorMonster, true) && DobjArtifact.IsCarriedByContainer())
						{
							RedirectToGetCommand<IDropCommand>(DobjArtifact);
						}
						else
						{
							NextState = gEngine.CreateInstance<IStartState>();
						}
					}
					else if (DobjArtifact.DisguisedMonster == null)
					{
						NextState = gEngine.CreateInstance<IStartState>();
					}

					goto Cleanup;
				}
			}

			ProcessLightSource();

			DroppedArtifactList = DropAll ? ActorMonster.GetCarriedList() : new List<IArtifact>() { DobjArtifact };

			if (DroppedArtifactList.Count <= 0)
			{
				PrintNothingToDrop();

				NextState = gEngine.CreateInstance<IStartState>();

				goto Cleanup;
			}

			foreach (var artifact in DroppedArtifactList)
			{
				ProcessArtifact(artifact);
			}

			gOut.WriteLine();

		Cleanup:

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
		}

		public virtual void ProcessArtifact(IArtifact artifact)
		{
			RetCode rc;

			Debug.Assert(artifact != null);

			artifact.SetInRoom(ActorRoom);

			if (ActorMonster.Weapon == artifact.Uid)
			{
				Debug.Assert(artifact.GeneralWeapon != null);

				rc = artifact.RemoveStateDesc(artifact.GetReadyWeaponDesc());

				Debug.Assert(gEngine.IsSuccess(rc));

				ActorMonster.Weapon = -1;
			}

			PrintDropped(artifact);
		}

		public virtual void ProcessLightSource()
		{
			if (gGameState.Ls > 0)
			{
				LsArtifact = gADB[gGameState.Ls];

				Debug.Assert(LsArtifact != null && LsArtifact.LightSource != null);

				if ((DropAll || LsArtifact == DobjArtifact) && LsArtifact.IsCarriedByMonster(ActorMonster))
				{
					gEngine.LightOut(LsArtifact);
				}
			}
		}

		public DropCommand()
		{
			SortOrder = 130;

			Name = "DropCommand";

			Verb = "drop";

			Type = CommandType.Manipulation;
		}
	}
}
