
// EatCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
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
	public class EatCommand : Command, IEatCommand
	{
		/// <summary></summary>
		public virtual IArtifactCategory DrinkableAc { get; set; }

		/// <summary></summary>
		public virtual IArtifactCategory EdibleAc { get; set; }

		/// <summary></summary>
		public virtual IArtifactCategory DobjArtAc { get; set; }

		public override void Execute()
		{
			Debug.Assert(DobjArtifact != null);

			DrinkableAc = DobjArtifact.Drinkable;

			EdibleAc = DobjArtifact.Edible;

			DobjArtAc = EdibleAc != null ? EdibleAc : DrinkableAc;

			if (DobjArtAc == null)
			{
				PrintCantVerbObj(DobjArtifact);

				NextState = Globals.CreateInstance<IStartState>();

				goto Cleanup;
			}

			if (DobjArtAc.Type == ArtifactType.Drinkable)
			{
				NextState = Globals.CreateInstance<IDrinkCommand>();

				CopyCommandData(NextState as ICommand);

				goto Cleanup;
			}

			if (!DobjArtAc.IsOpen())
			{
				PrintMustFirstOpen(DobjArtifact);

				NextState = Globals.CreateInstance<IStartState>();

				goto Cleanup;
			}

			if (DobjArtAc.Field2 < 1)
			{
				PrintNoneLeft(DobjArtifact);

				goto Cleanup;
			}

			if (DobjArtAc.Field2 != Constants.InfiniteDrinkableEdible)
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

					NextState = Globals.CreateInstance<IPlayerDeadState>(x =>
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
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		public EatCommand()
		{
			SortOrder = 140;

			if (Globals.IsRulesetVersion(5))
			{
				IsPlayerEnabled = false;
			}

			Uid = 45;

			Name = "EatCommand";

			Verb = "eat";

			Type = CommandType.Manipulation;
		}
	}
}
