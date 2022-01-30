
// GiveCommand.cs

// Copyright (c) 2014+ by Kenneth Pedersen.  All rights reserved.

using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static LandOfTheMountainKing.Game.Plugin.PluginContext;

namespace LandOfTheMountainKing.Game.Commands
{
	[ClassMappings]
	public class GiveCommand : EamonRT.Game.Commands.GiveCommand, IGiveCommand
	{		
		public override void Execute()
		{
			// Can't give away gold

			if (GoldAmount > 0)
			{
				gEngine.PrintEffectDesc(60);
				NextState = Globals.CreateInstance<IStartState>();
			}

			// Give necklace to Lisa

			else if (DobjArtifact?.Uid == 27 && IobjMonster?.Uid == 3 && IobjMonster.Reaction == Friendliness.Neutral)
			{
				gEngine.PrintEffectDesc(31);
				gEngine.PrintEffectDesc(32);
				gEngine.PrintEffectDesc(33);
				gEngine.PrintEffectDesc(34);
				gEngine.PrintEffectDesc(35);
				gEngine.PrintEffectDesc(36);
				gLMKKP1.NecklaceTaken = 2;
				DobjArtifact.SetCarriedByMonster(IobjMonster);
				NextState = Globals.CreateInstance<IStartState>();

				//Set damage taken to zero:
				ActorMonster.DmgTaken = 0;
				ActorMonster.Hardiness = 55;
			}
			else
			{
				base.Execute();
			}
		}
	}
}
