
// Monster.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Text;
using Eamon.Framework;
using Eamon.Framework.Primitive.Enums;
using Eamon.Game.Attributes;
using static TheBeginnersCave.Game.Plugin.Globals;

namespace TheBeginnersCave.Game
{
	[ClassMappings]
	public class Monster : Eamon.Game.Monster, IMonster
	{
		public override long Weapon
		{
			get
			{
				return base.Weapon;
			}

			set
			{
				if (gEngine.EnableMutateProperties)
				{
					// If this is any monster going from wielding Trollsfire to not wielding Trollsfire

					if (base.Weapon == 10 && value != 10)
					{
						// Deactivate Trollsfire effect; the Trollsfire property is complex and does a fair bit of processing

						gGameState.Trollsfire = 0;
					}

					// If this is the pirate going from not wielding Trollsfire to wielding Trollsfire

					else if (Uid == 8 && base.Weapon != 10 && value == 10)
					{
						// Activate Trollsfire effect

						gGameState.Trollsfire = 1;
					}
				}

				base.Weapon = value;
			}
		}

		public override bool ShouldRefuseToAcceptGold()
		{
			return false;
		}

		public override bool ShouldRefuseToAcceptGift(IArtifact artifact)
		{
			Debug.Assert(artifact != null);

			return !gEngine.IsRulesetVersion(5, 62) && (Reaction == Friendliness.Enemy || (Reaction == Friendliness.Neutral && artifact.Value < 3000));
		}

		public override void AddHealthStatus(StringBuilder buf, bool appendNewLine = true)
		{
			string result = null;

			if (buf == null)
			{
				// PrintError

				goto Cleanup;
			}

			var index = gEngine.GetMonsterHealthStatusIndex(Hardiness, DmgTaken);

			// Historical status reports from original

			if (index > 5)
			{
				result = "dead!";
			}
			else if (index == 5)
			{
				result = "at death's door, knocking loudly.";
			}
			else if (index == 4)
			{
				result = "very badly injured.";
			}
			else if (index == 3)
			{
				result = "in pain.";
			}
			else if (index == 2)
			{
				result = "hurting.";
			}
			else if (index == 1)
			{
				var str = buf.ToString();

				if (str.EndsWith("You are ", StringComparison.OrdinalIgnoreCase))
				{
					buf.Length -= 4;
				}
				else if (str.Length > 3 && str.Substring(str.Length - 4).EndsWith(" is ", StringComparison.OrdinalIgnoreCase))
				{
					buf.Length -= 3;
				}

				str = buf.ToString();

				if (str.EndsWith("They are ", StringComparison.OrdinalIgnoreCase))
				{
					result = "taking damage but still in good shape.";
				}
				else if (str.EndsWith("You ", StringComparison.OrdinalIgnoreCase))
				{
					result = "have taken damage but are still in good shape.";
				}
				else
				{
					result = "has taken damage but is still in good shape.";
				}
			}
			else if (index < 1)
			{
				result = "in perfect health.";
			}

			Debug.Assert(result != null);

			buf.AppendFormat("{0}{1}", result, appendNewLine ? Environment.NewLine : "");

		Cleanup:

			;
		}
	}
}
