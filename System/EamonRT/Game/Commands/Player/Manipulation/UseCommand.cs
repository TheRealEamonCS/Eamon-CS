
// UseCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
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
	public class UseCommand : Command, IUseCommand
	{
		public virtual ArtifactType[] ArtTypes { get; set; }

		/// <summary></summary>
		public virtual IArtifactCategory DobjArtAc { get; set; }

		public override void Execute()
		{
			Debug.Assert(DobjArtifact != null);

			ProcessEvents(EventType.BeforeUseArtifact);

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			DobjArtAc = DobjArtifact.GetArtifactCategory(ArtTypes, false);

			if (DobjArtAc == null)
			{
				PrintTryDifferentCommand(DobjArtifact);

				goto Cleanup;
			}

			if (DobjArtAc.Type == ArtifactType.DisguisedMonster)
			{
				if (!DobjArtifact.IsUnmovable() && !DobjArtifact.IsCarriedByCharacter())
				{
					if (DobjArtifact.IsCarriedByContainer())
					{
						PrintRemovingFirst(DobjArtifact);
					}
					else
					{
						PrintTakingFirst(DobjArtifact);
					}
				}

				gEngine.RevealDisguisedMonster(ActorRoom, DobjArtifact);

				NextState = gEngine.CreateInstance<IMonsterStartState>();

				goto Cleanup;
			}

			if (DobjArtAc.IsWeapon01())
			{
				NextState = gEngine.CreateInstance<IReadyCommand>();

				CopyCommandData(NextState as ICommand);

				goto Cleanup;
			}

			if (DobjArtAc.Type == ArtifactType.Drinkable)
			{
				NextState = gEngine.CreateInstance<IDrinkCommand>();

				CopyCommandData(NextState as ICommand);

				goto Cleanup;
			}

			if (DobjArtAc.Type == ArtifactType.Edible)
			{
				NextState = gEngine.CreateInstance<IEatCommand>();

				CopyCommandData(NextState as ICommand);

				goto Cleanup;
			}

			Debug.Assert(DobjArtAc.Type == ArtifactType.Wearable);

			NextState = gEngine.CreateInstance<IWearCommand>();

			CopyCommandData(NextState as ICommand);

		Cleanup:

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
		}

		/*
		public override bool IsPrepEnabled(IPrep prep)
		{
			Debug.Assert(prep != null);

			PrepNames = new string[] { TODO };

			return PrepNames.FirstOrDefault(pn => prep.Name.Equals(pn, StringComparison.OrdinalIgnoreCase)) != null;
		}
		*/

		public UseCommand()
		{
			SortOrder = 230;

			if (gEngine.IsRulesetVersion(5, 62))
			{
				IsPlayerEnabled = false;
			}

			Name = "UseCommand";

			Verb = "use";

			Type = CommandType.Manipulation;

			ArtTypes = new ArtifactType[] { ArtifactType.DisguisedMonster, ArtifactType.Weapon, ArtifactType.MagicWeapon, ArtifactType.Drinkable, ArtifactType.Edible, ArtifactType.Wearable };
		}
	}
}
