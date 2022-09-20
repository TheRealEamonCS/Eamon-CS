
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
using static EamonRT.Game.Plugin.PluginContext;

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

				NextState = Globals.CreateInstance<IStartState>();

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
						NextState = Globals.CreateInstance<IStartState>();
					}

					goto Cleanup;
				}
			}

			if (DobjArtAc.Field1 == 0)
			{
				PrintWontLight(DobjArtifact);

				NextState = Globals.CreateInstance<IMonsterStartState>();

				goto Cleanup;
			}

			if (gGameState.Ls == DobjArtifact.Uid)
			{
				PrintExtinguishObj(DobjArtifact);

				Globals.Buf.Clear();

				rc = Globals.In.ReadField(Globals.Buf, Constants.BufSize02, null, ' ', '\0', false, null, gEngine.ModifyCharToUpper, gEngine.IsCharYOrN, null);

				Debug.Assert(gEngine.IsSuccess(rc));

				if (Globals.Buf.Length > 0 && Globals.Buf[0] == 'Y')
				{
					rc = DobjArtifact.RemoveStateDesc(DobjArtifact.GetProvidingLightDesc());

					Debug.Assert(gEngine.IsSuccess(rc));

					gGameState.Ls = 0;

					PrintLightExtinguished(DobjArtifact);
				}

				NextState = Globals.CreateInstance<IMonsterStartState>();

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
				NextState = Globals.CreateInstance<IMonsterStartState>();
			}
		}

		public LightCommand()
		{
			SortOrder = 170;

			IsDarkEnabled = true;

			if (Globals.IsRulesetVersion(5))
			{
				IsPlayerEnabled = false;
			}

			Name = "LightCommand";

			Verb = "light";

			Type = CommandType.Manipulation;
		}
	}
}
