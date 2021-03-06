
// AttackCommand.cs

// Copyright (c) 2014+ by Kenneth Pedersen.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static LandOfTheMountainKing.Game.Plugin.PluginContext;

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

				necklaceArtifact.SetCarriedByCharacter();			// TODO: put necklace in Room if its too heavy to be carried
			}
			else
			{
				base.Execute();
			}
		}
	}
}
