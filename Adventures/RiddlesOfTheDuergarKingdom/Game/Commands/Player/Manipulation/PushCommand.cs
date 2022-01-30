﻿
// PushCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.States;
using static RiddlesOfTheDuergarKingdom.Game.Plugin.PluginContext;

namespace RiddlesOfTheDuergarKingdom.Game.Commands
{
	[ClassMappings]
	public class PushCommand : EamonRT.Game.Commands.Command, Framework.Commands.IPushCommand
	{

		public override void Execute()
		{
			Debug.Assert(DobjArtifact != null);

			switch (DobjArtifact.Uid)
			{
				case 46:    // ore cart

					if (DobjArtifact.IsCarriedByContainer())
					{
						if (gGameState.PushingOreCart)
						{
							gOut.Print("You decide there are better things to do than push around an ore cart.");

							gGameState.PushingOreCart = false;
						}
						else if (gGameState.GetNBTL(Friendliness.Enemy) > 0)
						{
							PrintEnemiesNearby();

							NextState = Globals.CreateInstance<IStartState>();
						}
						else
						{
							gEngine.PrintEffectDesc(64);

							gGameState.PushingOreCart = true;
						}
					}
					else
					{
						gOut.Print("The ore cart isn't going anywhere while lying on its side.");

						NextState = Globals.CreateInstance<IStartState>();
					}

					goto Cleanup;

				default:

					PrintCantVerbObj(DobjArtifact);

					NextState = Globals.CreateInstance<IStartState>();

					goto Cleanup;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		public PushCommand()
		{
			SortOrder = 460;

			IsNew = true;

			Uid = 97;

			Name = "PushCommand";

			Verb = "push";

			Type = CommandType.Manipulation;
		}
	}
}
