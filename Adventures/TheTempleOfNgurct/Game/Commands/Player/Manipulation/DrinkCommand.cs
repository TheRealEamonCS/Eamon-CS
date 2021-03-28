
// DrinkCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static TheTempleOfNgurct.Game.Plugin.PluginContext;

namespace TheTempleOfNgurct.Game.Commands
{
	[ClassMappings]
	public class DrinkCommand : EamonRT.Game.Commands.DrinkCommand, IDrinkCommand
	{
		public virtual long DmgTaken { get; set; }

		public virtual bool DrankItAll { get; set; }

		public override void PrintVerbItAll(IArtifact artifact)
		{
			DrankItAll = true;
		}

		public override void PrintFeelBetter(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			if (DmgTaken > 0)
			{
				gOut.Print("Some of your wounds seem to clear up.");
			}
		}

		public override void PrintCantVerbHere()
		{
			PrintEnemiesNearby();
		}

		public override void Execute()
		{
			Debug.Assert(DobjArtifact != null);

			DmgTaken = ActorMonster.DmgTaken;

			var ac = DobjArtifact.Drinkable;

			// Sulphuric acid

			if (DobjArtifact.Uid == 53 && ac.IsOpen())
			{
				gEngine.PrintEffectDesc(29);

				gGameState.Die = 1;

				NextState = Globals.CreateInstance<IPlayerDeadState>(x =>
				{
					x.PrintLineSep = true;
				});
			}

			// Human blood

			else if (DobjArtifact.Uid == 52 && ac.IsOpen())
			{
				gEngine.PrintEffectDesc(30);

				DrankItAll = true;

				NextState = Globals.CreateInstance<IMonsterStartState>();
			}

			// Wine

			else if (DobjArtifact.Uid == 69 && ac.IsOpen())
			{
				var stat = gEngine.GetStats(Stat.Agility);

				Debug.Assert(stat != null);

				gEngine.PrintEffectDesc(31);

				ActorMonster.Agility *= 2;

				ActorMonster.Agility = (long)Math.Round((double)ActorMonster.Agility / 3);

				if (ActorMonster.Agility < stat.MinValue)
				{
					ActorMonster.Agility = stat.MinValue;
				}

				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
			else
			{
				base.Execute();
			}

			if (DrankItAll)
			{
				DobjArtifact.Value = 0;

				DobjArtifact.SetInLimbo();
			}
		}

		public override bool IsAllowedInRoom()
		{
			return gGameState.GetNBTL(Friendliness.Enemy) <= 0;
		}

		public DrinkCommand()
		{
			IsPlayerEnabled = true;
		}
	}
}
