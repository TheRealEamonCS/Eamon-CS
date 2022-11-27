
// ReadCommand.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
using Eamon.Game.Attributes;
using EamonRT.Framework.Commands;
using EamonRT.Framework.States;
using static ThePyramidOfAnharos.Game.Plugin.Globals;

namespace ThePyramidOfAnharos.Game.Commands
{
	[ClassMappings]
	public class ReadCommand : EamonRT.Game.Commands.ReadCommand, IReadCommand
	{
		public override void Execute()
		{
			Debug.Assert(DobjArtifact != null);

			// door / glyphs

			if (DobjArtifact.Uid == 76 || DobjArtifact.Uid == 77)
			{
				switch(gGameState.Ro)
				{
					case 6:

						gEngine.PrintTheGlyphsRead(26);

						NextState = gEngine.CreateInstance<IMonsterStartState>();

						break;

					case 29:

						gEngine.PrintTheGlyphsRead(25);

						NextState = gEngine.CreateInstance<IMonsterStartState>();

						break;

					case 31:

						gEngine.PrintTheGlyphsRead(18);

						NextState = gEngine.CreateInstance<IMonsterStartState>();

						break;

					default:

						base.Execute();
						
						break;
				}
			}
			else
			{
				base.Execute();
			}
		}
	}
}
