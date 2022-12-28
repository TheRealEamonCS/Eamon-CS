
// RequestCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.Globals;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class RequestCommand : Command, IRequestCommand
	{
		public override void Execute()
		{
			RetCode rc;

			Debug.Assert(DobjArtifact != null && IobjMonster != null);

			if (IobjMonster.Reaction < Friendliness.Friend)
			{
				gEngine.PrintMonsterEmotes(IobjMonster);

				gOut.WriteLine();

				goto Cleanup;
			}

			if (!DobjArtifact.IsRequestable())
			{
				PrintObjBelongsToActor(DobjArtifact, IobjMonster);

				goto Cleanup;
			}

			if (!GetCommandCalled)
			{
				RedirectToGetCommand<IRequestCommand>(DobjArtifact, false);

				goto Cleanup;
			}

			if (!DobjArtifact.IsCarriedByCharacter())
			{
				if (DobjArtifact.DisguisedMonster == null)
				{
					NextState = gEngine.CreateInstance<IStartState>();
				}

				goto Cleanup;
			}

			if (IobjMonster.Weapon == DobjArtifact.Uid)
			{
				Debug.Assert(DobjArtifact.GeneralWeapon != null);

				rc = DobjArtifact.RemoveStateDesc(DobjArtifact.GetReadyWeaponDesc());

				Debug.Assert(gEngine.IsSuccess(rc));

				IobjMonster.Weapon = -1;
			}

			if (ActorMonster.Weapon <= 0 && DobjArtifact.IsReadyableByCharacter() && NextState == null)
			{
				NextState = gEngine.CreateInstance<IReadyCommand>();

				CopyCommandData(NextState as ICommand, false);
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
		}

		/*
		public override bool IsPrepEnabled(IPrep prep)
		{
			Debug.Assert(prep != null);

			PrepNames = new string[] { "from" };

			return PrepNames.FirstOrDefault(pn => prep.Name.Equals(pn, StringComparison.OrdinalIgnoreCase)) != null;
		}
		*/

		public RequestCommand()
		{
			SortOrder = 300;

			IsIobjEnabled = true;

			if (gEngine.IsRulesetVersion(5))
			{
				IsPlayerEnabled = false;
			}

			Name = "RequestCommand";

			Verb = "request";

			Type = CommandType.Interactive;
		}
	}
}
