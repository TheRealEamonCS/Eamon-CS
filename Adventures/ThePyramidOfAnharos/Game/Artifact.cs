
// Artifact.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Game.Attributes;
using static ThePyramidOfAnharos.Game.Plugin.Globals;

namespace ThePyramidOfAnharos.Game
{
	[ClassMappings]
	public class Artifact : Eamon.Game.Artifact, IArtifact
	{
		public override long Location 
		{ 
			get
			{
				return base.Location;
			}

			set
			{
				if (gEngine.EnableMutateProperties)
				{
					// water bag

					if (Uid == 12)
					{
						// Note: using hardcoded Location for simplicity

						if (base.Location == -1 && value != -1)
						{
							Field2 = gGameState.KW;

							if (gGameState.KW > 5)
							{
								gGameState.KW = 5;
							}
						}
						else if (base.Location != -1 && value == -1)
						{
							if (Field2 > gGameState.KW)
							{
								gGameState.KW = Field2;
							}
						}
					}
				}

				base.Location = value;
			}
		}
	}
}
