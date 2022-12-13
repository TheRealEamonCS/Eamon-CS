
// Artifact.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System.Diagnostics;
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
					// Map

					if (Uid == 51)
					{
						var reliquariesArtifact = gADB[32];

						Debug.Assert(reliquariesArtifact != null);

						var sarcophagusArtifact = gADB[33];

						Debug.Assert(sarcophagusArtifact != null);

						// Map vanishes when sarcophagus is closed

						if (reliquariesArtifact.IsInLimbo() && !sarcophagusArtifact.InContainer.IsOpen())
						{
							result = gEngine.LimboLocation;
						}
					}

					// Door / Arch

					else if (Uid == 76)
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

					// Pyramid / Floor

					else if (Uid == 77)
					{
						if (gGameState.Ro == 12)
						{
							Name = "pyramid";

							ArticleType = ArticleType.A;

							Synonyms = null;

							result = gGameState.Ro;
						}
						else if (gGameState.Ro > 13 && gGameState.Ro < 43)
						{
							Name = "floor";

							ArticleType = ArticleType.The;

							Synonyms = new string[] { "dust" };

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
