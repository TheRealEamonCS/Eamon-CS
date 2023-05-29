
// EatCommand.cs

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
	public class EatCommand : Command, IEatCommand
	{
		/// <summary></summary>
		public virtual ArtifactType[] ArtTypes { get; set; }

		/// <summary></summary>
		public virtual IArtifactCategory DobjArtAc { get; set; }

		public override void ExecuteForPlayer()
		{
			Debug.Assert(DobjArtifact != null);

			ProcessEvents(EventType.BeforeEatArtifact);

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			DobjArtAc = DobjArtifact.GetArtifactCategory(ArtTypes, false);

			if (DobjArtAc == null)
			{
				PrintCantVerbObj(DobjArtifact);

				NextState = gEngine.CreateInstance<IStartState>();

				goto Cleanup;
			}

			if (DobjArtAc.Type == ArtifactType.DisguisedMonster)
			{
				gEngine.RevealDisguisedMonster(ActorRoom, DobjArtifact);

				goto Cleanup;
			}

			if (DobjArtAc.Type == ArtifactType.Drinkable)
			{
				NextState = gEngine.CreateInstance<IDrinkCommand>();

				CopyCommandData(NextState as ICommand);

				goto Cleanup;
			}

			if (!DobjArtAc.IsOpen())
			{
				PrintMustFirstOpen(DobjArtifact);

				NextState = gEngine.CreateInstance<IStartState>();

				goto Cleanup;
			}

			if (DobjArtAc.Field2 < 1)
			{
				PrintNoneLeft(DobjArtifact);

				goto Cleanup;
			}

			if (DobjArtAc.Field2 != gEngine.InfiniteDrinkableEdible)
			{
				DobjArtAc.Field2--;
			}

			ProcessEvents(EventType.BeforeNowEmptyArtifactCheck);

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			if (DobjArtAc.Field2 < 1)
			{
				DobjArtifact.Value = 0;

				DobjArtifact.SetInLimbo();

				PrintVerbItAll(DobjArtifact);
			}
			else if (DobjArtAc.Field1 == 0)
			{
				PrintOkay(DobjArtifact);
			}

			if (DobjArtAc.Field1 != 0)
			{
				ActorMonster.DmgTaken -= DobjArtAc.Field1;

				if (ActorMonster.DmgTaken < 0)
				{
					ActorMonster.DmgTaken = 0;
				}

				if (DobjArtAc.Field1 > 0)
				{
					PrintFeelBetter(DobjArtifact);
				}
				else
				{
					PrintFeelWorse(DobjArtifact);
				}

				PrintHealthStatus(ActorMonster, false);

				if (ActorMonster.IsDead())
				{
					gGameState.Die = 1;

					NextState = gEngine.CreateInstance<IPlayerDeadState>(x =>
					{
						x.PrintLineSep = true;
					});

					goto Cleanup;
				}
			}

			ProcessEvents(EventType.AfterEatArtifact);

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

		public EatCommand()
		{
			SortOrder = 140;

			if (gEngine.IsRulesetVersion(5, 62))
			{
				IsPlayerEnabled = false;
			}

			Name = "EatCommand";

			Verb = "eat";

			Type = CommandType.Manipulation;

			ArtTypes = new ArtifactType[] { ArtifactType.DisguisedMonster, ArtifactType.Edible, ArtifactType.Drinkable };
		}
	}
}
