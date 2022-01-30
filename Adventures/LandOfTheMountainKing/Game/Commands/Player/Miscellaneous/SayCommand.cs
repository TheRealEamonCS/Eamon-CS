
// SayCommand.cs

// Copyright (c) 2014+ by Kenneth Pedersen.  All rights reserved.

using System;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Extensions;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static LandOfTheMountainKing.Game.Plugin.PluginContext;

namespace LandOfTheMountainKing.Game.Commands
{
	[ClassMappings]
	public class SayCommand : EamonRT.Game.Commands.SayCommand, ISayCommand
	{
		public override void ProcessEvents(EventType eventType)
		{
			base.ProcessEvents(eventType);

			if (eventType == EventType.BeforePrintSayText)
			{
				var Lisa = gMDB[3];

				// Assume custom text output, skip default behavior

				GotoCleanup = true;

				if (gLMKKP1.SaidHello == 1 && gLMKKP1.NecklaceTaken < 2 && ActorRoom.Uid == 13 && Lisa.Reaction == Friendliness.Neutral && ProcessedPhrase.Equals("hi", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(61);
				}
				else if (gLMKKP1.SaidHello == 1 && gLMKKP1.NecklaceTaken < 2 && ActorRoom.Uid == 13 && Lisa.Reaction == Friendliness.Neutral && ProcessedPhrase.Equals("hello", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(61);
				}
				else if (gLMKKP1.SaidHello == 0 && gLMKKP1.NecklaceTaken < 2 && ActorRoom.Uid == 13 && Lisa.Reaction == Friendliness.Neutral && ProcessedPhrase.Equals("hello", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(16);
					gEngine.PrintEffectDesc(17);
					gEngine.PrintEffectDesc(18);
					gLMKKP1.SaidHello = 1;
				}
				else if (gLMKKP1.SaidHello == 0 && gLMKKP1.NecklaceTaken < 2 && ActorRoom.Uid == 13 && Lisa.Reaction == Friendliness.Neutral && ProcessedPhrase.Equals("hi", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(19);
					gEngine.PrintEffectDesc(17);
					gEngine.PrintEffectDesc(18);
					gLMKKP1.SaidHello = 1;
				}
				else if (gLMKKP1.NecklaceTaken == 2 && ActorRoom.Uid == 13 && Lisa.Reaction == Friendliness.Neutral && ProcessedPhrase.Equals("hi", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(62);
				}
				else if (gLMKKP1.NecklaceTaken == 2 && ActorRoom.Uid == 13 && Lisa.Reaction == Friendliness.Neutral && ProcessedPhrase.Equals("hello", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(62);
				}
				else if (gLMKKP1.NecklaceTaken < 2 && ActorRoom.Uid == 13 && Lisa.Reaction == Friendliness.Neutral && ProcessedPhrase.Equals("damian", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(63);
				}
				else if (gLMKKP1.NecklaceTaken == 2 && ActorRoom.Uid == 13 && Lisa.Reaction == Friendliness.Neutral && ProcessedPhrase.Equals("damian", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(64);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Reaction == Friendliness.Neutral && ProcessedPhrase.Equals("necklace", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(65);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Reaction == Friendliness.Neutral && ProcessedPhrase.Equals("bats", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(66);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Reaction == Friendliness.Neutral && ProcessedPhrase.Equals("bat", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(66);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Reaction == Friendliness.Neutral && ProcessedPhrase.Equals("cave", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(67);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Reaction == Friendliness.Neutral && ProcessedPhrase.Equals("lighthouse", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(68);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Reaction == Friendliness.Neutral && ProcessedPhrase.Equals("opening", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(69);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Reaction == Friendliness.Neutral && ProcessedPhrase.Equals("window", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(69);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Reaction == Friendliness.Neutral && ProcessedPhrase.Equals("squid", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(70);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Reaction == Friendliness.Neutral && ProcessedPhrase.Equals("squids", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(70);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Reaction == Friendliness.Neutral && ProcessedPhrase.Equals("warrior", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(71);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Reaction == Friendliness.Neutral && ProcessedPhrase.Equals("tree", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(72);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Reaction == Friendliness.Neutral && ProcessedPhrase.Equals("oak", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(72);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Reaction == Friendliness.Neutral && ProcessedPhrase.Equals("oak tree", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(72);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Reaction == Friendliness.Neutral && ProcessedPhrase.Equals("large tree", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(72);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Reaction == Friendliness.Neutral && ProcessedPhrase.Equals("large oak", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(72);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Reaction == Friendliness.Neutral && ProcessedPhrase.Equals("lisa", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(73);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Reaction == Friendliness.Neutral && ProcessedPhrase.Equals("swamp", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(74);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Reaction == Friendliness.Neutral && ProcessedPhrase.Equals("monster", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(75);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Reaction == Friendliness.Neutral && ProcessedPhrase.Equals("monsters", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(75);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Reaction == Friendliness.Neutral && ProcessedPhrase.Equals("werewolves", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(76);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Reaction == Friendliness.Neutral && ProcessedPhrase.Equals("werewolf", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(76);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Reaction == Friendliness.Neutral && ProcessedPhrase.Equals("ogres", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(77);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Reaction == Friendliness.Neutral && ProcessedPhrase.Equals("ogre", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(77);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Reaction == Friendliness.Neutral && ProcessedPhrase.Equals("king", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(78);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Reaction == Friendliness.Neutral && ProcessedPhrase.Equals("mountain king", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(78);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Reaction == Friendliness.Neutral && ProcessedPhrase.Equals("creatures", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(79);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Reaction == Friendliness.Neutral && ProcessedPhrase.Equals("enemies", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(79);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Reaction == Friendliness.Neutral && ProcessedPhrase.Equals("servants", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(79);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Reaction == Friendliness.Neutral && ProcessedPhrase.Equals("land", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(81);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Reaction == Friendliness.Neutral && ProcessedPhrase.Equals("reward", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(82);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Reaction == Friendliness.Neutral && ProcessedPhrase.Equals("mountains", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(83);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Reaction == Friendliness.Neutral && ProcessedPhrase.Equals("forest", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(84);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Reaction == Friendliness.Neutral && ProcessedPhrase.Equals("hair", StringComparison.OrdinalIgnoreCase))
				{
					gEngine.PrintEffectDesc(85);
				}
				else if (ActorRoom.Uid == 13 && Lisa.Reaction == Friendliness.Neutral)
				{
					gEngine.PrintEffectDesc(80);
				}
				else
				{
					// No custom text output, use default behavior

					GotoCleanup = false;
				}
			}
		}
	}
}
