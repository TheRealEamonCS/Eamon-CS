
// RequestCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static Firestorm.Game.Plugin.Globals;

namespace Firestorm.Game.Commands
{
	[ClassMappings]
	public class RequestCommand : EamonRT.Game.Commands.RequestCommand, IRequestCommand
	{
		public override void ExecuteForPlayer()
		{
			Debug.Assert(DobjArtifact != null && IobjMonster != null);

			var broadTippedArrowsArtifact = gADB[61];

			Debug.Assert(broadTippedArrowsArtifact != null);

			// Request ornate brass key from Thorak Junior

			if (DobjArtifact.Uid == 60 && IobjMonster.Uid == 39 && IobjMonster.Reaction == Friendliness.Neutral)
			{
				if (broadTippedArrowsArtifact.IsCarriedByMonster(ActorMonster))
				{
					gEngine.PrintEffectDesc(58);

					broadTippedArrowsArtifact.SetInLimbo();

					if (ActorMonster.CanCarryArtifactWeight(DobjArtifact))
					{
						DobjArtifact.SetCarriedByMonster(ActorMonster);
					}
					else
					{
						DobjArtifact.SetInRoom(ActorRoom);
					}

					IobjMonster.Friendliness = Friendliness.Enemy;

					IobjMonster.Reaction = IobjMonster.Friendliness;
				}
				else
				{
					gEngine.PrintEffectDesc(57);
				}

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
			else
			{
				base.ExecuteForPlayer();
			}
		}
	}
}
