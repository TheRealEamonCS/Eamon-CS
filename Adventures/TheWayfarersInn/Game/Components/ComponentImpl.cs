
// ComponentImpl.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Game.Attributes;
using EamonRT.Framework.Components;
using static TheWayfarersInn.Game.Plugin.Globals;

namespace TheWayfarersInn.Game.Components
{
	[ClassMappings]
	public class ComponentImpl : EamonRT.Game.Components.ComponentImpl, IComponentImpl
	{
		public override void PrintWhamHitObj(IArtifact artifact)
		{
			// Armoire

			if (artifact != null && artifact.Uid == 184 && artifact.GeneralContainer.GetBreakageStrength() > 0)
			{
				gOut.Print("Wham! You hit {0} lock!", artifact.GetTheName());
			}
			else
			{
				base.PrintWhamHitObj(artifact);
			}
		}

		public override void PrintSmashesToPieces(IRoom room, IArtifact artifact, bool spillContents)
		{
			// Crumbling brick wall

			if (artifact.Uid == 53)
			{
				gEngine.PrintEffectDesc(20);
			}
			else
			{
				base.PrintSmashesToPieces(room, artifact, spillContents);
			}
		}
	}
}
