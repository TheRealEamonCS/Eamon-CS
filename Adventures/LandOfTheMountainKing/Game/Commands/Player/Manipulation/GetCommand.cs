
// GetCommand.cs

// Copyright (c) 2014+ by Kenneth Pedersen.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static LandOfTheMountainKing.Game.Plugin.PluginContext;

namespace LandOfTheMountainKing.Game.Commands
{
	[ClassMappings]
	public class GetCommand : EamonRT.Game.Commands.GetCommand, IGetCommand
	{
		public override void Execute()
		{
			if (GetAll)
			{
				var HandleArtifact = gADB[23];

				if (HandleArtifact.IsInRoom(ActorRoom))
				{
					gEngine.PrintEffectDesc(23);
				}
			}

			base.Execute();
		}

		public override void ProcessArtifact(IArtifact artifact, IArtifactCategory ac, ref bool nlFlag)
		{
			// Get the handle

			if (artifact.Uid == 23)
			{
				var SwampMonster = gMDB[6]; // create Swamp monster variable
				var SilverSword = gADB[25]; // create SilverSword variable

				if (gLMKKP1.SwampMonsterKilled == 0 && SwampMonster.IsInLimbo())
				{
					var effect = gEDB[20];
					SwampMonster.SetInRoomUid(8);
					ProcessAction(() => gOut.Print("{0}", effect.Desc), ref nlFlag);
				}
				else if (SwampMonster.IsInRoomUid(8))
				{
					var effect = gEDB[21];
					ProcessAction(() => gOut.Print("{0}", effect.Desc), ref nlFlag);
				}
				else if (gLMKKP1.SwampMonsterKilled == 1)
				{
					var effect = gEDB[22];
					ProcessAction(() => gOut.Print("{0}", effect.Desc), ref nlFlag);
					SilverSword.SetCarriedByCharacter();			// TODO: put silver sword in Room if its too heavy to be carried
					artifact.SetInLimbo();
				}
				else
				{
					base.ProcessArtifact(artifact, ac, ref nlFlag);
				}
			}
			else
			{
				base.ProcessArtifact(artifact, ac, ref nlFlag);
			}
		}
	}
}
