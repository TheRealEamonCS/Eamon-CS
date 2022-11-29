
// ReadyCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon;
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
	public class ReadyCommand : Command, IReadyCommand
	{
		public virtual ArtifactType[] ArtTypes { get; set; }

		public virtual bool OmitReadySameWeapon { get; set; }

		/// <summary></summary>
		public virtual IArtifactCategory DobjArtAc { get; set; }

		/// <summary></summary>
		public virtual IArtifact ShieldArtifact { get; set; }

		/// <summary></summary>
		public virtual IArtifact ActorWeapon { get; set; }

		public override void Execute()
		{
			RetCode rc;

			Debug.Assert(DobjArtifact != null);

			DobjArtAc = DobjArtifact.GetArtifactCategory(ArtTypes, false);

			if (DobjArtAc == null)
			{
				PrintNotWeapon(DobjArtifact);

				NextState = gEngine.CreateInstance<IStartState>();

				goto Cleanup;
			}

			if (DobjArtAc.Type == ArtifactType.Wearable)
			{
				NextState = gEngine.CreateInstance<IWearCommand>();

				CopyCommandData(NextState as ICommand);

				goto Cleanup;
			}

			if (!DobjArtifact.IsReadyableByCharacter())
			{
				PrintNotReadyableWeapon(DobjArtifact);

				NextState = gEngine.CreateInstance<IStartState>();

				goto Cleanup;
			}

			if (!DobjArtifact.IsCarriedByCharacter())
			{
				if (!GetCommandCalled)
				{
					RedirectToGetCommand<IReadyCommand>(DobjArtifact);
				}
				else if (DobjArtifact.DisguisedMonster == null)
				{
					NextState = gEngine.CreateInstance<IStartState>();
				}

				goto Cleanup;
			}

			// can't use two-handed weapon while wearing shield

			if (gGameState.Sh > 0 && DobjArtAc.Field5 > 1)
			{
				ShieldArtifact = gADB[gGameState.Sh];

				Debug.Assert(ShieldArtifact != null);

				PrintCantReadyWeaponWithShield(DobjArtifact, ShieldArtifact);

				NextState = gEngine.CreateInstance<IStartState>();

				goto Cleanup;
			}

			ProcessEvents(EventType.BeforeReadyArtifact);

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			if (!OmitReadySameWeapon || ActorMonster.Weapon != DobjArtifact.Uid)
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
			}

			PrintReadied(DobjArtifact);

			ProcessEvents(EventType.AfterReadyArtifact);

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

		public ReadyCommand()
		{
			SortOrder = 210;

			Name = "ReadyCommand";

			Verb = "ready";

			Type = CommandType.Manipulation;

			ArtTypes = gEngine.IsRulesetVersion(5) ?
				new ArtifactType[] { ArtifactType.Weapon, ArtifactType.MagicWeapon } :
				new ArtifactType[] { ArtifactType.Weapon, ArtifactType.MagicWeapon, ArtifactType.Wearable };
		}
	}
}
