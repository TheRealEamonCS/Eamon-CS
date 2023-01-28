
// GoCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.Globals;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class GoCommand : Command, IGoCommand
	{
		/// <summary></summary>
		public virtual IArtifactCategory DobjArtAc { get; set; }

		public override void Execute()
		{
			Debug.Assert(DobjArtifact != null);

			DobjArtAc = DobjArtifact.DoorGate;

			if (DobjArtAc == null)
			{
				PrintDontFollowYou();

				NextState = gEngine.CreateInstance<IStartState>();

				goto Cleanup;
			}

			NextState = gEngine.CreateInstance<IBeforePlayerMoveState>(x =>
			{
				x.DoorGateArtifact = DobjArtifact;
			});

		Cleanup:

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
		}

		public GoCommand()
		{
			Synonyms = new string[] { "enter" };

			SortOrder = 97;

			IsDobjPrepEnabled = true;

			if (gEngine.IsRulesetVersion(5, 62))
			{
				IsPlayerEnabled = false;
			}

			Name = "GoCommand";

			Verb = "go";

			Type = CommandType.Movement;
		}
	}
}
