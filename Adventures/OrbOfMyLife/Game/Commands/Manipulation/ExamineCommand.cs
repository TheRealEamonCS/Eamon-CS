
// ExamineCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Linq;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static OrbOfMyLife.Game.Plugin.Globals;

namespace OrbOfMyLife.Game.Commands
{
	[ClassMappings]
	public class ExamineCommand : EamonRT.Game.Commands.ExamineCommand, IExamineCommand
	{
		public override void ExecuteForPlayer()
		{
			Debug.Assert(DobjArtifact != null || DobjMonster != null);

			var keyOfIskArtifact = gADB[5];

			Debug.Assert(keyOfIskArtifact != null);

			var letterArtifact = gADB[35];

			Debug.Assert(letterArtifact != null);

			// DeadBody

			if (DobjArtifact?.DeadBody != null)
			{
				var monster = gEngine.GetMonsterList(m => m.DeadBody == DobjArtifact.Uid).FirstOrDefault();

				Debug.Assert(monster != null);

				gOut.Print("Yes, {0}'s dead!", monster.EvalGender("he", "she", "it"));

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}

			// Silver tube

			else if (DobjArtifact?.Uid == 6 && DobjArtifact.Name.Equals("silver tube", StringComparison.OrdinalIgnoreCase))
			{
				DobjArtifact.Name = "Sword of Light";

				DobjArtifact.PluralType = PluralType.None;

				DobjArtifact.ArticleType = ArticleType.The;

				gEngine.PrintEffectDesc(24);

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}

			// Crystal ball

			else if (DobjArtifact?.Uid == 8)             /* && ContainerType == ContainerType.In */
			{
				gEngine.PrintEffectDesc(10);

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}

			// Globe

			else if (DobjArtifact?.Uid == 18)
			{
				gEngine.PrintEffectDesc(12);

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}

			// Log

			else if (DobjArtifact?.Uid == 33 && keyOfIskArtifact.IsInLimbo())
			{
				gOut.Print("You find something on {0}!", DobjArtifact.GetTheName());

				keyOfIskArtifact.SetInRoom(ActorRoom);

				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
			else
			{
				base.ExecuteForPlayer();
			}

			// Sagonne's letter

			if (DobjMonster?.Uid == 22 && letterArtifact.IsInLimbo())
			{
				letterArtifact.SetInRoom(ActorRoom);
			}
		}
	}
}
