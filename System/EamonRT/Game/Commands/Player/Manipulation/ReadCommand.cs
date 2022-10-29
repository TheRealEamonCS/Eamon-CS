
// ReadCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon;
using Eamon.Framework;
using Eamon.Framework.Primitive.Classes;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using Eamon.Game.Extensions;
using EamonRT.Framework.Commands;
using EamonRT.Framework.Primitive.Enums;
using EamonRT.Framework.States;
using static EamonRT.Game.Plugin.Globals;

namespace EamonRT.Game.Commands
{
	[ClassMappings]
	public class ReadCommand : Command, IReadCommand
	{
		/// <summary></summary>
		public virtual IArtifactCategory DisguisedMonsterAc { get; set; }

		/// <summary></summary>
		public virtual IArtifactCategory ReadableAc { get; set; }

		/// <summary></summary>
		public virtual IArtifactCategory DobjArtAc { get; set; }

		/// <summary></summary>
		public virtual IEffect ReadEffect { get; set; }

		/// <summary></summary>
		public virtual long ReadEffectIndex { get; set; }

		public override void Execute()
		{
			RetCode rc;

			Debug.Assert(DobjArtifact != null);

			DisguisedMonsterAc = DobjArtifact.DisguisedMonster;

			ReadableAc = DobjArtifact.Readable;

			DobjArtAc = DisguisedMonsterAc != null ? DisguisedMonsterAc :
						ReadableAc;

			if (DobjArtAc == null)
			{
				PrintCantVerbObj(DobjArtifact);

				NextState = gEngine.CreateInstance<IStartState>();

				goto Cleanup;
			}

			if (DobjArtAc.Type == ArtifactType.DisguisedMonster)
			{
				gEngine.RevealDisguisedMonster(ActorRoom, DobjArtifact);

				NextState = gEngine.CreateInstance<IStartState>();

				goto Cleanup;
			}

			if (!DobjArtAc.IsOpen())
			{
				PrintMustFirstOpen(DobjArtifact);

				NextState = gEngine.CreateInstance<IStartState>();

				goto Cleanup;
			}

			ProcessEvents(EventType.BeforePrintArtifactReadText);

			if (GotoCleanup)
			{
				goto Cleanup;
			}

			for (ReadEffectIndex = 0; ReadEffectIndex < DobjArtAc.Field2; ReadEffectIndex++)
			{
				ReadEffect = gEDB[DobjArtAc.Field1 + ReadEffectIndex];

				if (ReadEffect != null)
				{
					gEngine.Buf.Clear();

					rc = ReadEffect.BuildPrintedFullDesc(gEngine.Buf);
				}
				else
				{
					gEngine.Buf.SetPrint("{0}", "???");

					rc = RetCode.Success;
				}

				Debug.Assert(gEngine.IsSuccess(rc));

				gOut.Write("{0}", gEngine.Buf);
			}

			ProcessEvents(EventType.AfterReadArtifact);

			if (GotoCleanup)
			{
				goto Cleanup;
			}

		Cleanup:

			if (NextState == null)
			{
				NextState = gEngine.CreateInstance<IMonsterStartState>();
			}
		}

		public ReadCommand()
		{
			SortOrder = 200;

			if (gEngine.IsRulesetVersion(5))
			{
				IsPlayerEnabled = false;
			}

			Name = "ReadCommand";

			Verb = "read";

			Type = CommandType.Manipulation;
		}
	}
}
