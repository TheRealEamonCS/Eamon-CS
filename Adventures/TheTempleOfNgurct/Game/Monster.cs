
// Monster.cs

// Copyright (c) 2014+ by Michael Penner.  All rights reserved.

using System;
using System.Diagnostics;
using System.Text;
using Eamon.Framework;
using Eamon.Game.Attributes;
using static TheTempleOfNgurct.Game.Plugin.PluginContext;

namespace TheTempleOfNgurct.Game
{
	[ClassMappings]
	public class Monster : Eamon.Game.Monster, IMonster
	{
		public override bool HasHumanNaturalAttackDescs()
		{
			return Uid == 26;
		}

		public override void AddHealthStatus(StringBuilder buf, bool addNewLine = true)
		{
			string result = null;

			if (buf == null)
			{
				// PrintError

				goto Cleanup;
			}

			if (IsDead())
			{
				result = Globals.FireDamage ? "burnt to a crisp!" : "dead!";       // Crispy critter
			}
			else
			{
				var x = DmgTaken;

				x = (((long)((double)(x * 5) / (double)Hardiness)) + 1) * (x > 0 ? 1 : 0);

				result = Globals.FireDamage ? "very badly burned." : "at death's door, knocking loudly.";

				if (x == 4)
				{
					result = Globals.FireDamage ? "badly scorched." : "very badly injured.";
				}
				else if (x == 3)
				{
					result = Globals.FireDamage ? "scorched." : "badly injured.";
				}
				else if (x == 2)
				{
					result = Globals.FireDamage ? "singed." : "lightly injured.";
				}
				else if (x == 1)
				{
					result = Globals.FireDamage ? "still in good shape." : "in good shape.";
				}
				else if (x < 1)
				{
					result = "in perfect health.";
				}
			}

			Debug.Assert(result != null);

			buf.AppendFormat("{0}{1}", result, addNewLine ? Environment.NewLine : "");

		Cleanup:

			;
		}
	}
}
