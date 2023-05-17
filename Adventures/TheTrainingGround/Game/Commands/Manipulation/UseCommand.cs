
// UseCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static TheTrainingGround.Game.Plugin.Globals;

namespace TheTrainingGround.Game.Commands
{
	[ClassMappings]
	public class UseCommand : EamonRT.Game.Commands.UseCommand, IUseCommand
	{
		public override void ExecuteForPlayer()
		{
			Debug.Assert(DobjArtifact != null);

			// Hammer of Thor

			if (DobjArtifact.Uid == 24)
			{
				var monsterList = gEngine.GetMonsterList(m => m.IsInRoom(ActorRoom) && m.Reaction == Friendliness.Enemy && m.Field1 == 0);

				foreach (var monster in monsterList)
				{
					monster.Courage /= 4;

					monster.Field1 = 1;
				}

				gEngine.PrintEffectDesc(32);

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
			else
			{
				base.ExecuteForPlayer();
			}
		}
	}
}
