
// FreeCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.PluginContext;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class FreeCommand : Command, IFreeCommand
	{
		/// <summary></summary>
		public virtual IArtifactCategory DobjArtAc { get; set; }

		/// <summary></summary>
		public virtual IMonster BoundMonster { get; set; }

		/// <summary></summary>
		public virtual IMonster GuardMonster { get; set; }

		/// <summary></summary>
		public virtual IArtifact KeyArtifact { get; set; }

		/// <summary></summary>
		public virtual long BoundMonsterUid { get; set; }

		/// <summary></summary>
		public virtual long GuardMonsterUid { get; set; }

		/// <summary></summary>
		public virtual long KeyArtifactUid { get; set; }

		public override void Execute()
		{
			Debug.Assert(DobjArtifact != null || DobjMonster != null);

			if (DobjMonster != null)
			{
				PrintCantVerbObj(DobjMonster);

				goto Cleanup;
			}

			DobjArtAc = DobjArtifact.BoundMonster;

			if (DobjArtAc == null)
			{
				PrintCantVerbObj(DobjArtifact);

				NextState = Globals.CreateInstance<IStartState>();

				goto Cleanup;
			}

			BoundMonsterUid = DobjArtAc.GetMonsterUid();

			KeyArtifactUid = DobjArtAc.GetKeyUid();

			GuardMonsterUid = DobjArtAc.Field3;

			BoundMonster = BoundMonsterUid > 0 ? gMDB[BoundMonsterUid] : null;

			KeyArtifact = KeyArtifactUid > 0 ? gADB[KeyArtifactUid] : null;

			GuardMonster = GuardMonsterUid > 0 ? gMDB[GuardMonsterUid] : null;

			Debug.Assert(BoundMonster != null);

			ProcessEvents(EventType.BeforeGuardMonsterCheck);

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			if (GuardMonster != null && GuardMonster.IsInRoom(ActorRoom))
			{
				PrintWontLetYou(GuardMonster);

				goto Cleanup;
			}

			if (KeyArtifactUid == -1)
			{
				PrintNoObviousWayToDoThat();

				goto Cleanup;
			}

			if (KeyArtifact != null && !KeyArtifact.IsCarriedByCharacter() && !KeyArtifact.IsWornByCharacter() && !KeyArtifact.IsInRoom(ActorRoom))
			{
				PrintDontHaveTheKey();

				goto Cleanup;
			}

			PrintFreeActorWithKey(BoundMonster, KeyArtifact);

			BoundMonster.SetInRoom(ActorRoom);

			DobjArtifact.SetInLimbo();

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		public FreeCommand()
		{
			Synonyms = new string[] { "release" };

			SortOrder = 270;

			if (Globals.IsRulesetVersion(5))
			{
				IsPlayerEnabled = false;
			}

			Uid = 36;

			Name = "FreeCommand";

			Verb = "free";

			Type = CommandType.Interactive;
		}
	}
}
