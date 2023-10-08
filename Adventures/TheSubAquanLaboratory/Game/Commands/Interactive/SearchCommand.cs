
// SearchCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static TheSubAquanLaboratory.Game.Plugin.Globals;

namespace TheSubAquanLaboratory.Game.Commands
{
	[ClassMappings]
	public class SearchCommand : EamonRT.Game.Commands.Command, Framework.Commands.ISearchCommand
	{
		public override void PrintCantVerbObj(IGameBase obj)
		{
			Debug.Assert(obj != null);

			gOut.Print("You can only {0} dead bodies.", Verb);
		}

		public override void ExecuteForPlayer()
		{
			Debug.Assert(DobjArtifact != null);

			switch (DobjArtifact.Uid)
			{
				case 89:

					// Dismantled worker android

					var plasticCardArtifact = gADB[82];

					Debug.Assert(plasticCardArtifact != null);

					if (plasticCardArtifact.IsInLimbo())
					{
						gOut.Print("{0}", plasticCardArtifact.Desc);

						plasticCardArtifact.SetInRoom(ActorRoom);

						plasticCardArtifact.Seen = true;

						var command = gEngine.CreateInstance<IGetCommand>();

						CopyCommandData(command);

						command.Dobj = plasticCardArtifact;

						NextState = command;
					}
					else
					{
						PrintNothingFound(DobjArtifact);
					}

					goto Cleanup;

				default:

					if (DobjArtifact.DeadBody != null && DobjArtifact.Uid != 107)
					{
						PrintNothingFound(DobjArtifact);
					}
					else
					{ 
						PrintCantVerbObj(DobjArtifact);

						NextState = gEngine.CreateInstance<IStartState>();
					}

					goto Cleanup;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
		}

		public virtual void PrintNothingFound(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			gOut.Print("Searching {0} reveals nothing of interest.", artifact.GetTheName());
		}

		public SearchCommand()
		{
			SortOrder = 460;

			IsNew = true;

			Name = "SearchCommand";

			Verb = "search";

			Type = CommandType.Interactive;
		}
	}
}
