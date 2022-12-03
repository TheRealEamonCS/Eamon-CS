
// Artifact.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
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
				var result = base.Location;

				if (gEngine.EnableMutateProperties)
				{
					// Door / Arch

					if (Uid == 76)
					{
						if (gGameState.Ro == 6)
						{
							IsPlural = false;

							PluralType = PluralType.S;

							Name = "door";

							ArticleType = ArticleType.A;

							result = gGameState.Ro;
						}
						else if (gGameState.Ro == 29)
						{
							IsPlural = false;

							PluralType = PluralType.Es;

							Name = "arch";

							ArticleType = ArticleType.An;

							result = gGameState.Ro;
						}
					}
				}

				return result;
			}

			set
			{
				if (gEngine.EnableMutateProperties)
				{
					// Water bag

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
