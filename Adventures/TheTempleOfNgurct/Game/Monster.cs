
// Monster.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Text;
using Eamon.Framework;
using Eamon.Game.Attributes;
using static TheTempleOfNgurct.Game.Plugin.Globals;

namespace TheTempleOfNgurct.Game
{
	[ClassMappings]
	public class Monster : Eamon.Game.Monster, IMonster
	{
		public override bool HasHumanNaturalAttackDescs()
		{
			return Uid == 26;
		}

		public override void AddHealthStatus(StringBuilder buf, bool appendNewLine = true)
		{
			string result = null;

			if (buf == null)
			{
				// PrintError

				goto Cleanup;
			}

			if (IsDead())
			{
				result = gEngine.FireDamage ? "burnt to a crisp!" : "dead!";       // Crispy critter
			}
			else
			{
				var x = DmgTaken;

				x = (((long)((double)(x * 5) / (double)Hardiness)) + 1) * (x > 0 ? 1 : 0);

				result = gEngine.FireDamage ? "very badly burned." : "at death's door, knocking loudly.";

				if (x == 4)
				{
					result = gEngine.FireDamage ? "badly scorched." : "very badly injured.";
				}
				else if (x == 3)
				{
					result = gEngine.FireDamage ? "scorched." : "badly injured.";
				}
				else if (x == 2)
				{
					result = gEngine.FireDamage ? "singed." : "lightly injured.";
				}
				else if (x == 1)
				{
					result = gEngine.FireDamage ? "still in good shape." : "in good shape.";
				}
				else if (x < 1)
				{
					result = "in perfect health.";
				}
			}

			Debug.Assert(result != null);

			buf.AppendFormat("{0}{1}", result, appendNewLine ? Environment.NewLine : "");

		Cleanup:

			;
		}
	}
}
