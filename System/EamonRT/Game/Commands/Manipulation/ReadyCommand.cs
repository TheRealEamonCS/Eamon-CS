
// ReadyCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using System.Linq;
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

		public override void ExecuteForPlayer()
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

			if (!DobjArtifact.IsReadyableByMonster(ActorMonster))
			{
				PrintNotReadyableWeapon(DobjArtifact);

				NextState = gEngine.CreateInstance<IStartState>();

				goto Cleanup;
			}

			if (!DobjArtifact.IsCarriedByMonster(ActorMonster))
			{
				if (!ShouldAllowRedirectToGetCommand())
				{
					PrintDontHaveIt02(DobjArtifact);

					NextState = gEngine.CreateInstance<IStartState>();
				}
				else if (!GetCommandCalled)
				{
					RedirectToGetCommand<IReadyCommand>(DobjArtifact);
				}
				else if (DobjArtifact.DisguisedMonster == null)
				{
					NextState = gEngine.CreateInstance<IStartState>();
				}

				goto Cleanup;
			}

			// Can't use two-handed weapon while wearing shield

			if (gGameState.Sh > 0 && DobjArtAc.Field5 > 1)
			{
				ShieldArtifact = gADB[gGameState.Sh];

				Debug.Assert(ShieldArtifact != null);

				PrintCantReadyWeaponWithShield(DobjArtifact, ShieldArtifact);

				if (gEngine.LastCommandList.FirstOrDefault(c => c is IGetCommand) == null)
				{
					NextState = gEngine.CreateInstance<IStartState>();
				}

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

		public override void ExecuteForMonster()
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
						gEngine.PauseCombat();
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

		public ReadyCommand()
		{
			SortOrder = 210;

			IsMonsterEnabled = true;

			Name = "ReadyCommand";

			Verb = "ready";

			Type = CommandType.Manipulation;

			ArtTypes = gEngine.IsRulesetVersion(5, 62) ?
				new ArtifactType[] { ArtifactType.Weapon, ArtifactType.MagicWeapon } :
				new ArtifactType[] { ArtifactType.Weapon, ArtifactType.MagicWeapon, ArtifactType.Wearable };
		}
	}
}
