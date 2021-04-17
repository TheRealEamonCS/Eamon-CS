
// RemoveCommand.cs

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
	public class RemoveCommand : Command, IRemoveCommand
	{
		public long _dobjArtifactCount;

		public long _dobjArtifactWeight;

		public long _actorMonsterInventoryWeight;

		/// <summary></summary>
		public virtual IArtifactCategory ArmorArtifactAc { get; set; }

		/// <summary></summary>
		public virtual IArtifactCategory ShieldArtifactAc { get; set; }

		/// <summary></summary>
		public virtual IArtifact ArmorArtifact { get; set; }

		/// <summary></summary>
		public virtual IArtifact ShieldArtifact { get; set; }

		public override void Execute()
		{
			Debug.Assert(DobjArtifact != null);

			if (IobjArtifact != null)
			{
				if (!GetCommandCalled)
				{
					RedirectToGetCommand<IRemoveCommand>(DobjArtifact, false);

					goto Cleanup;
				}

				if (!DobjArtifact.IsCarriedByCharacter())
				{
					if (DobjArtifact.DisguisedMonster == null)
					{
						NextState = Globals.CreateInstance<IStartState>();
					}

					goto Cleanup;
				}

				if (ActorMonster.Weapon <= 0 && DobjArtifact.IsReadyableByCharacter() && NextState == null)
				{
					NextState = Globals.CreateInstance<IReadyCommand>();

					CopyCommandData(NextState as ICommand, false);

					goto Cleanup;
				}

				goto Cleanup;
			}

			ArmorArtifact = gADB[gGameState.Ar];

			ShieldArtifact = gADB[gGameState.Sh];

			ArmorArtifactAc = ArmorArtifact != null ? ArmorArtifact.Wearable : null;

			ShieldArtifactAc = ShieldArtifact != null ? ShieldArtifact.Wearable : null;

			if (DobjArtifact.Uid == gGameState.Sh)
			{
				ActorMonster.Armor = ArmorArtifactAc != null ? (ArmorArtifactAc.Field1 / 2) + ((ArmorArtifactAc.Field1 / 2) >= 3 ? 2 : 0) : 0;

				gGameState.Sh = 0;
			}

			if (DobjArtifact.Uid == gGameState.Ar)
			{
				ActorMonster.Armor = ShieldArtifactAc != null ? ShieldArtifactAc.Field1 : 0;

				gGameState.Ar = 0;
			}

			DobjArtifact.SetCarriedByCharacter();

			PrintRemoved(DobjArtifact);

			ProcessEvents(EventType.AfterRemoveWornArtifact);

			if (GotoCleanup)
			{
				goto Cleanup;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		/*
		public override bool IsPrepEnabled(IPrep prep)
		{
			Debug.Assert(prep != null);

			PrepNames = new string[] { "in", "fromin", "on", "fromon", "under", "fromunder", "behind", "frombehind", "from" };

			return PrepNames.FirstOrDefault(pn => prep.Name.Equals(pn, StringComparison.OrdinalIgnoreCase)) != null;
		}
		*/

		public RemoveCommand()
		{
			SortOrder = 220;

			IsIobjEnabled = true;

			if (Globals.IsRulesetVersion(5))
			{
				IsPlayerEnabled = false;
			}

			Uid = 53;

			Name = "RemoveCommand";

			Verb = "remove";

			Type = CommandType.Manipulation;
		}
	}
}
