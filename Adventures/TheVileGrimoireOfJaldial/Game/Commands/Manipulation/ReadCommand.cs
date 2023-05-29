
// ReadCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static TheVileGrimoireOfJaldial.Game.Plugin.Globals;

namespace TheVileGrimoireOfJaldial.Game.Commands
{
	[ClassMappings]
	public class ReadCommand : EamonRT.Game.Commands.ReadCommand, IReadCommand
	{
		public override void ExecuteForPlayer()
		{
			// Solitary tombstone

			if (DobjArtifact?.Uid == 10)
			{
				gEngine.PrintEffectDesc(83);

				gOut.WriteLine();

				gEngine.PrintTitle("Here under this stone lies wise sage Druce,".PadTRight(44, ' '), false);
				gEngine.PrintTitle("Who in his time spoke many truths.".PadTRight(44, ' '), false);
				gEngine.PrintTitle("Just how he died none do now know,".PadTRight(44, ' '), false);
				gEngine.PrintTitle("It's thought he was killed by an unseen foe.".PadTRight(44, ' '), false);

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}

			// Decoration

			else if (DobjArtifact?.Uid == 41 && DobjArtifact.Field2 > 0)
			{
				switch (DobjArtifact.Field2)
				{
					case 1:
					{
						var firstNames = new string[] { "Joala", "Skulian", "Kalsil", "Torlal", "Quald", "Olsaian", "Lianda", "Slobalan", "Yeouil", "Geoamlo" };

						var middleNames = new string[] { "Iliom", "Uilalma", "Polais", "Eliuo", "Malamdi", "Didosal", "Alalkdi", "Jaldian", "Zloana", "Voquilana" };

						var lastNames = new string[] { "Aluionsa", "Opuloanaks", "Yoalamas", "Telaoam", "Dalsodid", "Ralpodkin", "Makdidtoala", "Salaidodona", "Hvolala", "Codlaeido" };

						var epitaphs = new string[]
						{
							"live better in the afterlife than here", "rest in pieces", "aspire to godsent ethics", "rest in peace", "fulfill the prescribed destiny which awaits",
							"die a thousand times more before finally resting", "listen next time with more interest", "show better timing with the next departure", "live forever in our memories",
							"be treated above as life was lived down here"
						};

						gEngine.PrintEffectDesc(84);

						gOut.WriteLine();

						gEngine.PrintTitle("HERE LIES", false);

						var fullName = string.Format("{0}  {1}  {2}", firstNames[gEngine.RollDice(1, 10, -1)], middleNames[gEngine.RollDice(1, 10, -1)], lastNames[gEngine.RollDice(1, 10, -1)]);

						gOut.WriteLine();

						gEngine.PrintTitle(fullName, false);

						var birthDate = gEngine.RollDice(1, 501, 999);

						var deathDate = birthDate + gEngine.RollDice(1, 100, 0);

						var dateRange = string.Format("{0}-{1}", birthDate, deathDate);

						gOut.WriteLine();

						gEngine.PrintTitle(dateRange, false);

						var gender = gEngine.RollDice(1, 100, 0) > 50 ? Gender.Male : Gender.Female;

						var epitaph = string.Format("May {0} {1}.", gEngine.EvalGender(gender, "he", "she", "it"), epitaphs[gEngine.RollDice(1, 10, -1)]);

						gOut.WriteLine();

						gEngine.PrintTitle(epitaph, false);

						break;
					}

					case 2:

						gEngine.PrintEffectDesc(85);

						gOut.WriteLine();

						gEngine.PrintTitle("Here I died many seasons ago.".PadTRight(35, ' '), false);
						gEngine.PrintTitle("My brittle bones beneath your toes.".PadTRight(35, ' '), false);
						gEngine.PrintTitle("What time I had I spent it well.".PadTRight(35, ' '), false);
						gEngine.PrintTitle("Or so I thought - I'm now in...".PadTRight(35, ' '), false);

						break;

					case 3:
					case 4:
					case 5:

						var command = gEngine.CreateInstance<IExamineCommand>();

						CopyCommandData(command);

						NextState = command;

						break;

					case 6:

						gEngine.PrintEffectDesc(86);

						break;

					case 7:

						gEngine.PrintEffectDesc(87);

						break;

					case 8:

						gEngine.PrintEffectDesc(88);

						gEngine.PrintEffectDesc(89);

						break;
				}

				if (NextState == null)
				{
					NextState = gEngine.CreateInstance<IMonsterStartState>();
				}
			}
			else
			{
				base.ExecuteForPlayer();
			}
		}
	}
}
