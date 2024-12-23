﻿
// GetCommand.cs

// Copyright (c) 2014+ by Kenneth Pedersen.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static LandOfTheMountainKing.Game.Plugin.Globals;

namespace LandOfTheMountainKing.Game.Commands
{
	[ClassMappings]
	public class GetCommand : EamonRT.Game.Commands.GetCommand, IGetCommand
	{
		public override void ExecuteForPlayer()
		{
			if (GetAll)
			{
				var HandleArtifact = gADB[23];

				if (HandleArtifact.IsInRoom(ActorRoom))
				{
					gEngine.PrintEffectDesc(23);
				}
			}

			base.ExecuteForPlayer();
		}

		public override void ProcessArtifact(IArtifact artifact, IArtifactCategory ac, ref bool nlFlag)
		{
			// Get the handle

			if (artifact.Uid == 23)
			{
				var SwampMonster = gMDB[6]; // Create Swamp monster variable
				var SilverSword = gADB[25]; // Create SilverSword variable

				if (gLMKKP1.SwampMonsterKilled == 0 && SwampMonster.IsInLimbo())
				{
					var effect = gEDB[20];
					SwampMonster.SetInRoomUid(8);
					ProcessAction(100, () => gOut.Print("{0}", effect.Desc), ref nlFlag);
					if (NextState is IPutCommand putCommand && putCommand.DobjArtifact?.Uid == 23)
					{
						NextState = null;
					}
				}
				else if (SwampMonster.IsInRoomUid(8))
				{
					var effect = gEDB[21];
					ProcessAction(101, () => gOut.Print("{0}", effect.Desc), ref nlFlag);
					if (NextState is IPutCommand putCommand && putCommand.DobjArtifact?.Uid == 23)
					{
						NextState = null;
					}
				}
				else if (gLMKKP1.SwampMonsterKilled == 1)
				{
					var effect = gEDB[22];
					ProcessAction(102, () => gOut.Print("{0}", effect.Desc), ref nlFlag);
					if (ActorMonster.CanCarryArtifactWeight(SilverSword))
					{
						SilverSword.SetCarriedByMonster(ActorMonster);
					}
					else
					{
						SilverSword.SetInRoom(ActorRoom);
					}
					artifact.SetInLimbo();
					if (NextState is IPutCommand putCommand && putCommand.DobjArtifact?.Uid == 23)
					{
						NextState.PrintDontFollowYou();
						NextState = null;
					}
					else if (ActorMonster.Weapon <= 0 && SilverSword.IsCarriedByMonster(ActorMonster))
					{
						PrintFullDesc(SilverSword, false, false);
						SilverSword.Seen = true;
						WeaponArtifact = SilverSword;
					}
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
