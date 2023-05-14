
// AttackCommand.cs

// Copyright (c) 2014+ by Kenneth Pedersen.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static LandOfTheMountainKing.Game.Plugin.Globals;

namespace LandOfTheMountainKing.Game.Commands
{
	[ClassMappings]
	public class AttackCommand : EamonRT.Game.Commands.AttackCommand, IAttackCommand
	{
		public override void Execute()
		{
			Debug.Assert(DobjArtifact != null || DobjMonster != null);

			var necklaceArtifact = gADB[27];

			Debug.Assert(necklaceArtifact != null);

			// Find necklace with locket on Damian's body

			if ((BlastSpell || ActorMonster.Weapon > 0) && DobjArtifact != null && DobjArtifact.Uid == 26 && necklaceArtifact.IsInLimbo())
			{
				base.Execute();

				gOut.Print("Around his neck you find a necklace, which you take.");

				if (ActorMonster.CanCarryArtifactWeight(necklaceArtifact))
				{
					necklaceArtifact.SetCarriedByMonster(ActorMonster);
				}
				else
				{
					necklaceArtifact.SetInRoom(ActorRoom);
				}
			}
			else
			{
				base.Execute();
			}
		}
	}
}
