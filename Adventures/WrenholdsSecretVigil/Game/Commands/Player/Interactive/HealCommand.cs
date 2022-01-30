
// HealCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using static WrenholdsSecretVigil.Game.Plugin.PluginContext;

namespace WrenholdsSecretVigil.Game.Commands
{
	[ClassMappings]
	public class HealCommand : EamonRT.Game.Commands.HealCommand, IHealCommand
	{
		public override void Execute()
		{
			var medallionArtifact = gADB[10];

			Debug.Assert(medallionArtifact != null);

			if (medallionArtifact.IsCarriedByCharacter() && gGameState.MedallionCharges > 0)
			{
				gOut.Print("{0} feel{1} warm in your hand!", medallionArtifact.GetTheName(true), medallionArtifact.EvalPlural("s", ""));

				gGameState.MedallionCharges--;

				CastSpell = false;
			}

			base.Execute();
		}
	}
}
