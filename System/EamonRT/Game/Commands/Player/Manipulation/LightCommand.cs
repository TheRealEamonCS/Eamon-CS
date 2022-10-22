
// LightCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.Globals;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class LightCommand : Command, ILightCommand
	{
		/// <summary></summary>
		public virtual IArtifactCategory DobjArtAc { get; set; }

		/// <summary></summary>
		public virtual IArtifact LsArtifact { get; set; }

		public override void Execute()
		{
			RetCode rc;

			Debug.Assert(DobjArtifact != null);

			DobjArtAc = DobjArtifact.LightSource;

			if (DobjArtAc == null)
			{
				if (ActorMonster.IsInRoomLit() || DobjArtifact.IsCarriedByCharacter())
				{
					PrintCantVerbObj(DobjArtifact);
				}

				NextState = gEngine.CreateInstance<IStartState>();

				goto Cleanup;
			}

			if (!DobjArtifact.IsUnmovable())
			{
				if (!DobjArtifact.IsCarriedByCharacter())
				{
					if (!GetCommandCalled)
					{
						RedirectToGetCommand<ILightCommand>(DobjArtifact);
					}
					else if (DobjArtifact.DisguisedMonster == null)
					{
						NextState = gEngine.CreateInstance<IStartState>();
					}

					goto Cleanup;
				}
			}

			if (DobjArtAc.Field1 == 0)
			{
				PrintWontLight(DobjArtifact);

				NextState = gEngine.CreateInstance<IMonsterStartState>();

				goto Cleanup;
			}

			if (gGameState.Ls == DobjArtifact.Uid)
			{
				PrintExtinguishObj(DobjArtifact);

				gEngine.Buf.Clear();

				rc = gEngine.In.ReadField(gEngine.Buf, gEngine.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				if (gEngine.Buf.Length > 0 && gEngine.Buf[0] == 'Y')
				{
					rc = DobjArtifact.RemoveStateDesc(DobjArtifact.GetProvidingLightDesc());

					Debug.Assert(gEngine.IsSuccess(rc));

					gGameState.Ls = 0;

					PrintLightExtinguished(DobjArtifact);
				}

				NextState = gEngine.CreateInstance<IMonsterStartState>();

				goto Cleanup;
			}

			if (gGameState.Ls > 0)
			{
				LsArtifact = gADB[gGameState.Ls];

				Debug.Assert(LsArtifact != null && LsArtifact.LightSource != null);

				gEngine.LightOut(LsArtifact);
			}

			rc = DobjArtifact.AddStateDesc(DobjArtifact.GetProvidingLightDesc());

			Debug.Assert(gEngine.IsSuccess(rc));

			gGameState.Ls = DobjArtifact.Uid;

			PrintLightObj(DobjArtifact);

		Cleanup:

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
		}

		public LightCommand()
		{
			SortOrder = 170;

			IsDarkEnabled = true;

			if (gEngine.IsRulesetVersion(5))
			{
				IsPlayerEnabled = false;
			}

			Name = "LightCommand";

			Verb = "light";

			Type = CommandType.Manipulation;
		}
	}
}
